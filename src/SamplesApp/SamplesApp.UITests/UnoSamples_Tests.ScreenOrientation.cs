using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uno.UITest.Helpers;
using Uno.UITest.Helpers.Queries;

namespace SamplesApp.UITests
{
	[TestFixture]
	public partial class UnoSamples_Tests : SampleControlUITestBase
	{
		[Test]
		public void Screen_Orientation_Portrait_Validation()
		{
			Run("UITests.Shared.ScreenOrientation.ApplicationView_Orientation");

			_app.WaitForElement(_app.Marked("Orientation"));

			var orientationTextBlock = _app.Marked("Orientation");

			// Set orientation Portrait
			_app.SetOrientationPortrait();

			Assert.AreEqual("Portrait", orientationTextBlock.GetDependencyPropertyValue("Text")?.ToString());
		}

		[Test]
		public void Screen_Orientation_Landscape_Validation()
		{
			Run("UITests.Shared.ScreenOrientation.ApplicationView_Orientation");

			_app.WaitForElement(_app.Marked("Orientation"));

			var orientationTextBlock = _app.Marked("Orientation");

			// Set orientation Landscape
			_app.SetOrientationLandscape();

			Assert.AreEqual("Landscape", orientationTextBlock.GetDependencyPropertyValue("Text")?.ToString());
		}
	}
}
