using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Uno.Extensions;
using Uno.Logging;

namespace Uno.Foundation.Interop
{
	[Obfuscation(Feature = "renaming", Exclude = true)]
	[Preserve]
	public sealed class JSObject
	{
		private static readonly Func<string, IntPtr> _strToIntPtr =
			Marshal.SizeOf<IntPtr>() == 4
				? (s => (IntPtr)int.Parse(s))
				: (Func<string, IntPtr>)(s => (IntPtr)long.Parse(s));

		/// <summary>
		/// Used by javascript to dispatch a method call to the managed object at <paramref name="handlePtr"/>.
		/// </summary>
		[Obfuscation(Feature = "renaming", Exclude = true)]
		[Preserve]
		public static string Dispatch(string handlePtr, string method, string parameters)
		{
			var intPtr = _strToIntPtr(handlePtr);
			var handle = GCHandle.FromIntPtr(intPtr);

			if (!handle.IsAllocated)
			{
				handle.Log().Debug($"Cannot invoke '{method}' as target has been collected!");
				return @"{""isSuccess"":false}";
			}

			if (!(handle.Target is JSObjectHandle jsObjectHandle))
			{
				handle.Log().Debug($"Cannot invoke '{method}' as target is not a valid JSObjectHandle! ({handle.Target?.GetType()})");
				return @"{""isSuccess"":false}";
			}

			if (!jsObjectHandle.TryGetManaged(out var target))
			{
				jsObjectHandle.Log().Debug($"Cannot invoke '{method}' as target has been collected!");
				return @"{""isSuccess"":false}";
			}

			var result = jsObjectHandle.Metadata.InvokeManaged(target, method, parameters);

			switch (result)
			{
				case string str:
					return $@"{{""isSuccess"":true, ""value"": ""{str.Replace("\\", "\\\\").Replace("\"", @"\""")}""}}";

				case bool b:
					return $@"{{""isSuccess"":true, ""value"": ""{(b ? "true" : "false")}""}}";

				default:
					return @"{""isSuccess"":true}";
			}
		}
	}
}
