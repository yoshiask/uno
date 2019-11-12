using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Uno.Disposables;
using Uno.Extensions;
using Uno.Foundation;

namespace Uno.Foundation.Interop
{
	public static class TSInteropMarshaller 
	{
		private static readonly Lazy<ILogger> _logger = new Lazy<ILogger>(() => typeof(TSInteropMarshaller).Log());

		public const UnmanagedType LPUTF8Str = (UnmanagedType)48;

		/// <summary>
		/// Prints the actual offsets of the structures present in <see cref="WindowManagerInterop"/> for debugging purposes.
		/// </summary>
		internal static void GenerateTSMarshallingLayouts()
		{
			// Uncomment this to troubshoot this field offsets.
			//
			// Console.WriteLine("Generating layouts");
			// foreach (var p in typeof(WindowManagerInterop).GetNestedTypes(System.Reflection.BindingFlags.NonPublic).Where(t => t.IsValueType))
			// {
			// 		var sb = new StringBuilder();
			   
			// 		Console.WriteLine($"class {p.Name}:");
			   
			// 		foreach (var field in p.GetFields())
			// 		{
			// 			var fieldOffset = Marshal.OffsetOf(p, field.Name);
			// 			Console.WriteLine($"\t{field.Name} : {fieldOffset}");
			// 		}
			// }
		}

		public static void InvokeJS<TParam>(
			string methodName,
			TParam paramStruct,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = null
		)
		{
			if (_logger.Value.IsEnabled(LogLevel.Debug))
			{
				_logger.Value.LogDebug($"InvokeJS for {memberName}/{typeof(TParam)}");
			}
			 
			var pParms = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TParam)));

			try
			{
				Marshal.StructureToPtr(paramStruct, pParms, false);

				var ret = WebAssemblyRuntime.InvokeJSUnmarshalled(methodName, pParms);
			}
			catch(Exception e)
			{
				if (_logger.Value.IsEnabled(LogLevel.Error))
				{
					_logger.Value.LogError($"Failed InvokeJS for {memberName}/{typeof(TParam)}: {e}"); 
				}
				throw;
			}
			finally
			{
				Marshal.DestroyStructure(pParms, typeof(TParam));
				Marshal.FreeHGlobal(pParms);
			}
		}

		public static TRet InvokeJS<TParam, TRet>(
			string methodName,
			TParam paramStruct,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = null
		)
		{
			if (_logger.Value.IsEnabled(LogLevel.Debug))
			{
				_logger.Value.LogDebug($"InvokeJS for {memberName}/{typeof(TParam)}/{typeof(TRet)}");
			}

			var pParms = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TParam)));
			var pReturnValue = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TRet)));

			TRet returnValue = default;

			try
			{
				Marshal.StructureToPtr(paramStruct, pParms, false);
				Marshal.StructureToPtr(returnValue, pReturnValue, false);

				var ret = WebAssemblyRuntime.InvokeJSUnmarshalled(methodName, pParms, pReturnValue);

				returnValue = (TRet)Marshal.PtrToStructure(pReturnValue, typeof(TRet));
				return returnValue;
			}
			catch (Exception e)
			{
				if (_logger.Value.IsEnabled(LogLevel.Error))
				{
					_logger.Value.LogDebug($"Failed InvokeJS for {memberName}/{typeof(TParam)}: {e}");
				}
				throw;
			}
			finally
			{
				Marshal.DestroyStructure(pParms, typeof(TParam));
				Marshal.FreeHGlobal(pParms);

				Marshal.DestroyStructure(pReturnValue, typeof(TRet));
				Marshal.FreeHGlobal(pReturnValue);
			}
		}

		//public static void InvokeJs<TRegisterParam, TCallbackArgs>(
		//	string registerMethodName,
		//	TRegisterParam registerMethodParamStruct,
		//	Property<TCallbackArgs> callbackArgs,
		//	[System.Runtime.CompilerServices.CallerMemberName]
		//	string memberName = null)
		//{
		//	if (_logger.Value.IsEnabled(LogLevel.Debug))
		//	{
		//		_logger.Value.LogDebug($"RegisterJSCallback for {memberName}/{typeof(TRegisterParam)}");
		//	}

		//	var pParms = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TRegisterParam)));

		//	try
		//	{
		//		Marshal.StructureToPtr(registerMethodParamStruct, pParms, false);

		//		var ret = WebAssemblyRuntime.InvokeJSUnmarshalled(registerMethodName, pParms, callbackArgs.Handle);
		//	}
		//	catch (Exception e)
		//	{
		//		if (_logger.Value.IsEnabled(LogLevel.Error))
		//		{
		//			_logger.Value.LogError($"Failed RegisterJSCallback for {memberName}/{typeof(TRegisterParam)}: {e}");
		//		}
		//		throw;
		//	}
		//	finally
		//	{
		//		Marshal.DestroyStructure(pParms, typeof(TRegisterParam));
		//		Marshal.FreeHGlobal(pParms);
		//	}
		//}

		public static Property<T> AllocJSProperty<T>(
			string setPropertyMethod,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = null)
		{
			if (_logger.Value.IsEnabled(LogLevel.Debug))
			{
				_logger.Value.LogDebug($"AllocJSProperty for {memberName}/{typeof(T)}");
			}

			var prop = new Property<T>();

			try
			{
				var ret = WebAssemblyRuntime.InvokeJSUnmarshalled(setPropertyMethod, prop.Handle);
			}
			catch (Exception e)
			{
				if (_logger.Value.IsEnabled(LogLevel.Error))
				{
					_logger.Value.LogError($"Failed AllocJSProperty for {memberName}/{typeof(T)}: {e}");
				}
				prop.Dispose();
				throw;
			}

			return prop;
		}

		public static (Property<TProp1>, Property<TProp2>) AllocJSProperties<TProp1, TProp2>(
			string setPropertyMethod,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = null)
		{
			if (_logger.Value.IsEnabled(LogLevel.Debug))
			{
				_logger.Value.LogDebug($"AllocJSProperties for {memberName}/{typeof(TProp1)}");
			}

			var prop1 = new Property<TProp1>();
			var prop2 = new Property<TProp2>();

			try
			{
				var ret = WebAssemblyRuntime.InvokeJSUnmarshalled(setPropertyMethod, prop1.Handle, prop2.Handle);
			}
			catch (Exception e)
			{
				if (_logger.Value.IsEnabled(LogLevel.Error))
				{
					_logger.Value.LogError($"Failed AllocJSProperties for {memberName}/{typeof(TProp1)}: {e}");
				}
				prop1.Dispose();
				throw;
			}

			return (prop1, prop2);
		}

		public class Property<T> : IDisposable
		{
			private int _state = 0;

			internal IntPtr Handle { get; } = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)));

			public Property()
			{
				Console.WriteLine($"++++++++++++++++++++++++++++++++++++++++++++++ Created property of {typeof(T).Name} at {Handle.ToInt64()} of {Marshal.SizeOf(typeof(T))} bytes long.");
			}

			public T Value
			{
				get => Read();
				set => Write(value);
			}

			public T Read()
				=> _state == 0
					? (T)Marshal.PtrToStructure(Handle, typeof(T))
					: default;

			public bool TryRead(out T args)
			{
				if (_state == 0)
				{
					args = (T)Marshal.PtrToStructure(Handle, typeof(T));
					return true;
				}
				else
				{
					args = default;
					return false;
				}
			}

			public void Write(T args)
			{
				if (_state != 0)
				{
					return;
				}

				Marshal.StructureToPtr(args, Handle, true);
			}

			/// <inheritdoc />
			public void Dispose()
			{
				if (Interlocked.CompareExchange(ref _state, 1, 0) == 0)
				{
					Marshal.DestroyStructure(Handle, typeof(T));
					Marshal.FreeHGlobal(Handle);
				}
			}

			~Property()
			{
				Dispose();
				GC.SuppressFinalize(this);
			}
		}
	}
}
