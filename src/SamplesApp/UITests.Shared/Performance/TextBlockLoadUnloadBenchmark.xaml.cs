using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Uno.UI.Samples.Controls;

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
	[SampleControlInfoAttribute("Performance", "TextBlockLoadUnloadBenchmark", typeof(nVentive.Umbrella.Presentation.Light.ViewModelBase))]
	public sealed partial class TextBlockLoadUnloadBenchmark : UserControl
	{
		public TextBlockLoadUnloadBenchmark()
		{
			this.InitializeComponent();
			Loaded += OnControlLoaded;
		}

		private void OnControlLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			Border rootBorder = null;
			BenchmarkControl.Setup = Setup;
			BenchmarkControl.Benchmark = _ =>
			{
				const int reps = 10000;
				var view = new TextBlock() { Text = "Text" };
				var actualReps = 0;
				var dcChanged = 0;
				view.Loaded += (o, e2) =>
				{
					actualReps++;
				};
				view.DataContextChanged += (o, e2) =>
				{
					dcChanged++;
				};
				for (int i = 0; i < reps; i++)
				{
					rootBorder.Child = null;
					rootBorder.Child = view;
				}

				return $"Loaded and unloaded TextBlock {reps} times, Loaded called {actualReps} times, DataContext changed {dcChanged} times.";
			};

			async Task Setup(ContentControl benchmarkControl)
			{
				rootBorder = new Border();
				benchmarkControl.Content = rootBorder;
				await Task.Delay(50);
			}
		}
	}
}
