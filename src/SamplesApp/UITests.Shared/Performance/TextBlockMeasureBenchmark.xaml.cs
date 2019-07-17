using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.UI.Samples.Controls;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UITests.Shared.Performance
{
	[SampleControlInfoAttribute("Performance", "TextBlockMeasureBenchmark", typeof(nVentive.Umbrella.Presentation.Light.ViewModelBase))]
	public sealed partial class TextBlockMeasureBenchmark : UserControl
	{
		public TextBlockMeasureBenchmark()
		{
			this.InitializeComponent();
			Loaded += OnControlLoaded;
		}

		private void OnControlLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var view = new TextBlock() { Text = "Text" };
			BenchmarkControl.Setup = Setup;
			BenchmarkControl.Benchmark = _ =>
			{
				const int reps = 10000;

				for (int i = 0; i < reps; i++)
				{
					view.InvalidateMeasure();
					view.Measure(new Size(500, 500));
				}

				return $"Measured TextBlock {reps} times, DesiredSize={view.DesiredSize}.";
			};

			async Task Setup(ContentControl benchmarkControl)
			{
				benchmarkControl.Content = view;
				await Task.Delay(50);
			}
		}
	}
}
