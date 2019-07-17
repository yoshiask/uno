using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uno.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UITests.Shared.Performance
{
	/// <summary>
	/// Control that runs a specified performance benchmark and displays the results.
	/// </summary>
	public partial class BenchmarkControl : ContentControl
	{
		public Func<ContentControl, Task> Setup { get; set; }
		public Func<ContentControl, string> Benchmark { get; set; }
		private Button _runButton;
		private TextBlock _resultTextBlock;
		private TextBlock _errorTextBlock;
		private TextBlock _outputTextBlock;

		public double BenchmarkTimeMs
		{
			get { return (double)GetValue(BenchmarkTimeMsProperty); }
			set { SetValue(BenchmarkTimeMsProperty, value); }
		}

		public static readonly DependencyProperty BenchmarkTimeMsProperty =
			DependencyProperty.Register("BenchmarkTimeMs", typeof(double), typeof(BenchmarkControl), new PropertyMetadata(-1));


		public int Repetitions
		{
			get { return (int)GetValue(RepetitionsProperty); }
			set { SetValue(RepetitionsProperty, value); }
		}

		public static readonly DependencyProperty RepetitionsProperty =
			DependencyProperty.Register("Repetitions", typeof(int), typeof(BenchmarkControl), new PropertyMetadata(3));


		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			if (_runButton != null)
			{
				_runButton.Click -= RunTask;
			}
			_runButton = GetTemplateChild("RunButton") as Button;

			_runButton.Click += RunTask;

			_resultTextBlock = GetTemplateChild("ResultTextBlock") as TextBlock;
			_errorTextBlock = GetTemplateChild("ErrorTextBlock") as TextBlock;
			_outputTextBlock = GetTemplateChild("OutputTextBlock") as TextBlock;
		}

		private async void RunTask(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			if (Benchmark == null)
			{
				throw new InvalidOperationException($"{nameof(Benchmark)} is not set.");
			}
			try
			{
				_outputTextBlock.Visibility = Visibility.Visible;
				_outputTextBlock.Text = "Running...";
				await Task.Yield();

				if (Setup != null)
				{
					await Setup(this);
				}

				var stopwatch = new Stopwatch();
				string output = null;
				var results = new List<long>();

				for (int i = 0; i < Repetitions + 1; i++)
				{
					GC.Collect();
					GC.WaitForPendingFinalizers();

					stopwatch.Restart();

					output = Benchmark(this);

					stopwatch.Stop();

					results.Add(stopwatch.ElapsedMilliseconds);
				}

				results.RemoveAt(0); //Throw away first trial

				BenchmarkTimeMs = results.Average();
				var sd = results.Select(l => (double)l).StdDev();

				_resultTextBlock.Text = $"{BenchmarkTimeMs:g4}±{sd:g3} ms";
				_outputTextBlock.Text = output;

				_resultTextBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
				_outputTextBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
				_errorTextBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
			}
			catch (Exception ex)
			{
				_errorTextBlock.Text = ex.Message;

				_resultTextBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
				_outputTextBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
				_errorTextBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
			}
		}
	}
}
