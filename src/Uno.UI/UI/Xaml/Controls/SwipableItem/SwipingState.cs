using System;
using System.Collections.Generic;
using System.Text;

namespace Windows.UI.Xaml.Controls
{
	public enum SwipingState
	{
		NotSwiped,
		SwipingNear,
		SwipedNear,
		LongSwipingNear,
		LongSwipedNear,
		SwipingFar,
		SwipedFar,
		LongSwipingFar,
		LongSwipedFar
	}
}
