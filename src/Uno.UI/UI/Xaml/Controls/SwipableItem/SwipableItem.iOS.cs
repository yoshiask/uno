#if __IOS__
using CoreGraphics;
using Uno.Extensions;
using System;
using UIKit;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows.UI.Xaml.Controls
{
	public partial class SwipableItem : ContentControl
	{
		private const double _translationDuration = 0.10; // In seconds

		private object _currentDataContext;
		private bool _hasAppliedTemplate = false;
		private bool _hasArrangedOnce = false;
		private bool _hasInitializedSnapPoints = false;
		private double _actualWidth;

		private UIElement _mainContainer;

		private double _translationWhenNotSwiped;
		private double _translationWhenSnappedFar;
		private double _translationWhenSnappedNear;
		private double _translation = 0;

		private Lazy<UITapGestureRecognizer> _outsideTapRecognizer;
		private Lazy<UIPanGestureRecognizer> _outsidePanRecognizer;

		public SwipableItem()
		{
#if !__ANDROID__ && !__IOS__
			this.DefaultStyleKey = typeof(SwipableItem);
#endif
			this.ClipsToBounds = true;

			// Since we can potentially add/remove this recognizer a lot, we make it lazy.
			_outsideTapRecognizer = new Lazy<UITapGestureRecognizer>(() =>
			{
				var recognizer = new UITapGestureRecognizer(OnOutsideTap);
				recognizer.Delegate = new SwipableItemOutsideGestureDelegate(this);
				recognizer.CancelsTouchesInView = false; // We only want to detect the tap we certainly don't want to cancel other gestures
				return recognizer;
			});

			_outsidePanRecognizer = new Lazy<UIPanGestureRecognizer>(() =>
			{
				var recognizer = new UIPanGestureRecognizer(OnOutsidePan);
				recognizer.Delegate = new SwipableItemOutsideGestureDelegate(this);
				recognizer.CancelsTouchesInView = false; // We only want to detect the pan we certainly don't want to cancel other gestures
				return recognizer;
			});
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_mainContainer = this.GetTemplateChild("MainPresenter") as UIElement;
			_mainContainer.Validation().NotNull("MainPresenter is a required Template Part in SwipableItem");

			var panGesture = new UIPanGestureRecognizer(Pan);
			panGesture.MaximumNumberOfTouches = 1;
			panGesture.Delegate = new SwipableItemGestureDelegate();
			_mainContainer.AddGestureRecognizer(panGesture);

			// Add a tap gesture in order to synchronize Android and iOS behavior
			var tapGesture = new UITapGestureRecognizer(Tap);
			tapGesture.CancelsTouchesInView = false;
			tapGesture.Delegate = new SwipableItemGestureDelegate();
			this.AddGestureRecognizer(tapGesture);

			_hasAppliedTemplate = true;
		}

		protected override void OnAfterArrange()
		{
			base.OnAfterArrange();

			_hasArrangedOnce = true;

			// Only calculate the snap points when the width changed because swipe is horizontal
			if (_actualWidth != ActualWidth)
			{
				InitializeSnapPoints();
				_actualWidth = ActualWidth;
			}
		}

		/// <summary>
		/// Indicates whether the DataContext is being applied.
		/// We use it as a way to know whether the view is being recycled, in which case we disable swipe transitions.
		/// </summary>
		private bool IsDataContextChanging => !ReferenceEquals(_currentDataContext, DataContext);

		protected override void OnDataContextChanged()
		{
			base.OnDataContextChanged();

			if (IsAutoReset)
			{
				SwipeState = SwipingState.NotSwiped;
			}

			_currentDataContext = DataContext;
		}

		private void InitializeSnapPoints()
		{
			if (_hasAppliedTemplate && _hasArrangedOnce)
			{
				_translationWhenNotSwiped = 0;
				_translationWhenSnappedNear = NearSnapWidth;
				_translationWhenSnappedFar = -FarSnapWidth;
				_hasInitializedSnapPoints = true;

				if (SwipeState.HasValue)
				{
					// Now that we have the snap points, make sure we align the main container accordingly.
					OnStateChanged(SwipeState.Value, useTransitions: false);
				}
			}
		}

		private void OnStateChanged(SwipingState newState, bool useTransitions)
		{
			// no point moving the container if the snap points are not ready.
			// OnStateChanged can be called before apply template in some cases.
			if (_hasInitializedSnapPoints)
			{
				MoveMainContainer(newState, useTransitions);
			}

			if (IsAutoReset)
			{
				UpdateOutsideRecognizers(newState);
			}
		}

		private void UpdateOutsideRecognizers(SwipingState newState)
		{
			// We register recognizers to look for taps and horizontal pans outside of the control 
			// so we can reset this control when it happens
			if (newState == SwipingState.NotSwiped)
			{
				UIApplication.SharedApplication.KeyWindow.RemoveGestureRecognizer(_outsideTapRecognizer.Value);
				UIApplication.SharedApplication.KeyWindow.RemoveGestureRecognizer(_outsidePanRecognizer.Value);
			}
			else if (newState == SwipingState.SwipedFar || newState == SwipingState.SwipedNear)
			{
				UIApplication.SharedApplication.KeyWindow.AddGestureRecognizer(_outsideTapRecognizer.Value);
				UIApplication.SharedApplication.KeyWindow.AddGestureRecognizer(_outsidePanRecognizer.Value);
			}
		}

		private void Tap(UITapGestureRecognizer gestureRecognizer)
		{
			if(gestureRecognizer.State == UIGestureRecognizerState.Ended)
			{
				if (IsAutoReset)
				{
					ResetSwipeState();
				}
			}
		}

		private void Pan(UIPanGestureRecognizer gestureRecognizer)
		{
			// When moving, we slide the item
			var presenter = gestureRecognizer.View;
			if (gestureRecognizer.State == UIGestureRecognizerState.Began ||
				gestureRecognizer.State == UIGestureRecognizerState.Changed ||
				gestureRecognizer.State == UIGestureRecognizerState.Ended
				)
			{
				var translation = gestureRecognizer.TranslationInView(this);

				_translation += translation.X;

				// Only translate horizontally
				// Clamp the translation to make sure we never go outside the ranges
				_translation = _translation.Clamp(_translationWhenSnappedFar, _translationWhenSnappedNear);
				presenter.Transform = CGAffineTransform.MakeTranslation((float)_translation, 0);

				// Reset the gesture recognizer's translation to {0, 0} - the next callback will get a delta from the current position.
				gestureRecognizer.SetTranslation(CGPoint.Empty, presenter);
			}

			// When gesture ends, we change the state of the control
			if (gestureRecognizer.State == UIGestureRecognizerState.Ended)
			{
				//If Centered first to avoid changing state to Far or Near is there are no near or farsnap points.
				if (_translation == _translationWhenNotSwiped)
				{
					ResetSwipeState();
				}
				//We validate the Center of the gesture is passed 50%.
				else if (_translation >= _translationWhenSnappedNear - (this.NearSnapWidth * (1 - SwipeDecisionPoint)))
				{
					if (SwipeState == SwipingState.SwipedNear)
					{
						// Force a OnStateChanged here if control is already in SwipedNear State.
						//(changing a DP to the same value won't trigger the OnValueChanged)
						OnStateChanged(SwipeState.Value, useTransitions: true);
					}
					else
					{
						SwipeState = SwipingState.SwipedNear;
					}
				}
				//We validate the Center of the gesture is passed 50%.
				else if (_translation <= _translationWhenSnappedFar + (this.FarSnapWidth * (1 - SwipeDecisionPoint)))
				{
					if (SwipeState == SwipingState.SwipedFar)
					{
						// Force a OnStateChanged here if control is already in SwipedFar State.
						//(changing a DP to the same value won't trigger the OnValueChanged)
						OnStateChanged(SwipeState.Value, useTransitions: true);
					}
					else
					{
						SwipeState = SwipingState.SwipedFar;
					}
				}
				else
				{
					ResetSwipeState();
				}
			}
		}

		private void OnOutsideTap(UITapGestureRecognizer gestureRecognizer)
		{
			// When gesture ends, we change the state of the control
			if (gestureRecognizer.State == UIGestureRecognizerState.Ended)
			{
				ResetSwipeState();
			}
		}

		private void OnOutsidePan(UIPanGestureRecognizer gestureRecognizer)
		{
			// When gesture ends, we change the state of the control
			if (gestureRecognizer.State == UIGestureRecognizerState.Began)
			{
				ResetSwipeState();
			}
		}

		private void ResetSwipeState()
		{
			if (SwipeState.HasValue)
			{
				if (SwipeState == SwipingState.NotSwiped)
				{
					// Force a OnStateChanged here if control is already in NotSwiped State.
					//(changing a DP to the same value won't trigger the OnValueChanged)
					OnStateChanged(SwipeState.Value, useTransitions: true);
				}
				else
				{
					// Reset to middle state;
					SwipeState = SwipingState.NotSwiped;
				}
			}
		}

		private void MoveMainContainer(SwipingState state, bool useTransitions)
		{
			Action animatedAction = null;

			switch (state)
			{
				case SwipingState.SwipedFar:
					animatedAction = () =>
					{
						_translation = _translationWhenSnappedFar;
						_mainContainer.Transform = CGAffineTransform.MakeTranslation((float)_translation, 0);
					};
					break;
				case SwipingState.SwipedNear:
					animatedAction = () =>
					{
						_translation = _translationWhenSnappedNear;
						_mainContainer.Transform = CGAffineTransform.MakeTranslation((float)_translation, 0);
					};
					break;
				case SwipingState.NotSwiped:
				default:
					animatedAction = () =>
					{
						_translation = _translationWhenNotSwiped;
						_mainContainer.Transform = CGAffineTransform.MakeTranslation((float)_translation, 0);
					};
					break;
			}

			if (useTransitions)
			{
				UIView.Animate(
					duration: _translationDuration,
					// Do it now
					delay: 0,
					options: UIViewAnimationOptions.CurveEaseIn,
					animation: animatedAction,
					// Force the position in case the animation did not execute properly. 
					// This happens when the animation is called while the control is offscreen
					completion: animatedAction
				);
			}
			else
			{
				animatedAction();
			}
		}
	}
}
#endif
