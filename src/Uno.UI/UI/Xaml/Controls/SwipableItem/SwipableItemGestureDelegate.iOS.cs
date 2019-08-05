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
    internal class SwipableItemGestureDelegate: UIGestureRecognizerDelegate
	{
		/// See the apple documentation 
		/// https://developer.apple.com/library/ios/documentation/EventHandling/Conceptual/EventHandlingiPhoneOS/GestureRecognizer_basics/GestureRecognizer_basics.html#//apple_ref/doc/uid/TP40009541-CH2-SW2
		/// for more details


		/// <summary>
		/// Determines if the recognizer should accept or not the touch.
		/// </summary>
		public override bool ShouldReceiveTouch(UIGestureRecognizer recognizer, UITouch touch)
		{
			// We always want to receive touches to possibly handle pan gestures.
			// If we wanted to implement a way to disable the swiping functionality, we could return false here.
			return true;
		}

		public override bool ShouldBegin(UIGestureRecognizer recognizer)
		{
			UIPanGestureRecognizer panRecognizer = recognizer as UIPanGestureRecognizer;
			if (panRecognizer != null)
			{
				// Accept the pan gesture only if X is greater than Y
				var translation = panRecognizer.TranslationInView(panRecognizer.View);
				return Math.Abs(translation.X) > Math.Abs(translation.Y);
			}
			else
			{
				// if the recognizer is something else than a pan, just accept it. (for example tap or long press)
				return true;
			}
		}

		/// <summary>
		/// Here we decide if multiple gestures can be recognized simultaneously
		/// </summary>
		public override bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
		{
			// if either of the gesture recognizers is a long press, don't recognize longpress and pan simultaneously
			if (gestureRecognizer is UILongPressGestureRecognizer || otherGestureRecognizer is UILongPressGestureRecognizer)
			{
				return false;
			}

			// if both gestures are pan gestures we will assume the other recognizer comes from a parent scrollviewer (i.e. listview)
			// Remark: Since we assume the parent scrollviewer is scrolling vertically, we only support this scenario.
			var panGestureRecognizer = gestureRecognizer as UIPanGestureRecognizer;
			var parentScrollViewGestureRecognizer = otherGestureRecognizer as UIPanGestureRecognizer;
			if (panGestureRecognizer != null && parentScrollViewGestureRecognizer != null)
			{
				var translation = panGestureRecognizer.TranslationInView(panGestureRecognizer.View);
				var velocity = panGestureRecognizer.VelocityInView(panGestureRecognizer.View);
#if SWIPABLEITEM_LOGGING
				this.Log().InfoIfEnabled(() => 
					string.Format("ShouldRecognize? Translation = {0},{1}, velocity {2},{3}",
						translation.X,
						translation.Y,
						velocity.X,
						velocity.Y
					)
				);
#endif
				return Math.Abs(translation.X) > Math.Abs(translation.Y) // Pan should be horizontal
					&& Math.Abs(velocity.Y) <= 0.25f; // Velocity should not be too high vertically, this is usually the case if someone is swiping very fast through the list
			}

			return true;
		}
	}
}
#endif
