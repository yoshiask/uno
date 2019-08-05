#if __IOS__
//#define SWIPABLEITEM_LOGGING
using Uno.Extensions;
using Uno.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace Windows.UI.Xaml.Controls
{
	internal class SwipableItemOutsideGestureDelegate : UIGestureRecognizerDelegate
	{
		SwipableItem _swipableItem;

		public SwipableItemOutsideGestureDelegate(SwipableItem swipableItem)
		{
			_swipableItem = swipableItem;
		}

		/// <summary>
		/// Determines if the recognizer should accept or not the touch.
		/// </summary>
		public override bool ShouldReceiveTouch(UIGestureRecognizer recognizer, UITouch touch)
		{
			// We always want to receive touches
			return true;
		}

		public override bool ShouldBegin(UIGestureRecognizer recognizer)
		{
			UIPanGestureRecognizer panRecognizer = recognizer as UIPanGestureRecognizer;
			if (panRecognizer != null)
			{
				// Accept the pan gesture only if X is greater than Y
				var translation = panRecognizer.TranslationInView(_swipableItem);
				if (Math.Abs(translation.X) <= Math.Abs(translation.Y))
				{
					// if pan is not horizontal, ignore it.
					return false;
				}
			}

			// Ignore gestures that occur within the SwipableItem
			var tapLocation = recognizer.LocationInView(_swipableItem);
			var bounds = _swipableItem.Bounds;

			return !bounds.Contains(tapLocation);
		}

		/// <summary>
		/// Here we decide if multiple gestures can be recognized simultaneously
		/// </summary>
		public override bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
		{
			// if either of the gesture recognizers is a long press, don't recognize longpress simultaneously
			if (gestureRecognizer is UILongPressGestureRecognizer ||
				otherGestureRecognizer is UILongPressGestureRecognizer)
			{
				return false;
			}

			return true;
		}
	}
}
#endif
