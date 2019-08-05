#if __ANDROID__
using Android.Gestures;
using System;
using System.Collections.Generic;
using System.Text;
using Android.Views;
using Android.Graphics;

using Uno.Extensions;

namespace Windows.UI.Xaml.Controls
{
	public class PanGestureListener : GestureDetector.SimpleOnGestureListener
	{
		Action<double, double> _onPan;

		public PanGestureListener(Action<double, double> onPan)
		{
			_onPan = onPan;
		}

		public override bool OnDown(MotionEvent e)
		{
			return true;
		}

		public override bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
		{
			if (e2.Action == MotionEventActions.Move)
			{
				_onPan(e2.GetX(), e2.GetY());
				return true;
			}

			return false;
		}
	}
}
#endif
