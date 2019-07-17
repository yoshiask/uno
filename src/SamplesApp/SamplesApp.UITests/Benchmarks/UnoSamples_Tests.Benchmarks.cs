using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Uno.UITest.Helpers;
using Uno.UITest.Helpers.Queries;
using Xamarin.UITest.Queries;

namespace SamplesApp.UITests.Benchmarks
{
	[TestFixture]
	public partial class UnoSamples_Tests
	{
		[Test]
		public void ViewCreationBenchmark()
		{
			Run("Uno.UI.Samples.Content.UITests.Performance.ViewCreationBenchmark");

			RunBenchmark();
		}

		[Test]
		public void ViewLoadUnloadBenchmark()
		{
			Run("Uno.UI.Samples.Content.UITests.Performance.ViewLoadUnloadBenchmark");

			RunBenchmark();
		}

		[Test]
		public void BorderLoadUnloadBenchmark()
		{
			Run("Uno.UI.Samples.Content.UITests.Performance.BorderLoadUnloadBenchmark");

			RunBenchmark();
		}

		[Test]
		public void TextBlockLoadUnloadBenchmark()
		{
			Run("Uno.UI.Samples.Content.UITests.Performance.TextBlockLoadUnloadBenchmark");

			RunBenchmark();
		}

		[Test]
		public void BorderMeasureBenchmark()
		{
			Run("Uno.UI.Samples.Content.UITests.Performance.BorderMeasureBenchmark");

			RunBenchmark();
		}

		[Test]
		public void TextBlockMeasureBenchmark()
		{
			Run("Uno.UI.Samples.Content.UITests.Performance.TextBlockMeasureBenchmark");

			RunBenchmark();
		}

		private void RunBenchmark()
		{
			var runButton = _app.Marked("RunButton");

			_app.WaitForElement(runButton);

			var position = runButton.FirstResult().Rect;

			_app.TapCoordinates(position.X + 5, position.Y + 5);

			_app.WaitForElement(_app.Marked("ResultTextBlock"));

			_app.Screenshot("0 - Benchmark completed");
		}
	}
}

