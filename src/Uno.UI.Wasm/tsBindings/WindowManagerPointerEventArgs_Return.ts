/* TSBindingsGenerator Generated code -- this code is regenerated on each build */
class WindowManagerPointerEventArgs_Return
{
	/* Pack=8 */
	Timestamp : number;
	Event : number;
	SourceHandle : number;
	OriginalSourceHandle : number;
	PointerId : number;
	PointerType : number;
	RawX : number;
	RawY : number;
	IsCtrlPressed : boolean;
	IsShiftPressed : boolean;
	PressedButton : number;
	IsOver_HasValue : boolean;
	IsOver : boolean;
	public marshal(pData:number)
	{
		Module.setValue(pData + 0, this.Timestamp, "double");
		Module.setValue(pData + 8, this.Event, "i32");
		Module.setValue(pData + 16, this.SourceHandle, "i32");
		Module.setValue(pData + 24, this.OriginalSourceHandle, "i32");
		Module.setValue(pData + 32, this.PointerId, "i32");
		Module.setValue(pData + 40, this.PointerType, "i32");
		Module.setValue(pData + 48, this.RawX, "double");
		Module.setValue(pData + 56, this.RawY, "double");
		Module.setValue(pData + 64, this.IsCtrlPressed, "i32");
		Module.setValue(pData + 72, this.IsShiftPressed, "i32");
		Module.setValue(pData + 80, this.PressedButton, "i32");
		Module.setValue(pData + 88, this.IsOver_HasValue, "i32");
		Module.setValue(pData + 96, this.IsOver, "i32");
	}
}
