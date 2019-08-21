package Uno.UI;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.*;
import android.graphics.PointF;
import android.graphics.Rect;
import android.util.Log;
import android.support.v4.view.*;
import java.lang.*;
import java.lang.reflect.*;

public abstract class UnoRecyclerView
		extends RecyclerView
		implements Uno.UI.UnoViewParent {
//	private boolean _childHandledTouchEvent;
//	private boolean _childBlockedTouchEvent;
//	private boolean _childIsUnoViewGroup;

	protected UnoRecyclerView(Context context) {
		super(context);
	}

//	public final void setChildHandledTouchEvent(boolean childHandledTouchEvent)
//	{
//		_childHandledTouchEvent |= childHandledTouchEvent;
//	}
//
//	public final void setChildBlockedTouchEvent(boolean childBlockedTouchEvent)
//	{
//		_childBlockedTouchEvent |= childBlockedTouchEvent;
//	}
//
//	public final void setChildIsUnoViewGroup(boolean childIsUnoViewGroup)
//	{
//		_childIsUnoViewGroup |= childIsUnoViewGroup;
//	}

	private boolean _isCurrentMotionBlockedByAnyChild, _isCurrentMotionHandledByAnyChild, _isCurrentMotionDispatchedToAChildUnoViewGroup;

	public final void setChildMotionEventResult(View child, boolean isBlocking, boolean isHandling)
	{
		_isCurrentMotionDispatchedToAChildUnoViewGroup = true;
		_isCurrentMotionBlockedByAnyChild = isBlocking;
		_isCurrentMotionHandledByAnyChild = isHandling;
	}

	public boolean dispatchTouchEvent(MotionEvent e)
	{
		// See UnoViewGroup for exegesis of Uno.Android's touch handling logic.
		_isCurrentMotionDispatchedToAChildUnoViewGroup = false;
		_isCurrentMotionBlockedByAnyChild = false;
		_isCurrentMotionHandledByAnyChild = false;

		Log.i("ListView", "BTW I'm here MotionEvent: " + e.toString());

		// Always dispatch the touch events, otherwise system controls may not behave
		// properly, such as not displaying "material design" animation cues (e.g. the
		// growing circles in buttons when keeping pressed).
		boolean superDispatchTouchEvent = super.dispatchTouchEvent(e);

		Log.i("ListView", "BTW I'm here superDispatchTouchEvent: " + superDispatchTouchEvent);

//		if (!_isCurrentMotionDispatchedToAChildUnoViewGroup) // child is native
//		{
//			// Log.i(this.toString(), "!_childIsUnoViewGroup: " + !_childIsUnoViewGroup);
//			// If no child is under the touch, or the UnoRecyclerView is scrolling, superDispatchTouchEvent is normally true.
//			_isCurrentMotionBlockedByAnyChild = _isCurrentMotionHandledByAnyChild = superDispatchTouchEvent;
//		}

		boolean isBlockingTouchEvent = superDispatchTouchEvent;
		// Always return true if super returns true, otherwise scrolling might not be handled correctly.
		boolean isHandlingTouchEvent = _isCurrentMotionHandledByAnyChild; // || superDispatchTouchEvent;

		// Log.i(this.toString(), "MotionEvent: " + e.toString());
		// Log.i(this.toString(), "superDispatchTouchEvent: " + superDispatchTouchEvent);
		// Log.i(this.toString(), "_childBlockedTouchEvent: " + _childBlockedTouchEvent);
		// Log.i(this.toString(), "_childHandledTouchEvent: " + _childHandledTouchEvent);
		// Log.i(this.toString(), "isBlockingTouchEvent: " + isBlockingTouchEvent);
		// Log.i(this.toString(), "isHandlingTouchEvent: " + isHandlingTouchEvent);

//		Uno.UI.UnoViewParent parentUnoViewGroup = getParentUnoViewGroup();
//		boolean parentIsUnoViewGroup = parentUnoViewGroup != null;
//		// Log.i(this.toString(), "parentIsUnoViewGroup: " + parentIsUnoViewGroup);
//
//		if (parentIsUnoViewGroup)
//		{
//			parentUnoViewGroup.setChildMotionEventResult(this, isBlockingTouchEvent, isHandlingTouchEvent);
//
//			Log.i("ListView", "BTW I'm here MANAGED : " + isBlockingTouchEvent);
//
//			// Prevents siblings from receiving the touch event.
//			// Won't actually be read by parent (which will prefer _childBlockedTouchEvent and _childHandledTouchEvent).
//			return isBlockingTouchEvent;
//		}
//		else // parent is native
//		{
//			Log.i("ListView", "BTW I'm here NATIVE: " + isHandlingTouchEvent);
//
//			// Native views don't understand the difference between 'blocked' and 'handled',
//			// and will assume true to mean that the touch event was handled (which can cause problems when nested inside native controls like ListViews).
//			return isBlockingTouchEvent;
//		}

		ViewParent parent = getParent();
		Uno.UI.UnoViewParent unoParent = null;
		while(parent != null)
		{
			if (parent instanceof Uno.UI.UnoViewParent) {
				unoParent = (Uno.UI.UnoViewParent)parent;
				break;
			}
		}

		if (unoParent == null) {
			// The top element of the visual tree must always reply 'true' in order to receive all pointers events.
			// (If we reply 'false' to an ACTION_DOWN, we won't receive the subsequent ACTION_MOVE nor ACTION_UP.)

			Log.i("ListView", "ROOT result: true");

			return true;
		}
		else if (isBlockingTouchEvent)
		{
			if (isHandlingTouchEvent) {
				unoParent.setChildMotionEventResult(this, true, isHandlingTouchEvent);
			}

			Log.i("ListView", "ROOT result: true");

			return true;
		}
		else
		{
			if (!isHandlingTouchEvent) {
				Log.e("ListView", "WHAT THE HELL *********************************************");
			}

			return false;
		}
	}

	@Override
	public void removeViewAt(int index) {
		View child = getChildAt(index);
		super.removeViewAt(index);
		notifyChildRemoved(child);
	}

	@Override
	public void removeView(View view) {
		super.removeView(view);
		notifyChildRemoved(view);
	}

	private void notifyChildRemoved(View child)
	{
		Uno.UI.UnoViewGroup childViewGroup = child instanceof Uno.UI.UnoViewGroup
			? (Uno.UI.UnoViewGroup)child
			: null;

		if(childViewGroup != null)
		{
			// This is required because the Parent property is set to null
			// after the onDetachedFromWindow is called.
			childViewGroup.onRemovedFromParent();
		}
	}

	private Uno.UI.UnoViewParent getParentUnoViewGroup()
	{
		ViewParent parent = getParent();

		return parent instanceof Uno.UI.UnoViewParent
				? (Uno.UI.UnoViewParent)parent
				: null;
	}
}
