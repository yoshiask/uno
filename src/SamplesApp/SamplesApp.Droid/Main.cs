using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Nostra13.Universalimageloader.Core;
using Windows.UI.Xaml.Media;

namespace SamplesApp.Droid
{
	[global::Android.App.ApplicationAttribute(
		Label = "@string/ApplicationName",
		LargeHeap = true,
		HardwareAccelerated = true,
		Theme = "@style/AppTheme"
	)]
	public class Application : Windows.UI.Xaml.NativeApplication
	{
		public Application(IntPtr javaReference, JniHandleOwnership transfer)
			: base(new App(), javaReference, transfer)
		{
			Android_EnableTracing();
			Android.App.Application.Context.Resources.GetIdentifier("String1", "string", Android.App.Application.Context.PackageName);

			ConfigureUniversalImageLoader();
		}

		private void ConfigureUniversalImageLoader()
		{
			// Create global configuration and initialize ImageLoader with this config
			ImageLoaderConfiguration config = new ImageLoaderConfiguration
				.Builder(Context)
				.Build();

			ImageLoader.Instance.Init(config);

			ImageSource.DefaultImageLoader = ImageLoader.Instance.LoadImageAsync;
		}

		public static void Android_EnableTracing()
		{
			nVentive.Umbrella.Diagnostics.Eventing.Tracing.IsEnabled = true;

			var traceFolder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments);

			if (!traceFolder.Exists())
			{
				traceFolder.Mkdirs();
			}

			// You may need to add permission WRITE_EXTERNAL_STORAGE in manifest
			// to avoid the next line to produce a System.UnauthorizedAccessException
			nVentive.Umbrella.Diagnostics.Eventing.Tracing.Factory =
				new nVentive.Umbrella.Services.Diagnostics.Eventing.EventProviderFactory(
					new nVentive.Umbrella.Services.Diagnostics.Eventing.FileEventSink(traceFolder.AbsolutePath)
				);

			Android_EnableUnoTracing();
		}

		public static void Android_EnableUnoTracing()
		{
			Uno.Diagnostics.Eventing.Tracing.IsEnabled = true;

			Uno.Diagnostics.Eventing.Tracing.Factory =
				new nVentive.Umbrella.Services.Diagnostics.Eventing.UnoEventProviderFactory(
					nVentive.Umbrella.Diagnostics.Eventing.Tracing.Factory
				);
		}
	}
}
