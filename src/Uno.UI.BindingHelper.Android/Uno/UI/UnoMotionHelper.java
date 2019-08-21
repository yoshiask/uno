package Uno.UI;

import android.graphics.Matrix;
import android.graphics.Rect;
import android.util.Log;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewParent;

/* internal */ abstract class DispatchMotionTarget {
	protected DispatchMotionTarget(Uno.UI.UnoViewParent view) {
	}

	abstract ViewGroup getView();

	// state local to the target but managed by the helper
	private boolean _isCustomDispatchIsActive;
	boolean getCustomDispatchIsActive() { return _isCustomDispatchIsActive; }
	void setCustomDispatchIsActive(boolean isCustomDispatchIsActive) { _isCustomDispatchIsActive = isCustomDispatchIsActive; }

	private View _customDispatchTarget;
	View getCustomDispatchTouchTarget() { return _customDispatchTarget; }
	void setCustomDispatchTouchTarget(View child) {
		if (getIsTargetCachingSupported()) {
			_customDispatchTarget = child;
		}
	}

	// access to common managed properties of the target
	abstract int getChildRenderTransformCount();
	abstract Matrix findRenderTransform(View child);
	abstract boolean getIsHitTestVisible();
	abstract boolean nativeHitCheck(); // TODO: This should be coerced into the IsHitTestVisible()
	abstract boolean getIsEnable();
	abstract boolean tryRaiseNativeMotionEvent(MotionEvent event, View originalSource);

	// specific to the raised event (generic vs touch)
	abstract boolean getIsTargetCachingSupported();
	abstract boolean dispatchToSuper(MotionEvent event);
	abstract boolean dispatchToChild(View child, MotionEvent event);
}

/* internal */ class UnoMotionHelper {
	private static final String LOGTAG = "UnoViewGroup";

	public static UnoMotionHelper Instance = new UnoMotionHelper();

	// To trace the 'dispatchTouchEvent', uncomment this and then uncomment logs in the method itself
	private static String _indent = "";
	//private boolean _isCurrentMotionBlockedByAnyChild, _currentMotionIsHandled, _isCurrentMotionDispatchedToAChildUnoViewGroup;

//	public final void setChildMotionEventResult(View child, boolean isBlocking, boolean isHandling)
//	{
//		_isCurrentMotionDispatchedToAChildUnoViewGroup = true;
//		_isCurrentMotionBlockedByAnyChild = isBlocking;
//		_currentMotionIsHandled = isHandling;
//	}

	private boolean _currentMotionIsHandled;
	private View _currentMotionOriginalSource;

	// To trace the pointer events (dispatchTouchEvent and dispatchGenericMotionEvent),
	// uncomment this and then uncomment logs in the method itself.
	public boolean dispatchMotionEvent(Uno.UI.DispatchMotionTarget target, MotionEvent event)
	{
		final ViewGroup view = target.getView();
		final String originalIndent = _indent;
		Log.i(LOGTAG, _indent + "    + " + view.toString() + "(" + System.identityHashCode(this) + ") " +
			"[size: " + view.getWidth() + "x" + view.getHeight() + " | scroll: x="+ view.getScrollX() + " y=" + view.getScrollY() + "]");
		_indent += "    | ";
		Log.i(LOGTAG, _indent + event.toString());

		final boolean dispatched = dispatchMotionEventCore(target, event);

		_indent = originalIndent;

		return dispatched;
	}

	private boolean dispatchMotionEventCore(Uno.UI.DispatchMotionTarget target, MotionEvent event)
	//private boolean dispatchMotionEvent(MotionEvent e, IDispatchStrategy dispatch)
	{
		// The purpose of dispatchTouchEvent is to find the target (view) of a touch event.
		// When the user touches the screen, dispatchTouchEvent is called on the top-most view, and recursively passed down to all children.
		// Once a view (that we will call target) decides to handle the touch event (generally when it has gesture detectors and none of its children handled the event), it returns true.
		// The parent then gets notified that the event was handled by one of its child, stops passing down the touch event to the siblings of the target,
		// ignores the event (i.e., doesn't handle it itself even if it could), and returns true to its own parent (which bubbles all the way to the top-most view).
		//
		// Essentially, returning true means that you 'handled' (and implicitly also 'blocked') a touch event, where:
		// - blocked -> prevent siblings from handling the event
		// - handled -> prevent parents from handling the event
		// Natively, you can't 'block' a touch event without also 'handling' it.
		//
		// In XAML, the distinction between 'blocking' and 'handling' is important, and a view must be able to 'block' a touch event without necessarily 'handling' it.
		// Therefore, the single boolean offered by dispatchTouchEvent isn't enough, and we must allow UnoViewGroups to communicate these 'blocked' vs 'handled' nuances through some other channel.
		// To do this, we introduce 3 boolean fields in UnoViewGroups: _childIsUnoViewGroup, _childBlockedTouchEvent, _childHandledTouchEvent (each with their own public setter).
		// Because UnoViewGroup must be compatible with native (non-UnoViewGroup) views, we need to process the input (super.dispatchTouchEvent) and output (return) differently based on the nature of the parent and children.
		//
		// Input:
		// - Child is UnoViewGroup (_childIsUnoViewGroup = true) ->
		//     - read the values of _childBlockedTouchEvent and _childHandledTouchEvent (set by the child in its own dispatchTouchEvent)
		// - Child is native (_childIsUnoViewGroup = false) ->
		//     - set both the values of _childBlockedTouchEvent and _childHandledTouchEvent to the value of super.dispatchTouchEvent
		//
		// Output:
		// - Parent is UnoViewGroup ->
		//     - set _childIsUnoViewGroup, _childBlockedTouchEvent and _childHandledTouchEvent on the parent
		//     - return true if isBlockingTouchEvent to prevent siblings from receiving the touch event
		//       (returned value won't actually be read by parent, as it will prefer _childBlockedTouchEvent and _childHandledTouchEvent instead)
		// - Parent is native ->
		//	   - return true if isHandlingTouchEvent
		//       (because native views can't read _childBlockedTouchEvent and _childHandledTouchEvent, and will assume true to mean the event was handled)

		final ViewGroup view = target.getView();

		// Reset possibly invalid states (set by children in previous calls)
		_currentMotionIsHandled = false;
		_currentMotionOriginalSource = null;

		final boolean isDown = event.getAction() == MotionEvent.ACTION_DOWN;
		if (isDown) {
			// When we receive a pointer DOWN, we have to reset the down -> move -> up sequence,
			// so the dispatch will re-evaluate the _customDispatchTouchTarget;
			// Note: we do not support the MotionEvent splitting for the custom touch target,
			//		 we expect that the children will properly split the events

			target.setCustomDispatchIsActive(target.getChildRenderTransformCount() > 0);
			target.setCustomDispatchTouchTarget(null);
		}

		if (!target.getIsHitTestVisible() || !target.getIsEnable()) {
			// The View is not TestVisible or disabled, there is nothing to do here!
			Log.i(LOGTAG, _indent + "BLOCKED [isHitTestVisible: " + target.getIsHitTestVisible() + " | isEnabled: " + target.getIsEnable() + "]");

			return false;
		}

		if (isDown && !isMotionInView(view, event)) {
			// When using the "super" dispatch path, it's possible that for visual constraints (i.e. clipping),
			// the view must not handle the touch. If that's the case, the touch event must be dispatched
			// to other controls.
			// Note: We do this check only when starting a new manipulation (isDown), so the whole down->move->up sequence is ignored;
			// Warning: We should also do this check for events that does not have strong sequence concept (e.g. Hover)
			// 			(!dispatch.getIsTargetCachingSupported() || isDown), BUT if we do this, we may miss some HOVER_EXIT
			//			So we prefer to not follow the UWP behavior (PointerEnter/Exit are raised only when entering/leaving
			//			non clipped content) and get all the events.
			Log.i(LOGTAG, _indent + "BLOCKED (not in view due to clipping)");

			return false;
		}

		// Note: Try to always dispatch the touch events, otherwise system controls may not behave
		//		 properly, such as not displaying "material design" animation cues
		//		 (e.g. the growing circles in buttons when keeping pressed (RippleEffect)).

		boolean childIsTouchTarget; // a.k.a. blocking
		if (target.getCustomDispatchIsActive()) {
			Log.i(LOGTAG, _indent + "CUSTOM dispatch (" + target.getChildRenderTransformCount() + " of " + view.getChildCount() + " children are transformed )");

			childIsTouchTarget = dispatchStaticTransformedMotionEvent(target, event);
		} else {
			Log.i(LOGTAG, _indent + "SUPER dispatch (none of the " + view.getChildCount() + " children is transformed)");

			// _shouldBlockRequestFocus: This is a hacky way to prevent requestFocus() being incorrectly called for views whose parent is
			// using the 'static transformation path.' The better fix would be for the static transformation code path to import more of
			// ViewGroup's logic to only propagate dispatchTouchEvent() to children selectively.
			//_shouldBlockRequestFocus = !isPointInView && e.getAction() == MotionEvent.ACTION_UP && hasTransformedSiblings();
			childIsTouchTarget = target.dispatchToSuper(event);
			//_shouldBlockRequestFocus = false;
		}

		// Note: There is a bug (#14712) where the UnoViewGroup receives the MOTION_DOWN,
		// is collapsed (or the child who received the MOTION_DOWN) (e.g. VisualState concurrency issue)
		// and doesn't receive the MOTION_UP. This is because the control is removed
		// from the visual tree when it's collapsed and won't get the dispatchTouchEvent.
		// To workaround this, simply put a transparent background on the clickable control
		// so that it receives the touch (tryHandleTouchEvent) instead of its children.

		final boolean isTouchTarget = childIsTouchTarget || target.nativeHitCheck();
		if (isTouchTarget && _currentMotionOriginalSource == null) {
			// If the ** static ** _currentMotionOriginalSource is null, it means we are the are the first managed child that
			// completes the dispatch, so we are the "OriginalSource" of the event (a.k.a. the leaf of the visual tree)

			Log.i(LOGTAG, _indent + "This control is the leaf, it's being set as the original source of the event.");

			_currentMotionOriginalSource = view;
		}

		if (!_currentMotionIsHandled && isTouchTarget) {
			// If the event was not "handled" (in the UWP terminology) by the managed, try to handle
			// it here for the current target. (i.e. we are bubbling the managed event here !)

			_currentMotionIsHandled = target.tryRaiseNativeMotionEvent(event, _currentMotionOriginalSource);

			Log.i(LOGTAG, _indent + "Managed event not handled yet, tried to raise it, result: " + _currentMotionIsHandled);
		}

		ViewParent parent = view.getParent();
		Uno.UI.UnoViewParent unoParent = null;
		while(parent != null)
		{
			if (parent instanceof Uno.UI.UnoViewParent) {
				unoParent = (Uno.UI.UnoViewParent)parent;
				break;
			}
			parent = parent.getParent();
		}

		if (unoParent == null) {
			// The top element of the visual tree must always reply 'true' in order to receive all pointers events.
			// (If we reply 'false' to an ACTION_DOWN, we won't receive the subsequent ACTION_MOVE nor ACTION_UP.)

			Log.i(LOGTAG, _indent + "ROOT true [isTarget: " + isTouchTarget + " | isHandled: " + _currentMotionIsHandled + "]");

			// When we reach to top of the visual tree, we clear the original source to prevent leak!
			_currentMotionOriginalSource = null;
			return true;
		}
		else if (isTouchTarget)
		{
			Log.i(LOGTAG, _indent + "TARGET true [isTarget: " + isTouchTarget + " | isHandled: " + _currentMotionIsHandled + "]");

			return true;
		}
		else
		{
			if (!_currentMotionIsHandled) {
				Log.e(LOGTAG, _indent + "WHAT THE HELL *********************************************");
			}

			Log.i(LOGTAG, _indent + "OUT false [isTarget: " + isTouchTarget + " | isHandled: " + _currentMotionIsHandled + "]");

			return false;
		}

//		else if (unoParent)
//		{
//			parentUnoViewGroup.setChildMotionEventResult(this, isBlockingTouchEvent, isHandlingTouchEvent);
//			// parentUnoViewGroup.setChildHandledTouchEvent(isHandlingTouchEvent);
//			// parentUnoViewGroup.setChildBlockedTouchEvent(isBlockingTouchEvent);
//
//			// Prevents siblings from receiving the touch event.
//			// Won't actually be read by parent (which will prefer _childBlockedTouchEvent and _childHandledTouchEvent).
//
//			Log.i(LOGTAG, _indent + "MANAGED result: " + isBlockingTouchEvent);
//
//			// As soon as we are the "touch target", we must return "true" in order to receive future events
//			// (Like move or up).
//			return isBlockingTouchEvent; // WE MUST RETURN TRUE TO NATIVE PARENT IN ANY CASES
//		}
//		else // parent is native
//		{
//			Log.i(LOGTAG, _indent + "NATIVE result: " + isHandlingTouchEvent);
//
//			// Native views don't understand the difference between 'blocked' and 'handled',
//			// and will assume true to mean that the touch event was handled (which can cause problems when nested inside native controls like ListViews).
//			return isBlockingTouchEvent; // WE MUST RETURN TRUE TO NATIVE PARENT IN ANY CASES
//		}
	}

	private boolean dispatchStaticTransformedMotionEvent(Uno.UI.DispatchMotionTarget target, MotionEvent event) {
		// As super ViewGroup won't apply the "StaticTransform" on the event (cf. https://android.googlesource.com/platform/frameworks/base/+/0e71b4f19ba602c8c646744e690ab01c69808b42/core/java/android/view/ViewGroup.java#2992)
		// when it determines if the `MotionEvent` is "in the view" of the child (https://android.googlesource.com/platform/frameworks/base/+/0e71b4f19ba602c8c646744e690ab01c69808b42/core/java/android/view/ViewGroup.java#2975)
		// the event will be filtered out and won't be propagated properly to all children (https://android.googlesource.com/platform/frameworks/base/+/0e71b4f19ba602c8c646744e690ab01c69808b42/core/java/android/view/ViewGroup.java#2665)
		// As a result a UIElement which has a `RenderTransform` won't be able to handle tap properly.
		// To workaround this, if we have some child transformation, we propagate the event by ourselves.
		// Doing this we bypass a lot of logic done by the super ViewGroup, (https://android.googlesource.com/platform/frameworks/base/+/0e71b4f19ba602c8c646744e690ab01c69808b42/core/java/android/view/ViewGroup.java#2557)
		// especially optimization of the TouchTarget resolving / tracking. (https://android.googlesource.com/platform/frameworks/base/+/0e71b4f19ba602c8c646744e690ab01c69808b42/core/java/android/view/ViewGroup.java#2654)
		// We assume that events that are wrongly dispatched to children are going to be filtered by children themselves
		// and this support is sufficient enough for our current cases.
		// Note: this is not fully compliant with the UWP contract (cf. https://github.com/nventive/Uno/issues/649)

		// Note: If this logic is called once, it has to be called for all MotionEvents in the same touch cycle, including Cancel, because if
		// ViewGroup.dispatchTouchEvent() isn't called for Down then all subsequent events won't be handled correctly
		// (because mFirstTouchTarget won't be set)

		final View touchTarget = target.getCustomDispatchTouchTarget();
		if (touchTarget != null) {
			// We already have a target for the events, just apply the static transform and dispatch the event.
			// Note: We "assumeInView" as all the pointers of the same sequence (down -> move -> up) should be raised
			//		 on the same element. This behavior a.k.a. "implicit capture" is not the UWP one, but the Android behavior.
			//		 It will be "patched" to follow the UWP behavior in the managed code (cf. UIElement.CapturePointer).
			return dispatchStaticTransformedMotionEvent(target, touchTarget, event, true);

		} else {
			final ViewGroup view = target.getView();

			// The target was not selected yet
			for (int i = view.getChildCount() - 1; i >= 0; i--) { // Inverse enumeration in order to prioritize controls that are on top
				View child = view.getChildAt(i);

				// Same check as native "canViewReceivePointerEvents"
				if (child.getVisibility() != View.VISIBLE || child.getAnimation() != null) {
					continue;
				}

				if (dispatchStaticTransformedMotionEvent(target, child, event, false)) {
					target.setCustomDispatchTouchTarget(child); // (try to) cache the child for future events
					return true; // Stop at the first child which is able to handle the event
				}
			}

			return false;
		}
	}

	private boolean dispatchStaticTransformedMotionEvent(Uno.UI.DispatchMotionTarget target, View child, MotionEvent event, boolean assumeInView){
		// For the ACTION_CANCEL the coordinates are not set properly:
		// "Canceling motions is a special case.  We don't need to perform any transformations
		// or filtering.  The important part is the action, not the contents."
		// https://android.googlesource.com/platform/frameworks/base/+/master/core/java/android/view/ViewGroup.java#3010
		if (event.getAction() == MotionEvent.ACTION_CANCEL) {
			return target.dispatchToChild(child, event);
		}

		final ViewGroup view = target.getView();
		final Matrix transform = target.findRenderTransform(child);
		final float offsetX = view.getScrollX() - child.getLeft();
		final float offsetY = view.getScrollY() - child.getTop();

		boolean handled = false;
		if (transform == null || transform.isIdentity()) {
			// No meaningful transformation on this child, instead of cloning the MotionEvent,
			// we only offset the current one, propagate it to the child and then offset it back to its original values.

			event.offsetLocation(offsetX, offsetY);

			// When we are searching for the target (i.e. !assumeInView),
			// we make sure that the view is under the touch event.
			if (assumeInView || isMotionInView(child, event)) {
				handled = target.dispatchToChild(child, event);
			}

			event.offsetLocation(-offsetX, -offsetY);
		} else {
			// We have a valid static transform on this child, we have to transform the MotionEvent
			// into the child coordinates.

			final Matrix inverse = new Matrix();
			transform.invert(inverse);

			final MotionEvent transformedEvent = MotionEvent.obtain(event);
			transformedEvent.offsetLocation(offsetX, offsetY);
			transformedEvent.transform(inverse);

			// When we are searching for the target (i.e. !assumeInView),
			// we make sure that the view is under the touch event.
			if (assumeInView || isMotionInView(child, transformedEvent)) {
				handled = target.dispatchToChild(child, transformedEvent);
			}

			transformedEvent.recycle();
		}

		return handled;
	}

	/**
	 * Checks if the given motion in the view's local coordinate space is within its bounds, taking any clipping into account.
	 * @param e The event to check
	 * @return True if the point is within the view's bounds, false otherwise.
	 */
	private static boolean isMotionInView(View view, MotionEvent e) {
		final float x = e.getX();
		final float y = e.getY();

		if (x < 0 || x >= view.getWidth()
			|| y < 0 || y >= view.getHeight()) {
			return false;
		}

		final Rect clipBounds = android.support.v4.view.ViewCompat.getClipBounds(view);
		if (clipBounds == null) {
			return true;
		} else{
			return x >= clipBounds.left && x < clipBounds.right && y >= clipBounds.top && y < clipBounds.bottom;
		}
	}
}
