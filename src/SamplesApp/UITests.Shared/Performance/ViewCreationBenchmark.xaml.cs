using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.UI.Samples.Controls;
using Windows.UI.Xaml;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UITests.Shared.Performance;

namespace Uno.UI.Samples.Content.UITests.Performance
{
	[SampleControlInfoAttribute("Performance", "ViewCreationBenchmark", typeof(nVentive.Umbrella.Presentation.Light.ViewModelBase))]
	public sealed partial class ViewCreationBenchmark : UserControl
	{
		public ViewCreationBenchmark()
		{
			this.InitializeComponent();
			Loaded += OnControlLoaded;
		}

		private void OnControlLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			BenchmarkControl.Benchmark = _ =>
			{
				const int reps = 10000;
				for (int i = 0; i < reps; i++)
				{
					var view = new VanillaView();
				}

				return $"Created {reps} views.";
			};
		}

	}
}
