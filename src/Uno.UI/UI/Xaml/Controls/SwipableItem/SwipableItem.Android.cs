#if __ANDROID__
using Android.Graphics;
using Android.Views;
using Android.Views.Animations;
using Uno.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uno.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Numerics;

namespace Windows.UI.Xaml.Controls
{
	public partial class SwipableItem : ContentControl, View.IOnTouchListener
	{
		private const long _translationDuration = 100; // In miliseconds

		private object _currentDataContext;
		private bool _hasAppliedTemplate = false;
		private bool _hasArrangedOnce = false;
		private bool _hasInitializedSnapPoints = false;

		private ContentPresenter _mainContainer;

		public GestureDetector GestureDetector { get; private set; }

		private double _translationWhenNotSwiped;
		private double _translationWhenSnappedFar;
		private double _translationWhenSnappedNear;

		private GestureStart _gestureStart;

		private bool _isSwiping = false;

		// Distance in pixels a touch can wander before we think the user is scrolling
		// https://developer.android.com/reference/android/view/ViewConfiguration.html#getScaledTouchSlop()
		private static readonly float _touchSlop = ViewConfiguration.Get(ContextHelper.Current).ScaledTouchSlop;

		private static readonly IInterpolator _interpolator = new AccelerateInterpolator(); // This is a EaseIn interpolator

		public SwipableItem()
		{
#if !__ANDROID__ && !__IOS__
			this.DefaultStyleKey = typeof(SwipableItem);
#endif

			var listener = new PanGestureListener(Pan);
			GestureDetector = new GestureDetector(ContextHelper.Current, listener);

			this.SetOnTouchListener(this);

			IsTabStop = true; // We need to be able to take focus in order to lose it and reset ourselves.
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_mainContainer = this.GetTemplateChild("MainPresenter") as ContentPresenter;
			_mainContainer.Validation().NotNull("MainPresenter is a required Template Part in SwipableItem");

			_hasAppliedTemplate = true;
		}

		protected override void OnAfterArrange()
		{
			base.OnAfterArrange();

			_hasArrangedOnce = true;
			InitializeSnapPoints();
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			if (IsAutoReset)
			{
				ResetSwipeState();
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
				_translationWhenSnappedFar = -(this.FarSnapWidth * ViewHelper.Scale);
				_translationWhenSnappedNear = this.NearSnapWidth * ViewHelper.Scale;

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
		}

		private void Pan(double translationX, double translationY)
		{
			double translation = translationX - _gestureStart.TouchPosition.X + _gestureStart.ContainerPosition.X;

			float xClamped = (float)translation.Clamp(_translationWhenSnappedFar, _translationWhenSnappedNear);

			_mainContainer.SetX(xClamped);
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

		private void OnSwipeEnded()
		{
			var x = _mainContainer.GetX();

			// Check if centered first to avoid changing state to Far or Near
			// if there are no near or farsnap points.
			if (x == _translationWhenNotSwiped)
			{
				ResetSwipeState();
			}
			else if (x >= (_translationWhenSnappedNear * SwipeDecisionPoint))
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
			else if (x <= (_translationWhenSnappedFar * SwipeDecisionPoint))
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

		private void MoveMainContainer(SwipingState state, bool useTransitions)
		{
			double translation;

			switch (state)
			{
				case SwipingState.SwipedFar:
					translation = _translationWhenSnappedFar;
					break;
				case SwipingState.SwipedNear:
					translation = _translationWhenSnappedNear;
					break;
				case SwipingState.NotSwiped:
				default:
					translation = _translationWhenNotSwiped;
					break;
			}

			if (useTransitions)
			{
				// Do not set an interpolator. It can cause weird behaviors with parent scrollviewers.
				_mainContainer.Animate()
					.X((float)translation)
					.SetDuration(_translationDuration)
					.Start();
			}
			else
			{
				_mainContainer.SetX((float)translation);
			}

		}

		#region UNO TOUCH OVERRIDES

		protected override void OnPointerPressed(PointerRoutedEventArgs args)
		{
			// We handle PointerPressed event to make sure we continue to receive the Move events.
			// In the android touch system, if you don't handle an event you will not receive the rest of the gesture.
			args.Handled = true;
		}

		#endregion

		#region ANDROID TOUCH OVERRIDES

		public bool OnTouch(Android.Views.View view, MotionEvent e)
		{
			// CANCEL will come if our gesture is taken over by another control. i.e. if a parent scrollviewer detects a
			// vertical scroll.
			// UP will come when the last finger comes up
			// When we receive either touch action, it means our gesture has ended
			if (e.Action == MotionEventActions.Cancel || e.Action == MotionEventActions.Up)
			{
				if (_isSwiping)
				{
					OnSwipeEnded();
				}

				_isSwiping = false;
			}

			return GestureDetector.OnTouchEvent(e);
		}

		public override bool OnInterceptTouchEvent(MotionEvent e)
		{
			switch (e.Action)
			{
				case MotionEventActions.Down:
					_gestureStart = new GestureStart(new PointF(e.GetX(), e.GetY()), new PointF(_mainContainer.GetX(), _mainContainer.GetY()));
					return false;
				case MotionEventActions.Move:
					return ShouldInterceptMove(e);
				default:
					_isSwiping = false;
					return base.OnInterceptTouchEvent(e); ;
			}
		}

		private bool ShouldInterceptMove(MotionEvent e)
		{
			if (_isSwiping)
			{
				return true;
			}
			else
			{
				Vector2 cumulativeDistance = new Vector2(
												Math.Abs(e.GetX() - _gestureStart.TouchPosition.X),
												Math.Abs(e.GetY() - _gestureStart.TouchPosition.Y)
											);

				if (cumulativeDistance.X > _touchSlop && // touchSlop is the minimal distance for us to determine it is a scroll
					cumulativeDistance.X > cumulativeDistance.Y) // Check the scroll is horizontal
				{
					_isSwiping = true;

					//Take focus when we are swiped
					Focus(FocusState.Pointer);

					// Once we have intercepted the gesture, we prevent all parent ViewGroups from intercepting our gesture.
					// For example, this prevents a parent listview from intercepting our gesture if the user starts scrolling
					// vertically after starting horizontally.
					this.GetParents()
						.OfType<ViewGroup>()
						.ForEach(view => view.RequestDisallowInterceptTouchEvent(disallowIntercept: true));

					return true;
				}
				else
				{
					return false;
				}

			}
		}

		#endregion

		private class GestureStart
		{
			public GestureStart(PointF touchPosition, PointF containerPosition)
			{
				TouchPosition = touchPosition;
				ContainerPosition = containerPosition;
			}

			public PointF TouchPosition { get; private set; }

			public PointF ContainerPosition { get; private set; }

		}
	}
}
#endif
