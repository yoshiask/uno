/* TSBindingsGenerator Generated code -- this code is regenerated on each build */
class WindowManagerEnablePointerEventsParams
{
	/* Pack=4 */
	HtmlId : number;
	IsEnabled : boolean;
	public static unmarshal(pData:number) : WindowManagerEnablePointerEventsParams
	{
		let ret = new WindowManagerEnablePointerEventsParams();
		
		{
			ret.HtmlId = Number(Module.getValue(pData + 0, "*"));
		}
		
		{
			ret.IsEnabled = Boolean(Module.getValue(pData + 4, "i32"));
		}
		return ret;
	}
}
