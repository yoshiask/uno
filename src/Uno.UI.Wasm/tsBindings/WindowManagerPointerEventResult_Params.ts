/* TSBindingsGenerator Generated code -- this code is regenerated on each build */
class WindowManagerPointerEventResult_Params
{
	/* Pack=4 */
	Handled : boolean;
	public static unmarshal(pData:number) : WindowManagerPointerEventResult_Params
	{
		let ret = new WindowManagerPointerEventResult_Params();
		
		{
			ret.Handled = Boolean(Module.getValue(pData + 0, "i32"));
		}
		return ret;
	}
}
