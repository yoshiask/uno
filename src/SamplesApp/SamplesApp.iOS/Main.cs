using System;
using System.IO;
using UIKit;

namespace SamplesApp.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main(string[] args)
		{
			IOS_EnableTracing();

			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main(args, null, typeof(App));
		}

		public static void IOS_EnableTracing()
		{
			nVentive.Umbrella.Diagnostics.Eventing.Tracing.IsEnabled = true;

			var traceFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			var traceUmbrellaFolder = Path.Combine(traceFolder, "UmbrellaTrace");

			if (!Directory.Exists(traceUmbrellaFolder))
			{
				Directory.CreateDirectory(traceUmbrellaFolder);
			}

			nVentive.Umbrella.Diagnostics.Eventing.Tracing.Factory =
				new nVentive.Umbrella.Services.Diagnostics.Eventing.EventProviderFactory(
					new nVentive.Umbrella.Services.Diagnostics.Eventing.FileEventSink(traceUmbrellaFolder)
				);

			iOS_EnableUnoTracing();
		}

		public static void iOS_EnableUnoTracing()
		{
			Uno.Diagnostics.Eventing.Tracing.IsEnabled = true;

			Uno.Diagnostics.Eventing.Tracing.Factory =
				new nVentive.Umbrella.Services.Diagnostics.Eventing.UnoEventProviderFactory(
					nVentive.Umbrella.Diagnostics.Eventing.Tracing.Factory
				);
		}
	}
}
