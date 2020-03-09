using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;

namespace Windows.UI.Input
{
	internal class NullGestureRecognizer : IGestureRecognizer
	{
		public static IGestureRecognizer Instance { get; } = new NullGestureRecognizer();
		private NullGestureRecognizer()
		{
		}

		public event TypedEventHandler<GestureRecognizer, ManipulationStartingEventArgs> ManipulationStarting { add { } remove { } }
		public event TypedEventHandler<GestureRecognizer, ManipulationCompletedEventArgs> ManipulationCompleted { add { } remove { } }
		public event TypedEventHandler<GestureRecognizer, ManipulationInertiaStartingEventArgs> ManipulationInertiaStarting { add { } remove { } }
		public event TypedEventHandler<GestureRecognizer, ManipulationStartedEventArgs> ManipulationStarted { add { } remove { } }
		public event TypedEventHandler<GestureRecognizer, ManipulationUpdatedEventArgs> ManipulationUpdated { add { } remove { } }
		public event TypedEventHandler<GestureRecognizer, TappedEventArgs> Tapped { add { } remove { } }
		public event TypedEventHandler<GestureRecognizer, RightTappedEventArgs> RightTapped { add { } remove { } }
		public event TypedEventHandler<GestureRecognizer, HoldingEventArgs> Holding { add { } remove { } }
		public event TypedEventHandler<GestureRecognizer, CrossSlidingEventArgs> CrossSliding { add { } remove { } }
		public event TypedEventHandler<GestureRecognizer, DraggingEventArgs> Dragging { add { } remove { } }

		public GestureSettings GestureSettings { get => default; set {} }
		public bool IsActive => default;
		public object Owner => default;
		public GestureRecognizer.Manipulation PendingManipulation => default;
		public float InertiaRotationDeceleration{ get => default; set {} }
		public float InertiaTranslationDeceleration{ get => default; set {} }
		public float InertiaExpansionDeceleration{ get => default; set {} }
		public float InertiaExpansion{ get => default; set {} }
		public bool AutoProcessInertia{ get => default; set {} }
		public CrossSlideThresholds CrossSlideThresholds{ get => default; set {} }
		public bool CrossSlideExact{ get => default; set {} }
		public float InertiaRotationAngle{ get => default; set {} }
		public bool ShowGestureFeedback{ get => default; set {} }
		public float PivotRadius{ get => default; set {} }
		public bool CrossSlideHorizontally{ get => default; set {} }
		public Point PivotCenter{ get => default; set {} }
		public bool ManipulationExact{ get => default; set {} }
		public float InertiaTranslationDisplacement{ get => default; set {} }
		public bool IsInertial => default;
		public MouseWheelParameters MouseWheelParameters => default;

		public void ProcessDownEvent(PointerPoint value) { }
		public void ProcessMoveEvents(IList<PointerPoint> value) { }
		public void ProcessMoveEvents(IList<PointerPoint> value, bool isRelevant) { }
		public void ProcessUpEvent(PointerPoint value) { }
		public void ProcessUpEvent(PointerPoint value, bool isRelevant) { }
		public void CompleteGesture() { }
		public void PreventHolding(uint pointerId) { }
		public bool CanBeDoubleTap(PointerPoint value) => default;
		public void ProcessMouseWheelEvent(PointerPoint value, bool isShiftKeyDown, bool isControlKeyDown) { }
		public void ProcessInertia() { }
	}
}
