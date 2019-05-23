package Uno.UI;

import android.graphics.Matrix;
import android.view.*;
import android.view.animation.Transformation;
import android.graphics.Rect;
import android.graphics.PointF;
import android.util.Log;

import java.lang.*;
import java.lang.reflect.*;
import java.util.Map;
import java.util.HashMap;
import java.util.ArrayList;

public class UnoRenderNode
	extends android.view.RenderNode {

	//private final Matrix _renderTransform = new Matrix();
	private final Object _inner;
 
	public UnoRenderNode(Object inner) {
		_inner = inner;
	}

	@Override
	public boolean hasIdentityMatrix() {
        return super.hasIdentityMatrix();
    }

	@Override
    public void getMatrix(Matrix outMatrix) {
        super.getMatrix(outMatrix);
    }

	@Override
    public void getInverseMatrix(Matrix outMatrix) {
        super.getInverseMatrix(outMatrix);
    }

	/*
	public Matrix getRenderTransform() {
		return _renderTransform;
	}
 
	@Override
	public Object invoke(Object proxy, Method method, Object[] args) throws Throwable {
		Log.i("", method.getName());

		switch(method.getName()) {
			case "hasIdentityMatrix":
				return hasIdentityMatrix(method.invoke(_inner, args));

			case "getMatrix":
				return getMatrix(method.invoke(_inner, args));

			case "getInverseMatrix":
				return getInverseMatrix(method.invoke(_inner, args));

			default:
				return method.invoke(_inner, args);
		}
	}

	private Object hasIdentityMatrix(Object innerValue) {
		return innerValue;
	}

	private Object getMatrix(Object innerValue) {
		return innerValue;
	}

	private Object getInverseMatrix(Object innerValue) {
		return innerValue;
	}
	*/

	/*
	public boolean hasIdentityMatrix() {
		return nHasIdentityMatrix(mNativeRenderNode);
	}
	public void getMatrix(@NonNull Matrix outMatrix) {
		nGetTransformMatrix(mNativeRenderNode, outMatrix.native_instance);
	}
	public void getInverseMatrix(@NonNull Matrix outMatrix) {
		nGetInverseTransformMatrix(mNativeRenderNode, outMatrix.native_instance);
	}
	*/
}
