namespace Uno.Foundation.Interop {
	export class ManagedObject {
		private static assembly: UI.Interop.IMonoAssemblyHandle;
		private static dispatchMethod: (handle: string, method: string, parameters: string) => string;

		private static init() {
			ManagedObject.dispatchMethod = (<any>Module).mono_bind_static_method("[Uno.Foundation] Uno.Foundation.Interop.JSObject:Dispatch");
		}

		public static dispatch(handle: string, method: string, parameters: string): any {
			if (!ManagedObject.dispatchMethod) {
				ManagedObject.init();
			}

			var serializedResult = ManagedObject.dispatchMethod(handle, method, parameters || "");
			var result = JSON.parse(serializedResult);

			if (result.isSuccess) {
				return result.value;
			}
		}
	}
}
