using Windows.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Uno.Extensions;
using Uno.Logging;
using Uno.UI.DataBinding;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Markup;
using Uno.UI;
using Windows.Foundation.Collections;
using Microsoft.Extensions.Logging;

#if XAMARIN_IOS
using UIKit;
#elif __MACOS__
using AppKit;
#endif

namespace Windows.UI.Xaml
{
	[ContentProperty(Name = "States")]
	public sealed partial class VisualStateGroup : DependencyObject
	{
		public VisualStateGroup()
		{
			IsAutoPropertyInheritanceEnabled = false;
			InitializeBinder();

			this.RegisterParentChangedCallback(this, OnParentChanged);
		}

		public string Name { get; set; }

#region States Dependency Property

		public IList<VisualState> States
		{
			get
			{
				var value = (IList<VisualState>)this.GetValue(StatesProperty);

				if (value == null)
				{
					// WARNING: if you remove this initialization, make sure to review 
					// the GetChildrenProviders method, which assumes that the property
					// may return null for performance reasons.
					var collection = new DependencyObjectCollection<VisualState>(parent: this, isAutoPropertyInheritanceEnabled: false);
					value = collection;

					this.SetValue(StatesProperty, value);
				}

				return value;
			}
			internal set { this.SetValue(StatesProperty, value); }
		}

		// Using a DependencyProperty as the backing store for States.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StatesProperty =
			DependencyProperty.Register(
				"States",
				typeof(IList<VisualState>),
				typeof(VisualStateGroup),
				new FrameworkPropertyMetadata(
					defaultValue: null,
					options: FrameworkPropertyMetadataOptions.ValueInheritsDataContext,
					propertyChangedCallback: (s, e) => ((VisualStateGroup)s)?.OnStatesChanged(e)
				)
			);

#endregion

#region Transitions DependencyProperty

		public IList<VisualTransition> Transitions
		{
			get
			{
				var value = (IList<VisualTransition>)this.GetValue(TransitionsProperty);

				if (value == null)
				{
					value = new DependencyObjectCollection<VisualTransition>(parent: this, isAutoPropertyInheritanceEnabled: false);
					this.SetValue(TransitionsProperty, value);
				}

				return value;
			}
			internal set { this.SetValue(TransitionsProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Transitions.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TransitionsProperty =
			DependencyProperty.Register(
				"Transitions",
				typeof(IList<VisualTransition>),
				typeof(VisualStateGroup),
				new FrameworkPropertyMetadata(
					defaultValue: null,
					propertyChangedCallback: (s, e) => ((VisualStateGroup)s)?.OnTransitionsChanged(e)
				)
			);

#endregion

		//Adds Event Handlers when collections changed
		private void OnStatesChanged(DependencyPropertyChangedEventArgs e)
		{
			if (e.OldValue is DependencyObjectCollection<VisualState> oldStates)
			{
				oldStates.VectorChanged -= VisualStateChanged;
			}

			if (e.NewValue is DependencyObjectCollection<VisualState> states)
			{
				states.VectorChanged += VisualStateChanged;
			}

			RefreshStateTriggers();
		}

		//Modifies the Owner FrameworkElement when Collection is modified
		private void VisualStateChanged(object sender, IVectorChangedEventArgs e)
		{
			RefreshStateTriggers();
		}

		//Adds Event Handlers when collections changed
		private void OnTransitionsChanged(DependencyPropertyChangedEventArgs e)
		{
		}

		public VisualState CurrentState { get; internal set; }

		public event VisualStateChangedEventHandler CurrentStateChanging;

		public event VisualStateChangedEventHandler CurrentStateChanged;

		internal void RaiseCurrentStateChanging(Control control, VisualState oldState, VisualState newState)
		{
			CurrentStateChanging?.Invoke(this, new VisualStateChangedEventArgs
			{
				Control = control,
				OldState = oldState,
				NewState = newState
			});
		}

		internal void RaiseCurrentStateChanged(Control control, VisualState oldState, VisualState newState)
		{
			CurrentStateChanged?.Invoke(this, new VisualStateChangedEventArgs
			{
				Control = control,
				OldState = oldState,
				NewState = newState
			});
		}

		internal void GoToState(IFrameworkElement element, VisualState oldState, VisualState newState, bool useTransitions, Action onStateChanged)
		{
			if (this.Log().IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug))
			{
				this.Log().DebugFormat("Go to state [{0}/{1}] on [{2}]", Name, newState?.Name, element);
			}

			var transition = FindTransition(oldState?.Name, newState?.Name);

			// The CurrentState is the target state even before raising the 'StateChanging' event!
			CurrentState = newState;

			// First step is to cleanup the current state
			if (oldState != null)
			{
				// Stops 'oldState' Storyboard animation
				if (oldState.Storyboard != null)
				{

					// TODO: We must wait for the "TurnOver" Animation to complete before staring the target state animation

					if (transition != null)
					{
						oldState.Storyboard.TurnOverAnimationsTo(transition.Storyboard);
					}

					if (transition?.Storyboard != null)
					{
						oldState.Storyboard.TurnOverAnimationsTo(transition.Storyboard);
					}
					else if (newState?.Storyboard != null)
					{
						oldState.Storyboard.TurnOverAnimationsTo(newState.Storyboard);
					}
					else
					{
						oldState.Storyboard.Stop();
					}
				}

				// Revert 'oldState' Setters
				foreach (var setter in this.CurrentState.Setters.OfType<Setter>())
				{
					if (element != null && (newState?.Setters.OfType<Setter>().Any(o => o.HasSameTarget(setter, DependencyPropertyValuePrecedences.Animations, element)) ?? false))
					{
						// PERF: We clear the value of the current setter only if there isn't any setter in the target state
						// which changes the same target property.

						continue;
					}

					setter.ClearValue();
				}
			}

			

			if (newState != null && element != null)
			{
				// Apply 'Setters'
				foreach (var setter in this.CurrentState.Setters.OfType<Setter>())
				{
					setter.ApplyValue(DependencyPropertyValuePrecedences.Animations, element);
				}
			}

			// Run 'transition'
			if (transition?.Storyboard == null || !useTransitions)
			{
				OnTransitionCompleted(this, null);
			}
			else
			{
				transition.Storyboard.Completed += OnTransitionCompleted;
				transition.Storyboard.Begin();
			}

			// Start the 'newState' Storyboard animation


			void OnStoryboardCompleted(object s, object a)
			{
				onStateChanged();

				if (newState?.Storyboard == null)
				{
					return;
				}

				newState.Storyboard.Completed -= OnStoryboardCompleted;
			}

			void OnTransitionCompleted(object s, object a)
			{
				if (transition?.Storyboard != null && useTransitions)
				{
					transition.Storyboard.Completed -= OnTransitionCompleted;

					if (newState?.Storyboard != null)
					{
						transition.Storyboard.TurnOverAnimationsTo(newState.Storyboard);
					}
				}

				//Starts Storyboard Animation
				if (newState?.Storyboard == null)
				{
					OnStoryboardCompleted(this, null);
				}
				else
				{
					newState.Storyboard.Completed += OnStoryboardCompleted;
					newState.Storyboard.Begin();
				}
			}

			void Start
		}

		private VisualTransition FindTransition(string oldStateName, string newStateName)
		{
			var hasOldState = oldStateName.HasValue();
			var hasNewState = newStateName.HasValue();

			if (hasOldState && hasNewState)
			{
				var perfectMatch = Transitions.FirstOrDefault(vt =>
					string.Equals(vt.From, oldStateName) &&
					string.Equals(vt.To, newStateName));

				if (perfectMatch != null)
				{
					return perfectMatch;
				}
			}

			if (hasOldState)
			{
				var fromMatch = Transitions.FirstOrDefault(vt =>
					string.Equals(vt.From, oldStateName) &&
					vt.To == null);

				if (fromMatch != null)
				{
					return fromMatch;
				}
			}

			if (hasNewState)
			{
				var toMatch = Transitions.FirstOrDefault(vt =>
					vt.From == null &&
					string.Equals(vt.To, newStateName));

				return toMatch;
			}
		}

		internal void RefreshStateTriggers()
		{
			var activeVisualState = GetActiveTrigger();

			if (this.Log().IsEnabled(LogLevel.Debug))
			{
				this.Log().Debug($"[{this}].RefreshStateTriggers() activeState={activeVisualState}, oldState={CurrentState}");
			}

			if (this.GetParent() is IFrameworkElement root)
			{
				var ctrl = (root as FrameworkElement)?.FindFirstParent<Control>();

				// We use the VisualStateManager to effectively GoToState its protected overridable "RaiseStateChang<ing|ed>" are invoked
				VisualStateManager.GetVisualStateManager(ctrl).GoToStateCore(ctrl, root, activeVisualState.Name, this, activeVisualState, true);
			}
		}

		private void OnParentChanged(object instance, object key, DependencyObjectParentChangedEventArgs args)
		{
			RefreshStateTriggers();
		}

		/// <remarks>
		/// This method is not using LINQ for performance considerations.
		/// </remarks>
		private VisualState GetActiveTrigger()
		{
			// As documented there:
			// https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.visualstategroup#remarks
			// We're using a priority-based mechanism

			// When using StateTriggers to control visual states, the trigger engine uses the following precedence
			// rules to score triggers and determine which trigger, and the corresponding VisualState, will be active:
			//
			// 1. Custom trigger that derives from StateTriggerBase
			// 2. AdaptiveTrigger activated due to MinWindowWidth
			// 3. AdaptiveTrigger activated due to MinWindowHeight
			//
			// If there are multiple active triggers at a time that have a conflict in scoring(i.e.two active custom
			// triggers), then the first one declared in the markup file takes precedence.

			var winningPrecedence2 = default(VisualState);
			var winningPrecedence3 = default(VisualState);

			for (var stateIndex = 0; stateIndex < States.Count; stateIndex++)
			{
				var state = States[stateIndex];
				for (var triggerIndex = 0; triggerIndex < state.StateTriggers.Count; triggerIndex++)
				{
					var trigger = state.StateTriggers[triggerIndex];

					if (trigger.CurrentPrecedence == StateTriggerPrecedence.CustomTrigger)
					{
						return state; // we have a winner!
					}
					if (trigger.CurrentPrecedence == StateTriggerPrecedence.MinWidthTrigger && winningPrecedence2 == null)
					{
						winningPrecedence2 = state;
						if (winningPrecedence3 != null)
						{
							break;
						}
					}
					else if (trigger.CurrentPrecedence == StateTriggerPrecedence.MinHeightTrigger && winningPrecedence3 == null)
					{
						winningPrecedence3 = state;
						if (winningPrecedence2 != null)
						{
							break;
						}
					}
				}

				if (winningPrecedence2 != null && winningPrecedence3 != null)
				{
					break;
				}
			}

			var winnerState = winningPrecedence2 ?? winningPrecedence3;
			return winnerState;
		}

		public override string ToString() => Name ?? $"<unnamed group {GetHashCode()}>";
	}
}
