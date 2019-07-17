using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.UI.Samples.Controls;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;

namespace UITests.Shared.Performance
{
	[SampleControlInfoAttribute("Performance", "BorderMeasureBenchmark", typeof(nVentive.Umbrella.Presentation.Light.ViewModelBase))]
	public sealed partial class BorderMeasureBenchmark : UserControl
	{
		public BorderMeasureBenchmark()
		{
			this.InitializeComponent();
			Loaded += OnControlLoaded;
		}

		private void OnControlLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			var view = new Border() { Background = new SolidColorBrush(Colors.Tomato), Width = 50, Height = 50 };
			BenchmarkControl.Setup = Setup;
			BenchmarkControl.Benchmark = _ =>
			{
				const int reps = 10000;

				for (int i = 0; i < reps; i++)
				{
					view.InvalidateMeasure();
					view.Measure(new Size(500, 500));
				}

				return $"Measured Border {reps} times, DesiredSize={view.DesiredSize}.";
			};

			async Task Setup(ContentControl benchmarkControl)
			{
				benchmarkControl.Content = view;
				await Task.Delay(50);
			}
		}
	}
}

