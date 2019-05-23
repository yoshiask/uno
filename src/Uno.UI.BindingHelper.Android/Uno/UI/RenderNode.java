package android.view;

import android.graphics.*;
import android.graphics.drawable.*;
import android.view.*;
import android.view.animation.Transformation;
import android.util.Log;

import java.lang.*;
import java.lang.reflect.*;
import java.util.Map;
import java.util.HashMap;
import java.util.ArrayList;

public class RenderNode {

	private static RuntimeException NotSupported(String name) {
		Log.i("", name + "not supported on a dummy object");

        return new RuntimeException("Dummy object");
	}

	public void destroy() {
		throw NotSupported("destroy");
	}
    public static RenderNode create(String name, View owningView) {
		throw NotSupported("create");
	}
    public static RenderNode adopt(long nativePtr) {
		throw NotSupported("adopt");
	}
    public void requestPositionUpdates(SurfaceView view) {
		throw NotSupported("requestPositionUpdates");
	}
    public /*DisplayListCanvas*/Object start(int width, int height) {
		throw NotSupported("start");
	}
    public void end(/*DisplayListCanvas*/Object canvas) {
		throw NotSupported("end");
	}
    public void discardDisplayList() {
		throw NotSupported("discardDisplayList");
	}
    public boolean isValid() {
		throw NotSupported("isValid");
	}

    ///////////////////////////////////////////////////////////////////////////
    // Matrix manipulation
    ///////////////////////////////////////////////////////////////////////////
	public boolean hasIdentityMatrix() {
		throw NotSupported("hasIdentityMatrix");
	}

    public void getMatrix(Matrix outMatrix) {
		throw NotSupported("getMatrix");
	}

    public void getInverseMatrix(Matrix outMatrix) {
		throw NotSupported("getInverseMatrix");
	}

    ///////////////////////////////////////////////////////////////////////////
    // RenderProperty Setters
    ///////////////////////////////////////////////////////////////////////////
    public boolean setLayerType(int layerType) {
		throw NotSupported("setLayerType");
	}
    public boolean setLayerPaint(Paint paint) {
		throw NotSupported("setLayerPaint");
	}
    public boolean setClipBounds(Rect rect) {
		throw NotSupported("setClipBounds");
	}
    public boolean setClipToBounds(boolean clipToBounds) {
		throw NotSupported("setClipToBounds");
	}
    public boolean setProjectBackwards(boolean shouldProject) {
		throw NotSupported("setProjectBackwards");
	}
    public boolean setProjectionReceiver(boolean shouldRecieve) {
		throw NotSupported("setProjectionReceiver");
	}
    public boolean setOutline(Outline outline) {
		throw NotSupported("setOutline");
	}
    public boolean hasShadow() {
		throw NotSupported("hasShadow");
	}
    public boolean setSpotShadowColor(int color) {
		throw NotSupported("setSpotShadowColor");
	}
    public boolean setAmbientShadowColor(int color) {
		throw NotSupported("setAmbientShadowColor");
	}
    public int getSpotShadowColor() {
		throw NotSupported("getSpotShadowColor");
	}
    public int getAmbientShadowColor() {
		throw NotSupported("getAmbientShadowColor");
	}
    public boolean setClipToOutline(boolean clipToOutline) {
		throw NotSupported("setClipToOutline");
	}
    public boolean getClipToOutline() {
		throw NotSupported("getClipToOutline");
	}
    public boolean setRevealClip(boolean shouldClip, float x, float y, float radius) {
		throw NotSupported("setRevealClip");
	}
    public boolean setStaticMatrix(Matrix matrix) {
		throw NotSupported("setStaticMatrix");
	}
    public boolean setAnimationMatrix(Matrix matrix) {
		throw NotSupported("setAnimationMatrix");
	}
    public boolean setAlpha(float alpha) {
		throw NotSupported("setAlpha");
	}
    public float getAlpha() {
		throw NotSupported("getAlpha");
	}
    public boolean setHasOverlappingRendering(boolean hasOverlappingRendering) {
		throw NotSupported("setHasOverlappingRendering");
	}
    public boolean hasOverlappingRendering() {
		throw NotSupported("hasOverlappingRendering");
	}
    public boolean setElevation(float lift) {
		throw NotSupported("setElevation");
	}
    public float getElevation() {
		throw NotSupported("getElevation");
	}
    public boolean setTranslationX(float translationX) {
		throw NotSupported("setTranslationX");
	}
    public float getTranslationX() {
		throw NotSupported("getTranslationX");
	}
    public boolean setTranslationY(float translationY) {
		throw NotSupported("setTranslationY");
	}
    public float getTranslationY() {
		throw NotSupported("getTranslationY");
	}
    public boolean setTranslationZ(float translationZ) {
		throw NotSupported("setTranslationZ");
	}
    public float getTranslationZ() {
		throw NotSupported("getTranslationZ");
	}
    public boolean setRotation(float rotation) {
		throw NotSupported("setRotation");
	}
    public float getRotation() {
		throw NotSupported("getRotation");
	}
    public boolean setRotationX(float rotationX) {
		throw NotSupported("setRotationX");
	}
    public float getRotationX() {
		throw NotSupported("getRotationX");
	}
    public boolean setRotationY(float rotationY) {
		throw NotSupported("setRotationY");
	}
    public float getRotationY() {
		throw NotSupported("getRotationY");
	}
    public boolean setScaleX(float scaleX) {
		throw NotSupported("setScaleX");
	}
    public float getScaleX() {
		throw NotSupported("getScaleX");
	}
    public boolean setScaleY(float scaleY) {
		throw NotSupported("setScaleY");
	}
    public float getScaleY() {
		throw NotSupported("getScaleY");
	}
    public boolean setPivotX(float pivotX) {
		throw NotSupported("setPivotX");
	}
    public float getPivotX() {
		throw NotSupported("getPivotX");
	}
    public boolean setPivotY(float pivotY) {
		throw NotSupported("setPivotY");
	}
    public float getPivotY() {
		throw NotSupported("getPivotY");
	}
    public boolean isPivotExplicitlySet() {
		throw NotSupported("desisPivotExplicitlySettroy");
	}
    public boolean resetPivot() {
		throw NotSupported("resetPivot");
	}
    public boolean setCameraDistance(float distance) {
		throw NotSupported("setCameraDistance");
	}
    public float getCameraDistance() {
		throw NotSupported("getCameraDistance");
	}
    public boolean setLeft(int left) {
		throw NotSupported("setLeft");
	}
    public boolean setTop(int top) {
		throw NotSupported("setTop");
	}
    public boolean setRight(int right) {
		throw NotSupported("setRight");
	}
    public boolean setBottom(int bottom) {
		throw NotSupported("setBottom");
	}
    public boolean setLeftTopRightBottom(int left, int top, int right, int bottom) {
		throw NotSupported("setLeftTopRightBottom");
	}
    public boolean offsetLeftAndRight(int offset) {
		throw NotSupported("offsetLeftAndRight");
	}
    public boolean offsetTopAndBottom(int offset) {
		throw NotSupported("offsetTopAndBottom");
	}
    public void output() {
		throw NotSupported("output");
	}
    public int getDebugSize() {
		throw NotSupported("getDebugSize");
	}

    ///////////////////////////////////////////////////////////////////////////
    // Animations
    ///////////////////////////////////////////////////////////////////////////
    public void addAnimator(/*RenderNodeAnimator*/Object animator) {
		throw NotSupported("addAnimator");
	}
    public boolean isAttached() {
		throw NotSupported("isAttached");
	}
    public void registerVectorDrawableAnimator(/*AnimatedVectorDrawable.VectorDrawableAnimatorRT*/Object animatorSet) {
		throw NotSupported("registerVectorDrawableAnimator");
	}
    public void endAllAnimators() {
		throw NotSupported("endAllAnimators");
	}
}
