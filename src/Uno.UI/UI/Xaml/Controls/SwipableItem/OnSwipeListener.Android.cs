#if __ANDROID__
using Android.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Uno.UI;

namespace Windows.UI.Xaml.Controls
{
	public class OnSwipeListener : Java.Lang.Object, Android.Views.View.IOnTouchListener
	{
		public GestureDetector GestureDetector { get; private set; }

		private Android.Views.View _attachedView;

		public OnSwipeListener(Android.Views.View attachedView, Action<double, double> onPan)
		{
			_attachedView = attachedView;

			var listener = new PanGestureListener(onPan);
			GestureDetector = new GestureDetector(ContextHelper.Current, listener);
		}

		public bool OnTouch(Android.Views.View v, MotionEvent e)
		{
			return GestureDetector.OnTouchEvent(e);
		}
	}
}
#endif
