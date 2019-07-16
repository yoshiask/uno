using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uno.UITest.Helpers;
using Uno.UITest.Helpers.Queries;

namespace SamplesApp.UITests.Windows_UI_Xaml_Controls.SliderTest
{
	[TestFixture]
	partial class UnoSamples_Tests : SampleControlUITestBase
	{
		[Test]
		public void Slider_IsEnabled_Validation()
		{
			Run("UITests.Shared.Windows_UI_Xaml_Controls.Slider.Slider_Automated");

			_app.WaitForElement(_app.Marked("SliderContinuous"));

			var sliderContinuous = _app.Marked("SliderContinuous");
			var sliderDiscrete = _app.Marked("SliderDiscrete");
			var sliderDisabled = _app.Marked("SliderDisabled");

			var sliderContinuousPosition = sliderContinuous.FirstResult().Rect;
			var sliderDiscretePosition = sliderDiscrete.FirstResult().Rect;
			var sliderDisabledPosition = sliderDisabled.FirstResult().Rect;

			// Assert inital state 
			Assert.AreEqual("50", sliderContinuous.GetDependencyPropertyValue("Value")?.ToString());
			Assert.AreEqual("50", sliderDiscrete.GetDependencyPropertyValue("Value")?.ToString());
			Assert.AreEqual("50", sliderDisabled.GetDependencyPropertyValue("Value")?.ToString());

			// Sliding sliderContinuous
			_app.DragCoordinates(sliderContinuousPosition.X + 2, sliderContinuousPosition.Y + 2, sliderContinuousPosition.X + 200, sliderContinuousPosition.Y + 2);
			Assert.AreEqual("100", sliderContinuous.GetDependencyPropertyValue("Value")?.ToString());

			// Sliding sliderDiscrete
			_app.DragCoordinates(sliderDiscretePosition.X + 100, sliderDiscretePosition.Y + 2, sliderDiscretePosition.X - 10, sliderDiscretePosition.Y + 2);
			Assert.AreEqual("0", sliderDiscrete.GetDependencyPropertyValue("Value")?.ToString());

			// Sliding sliderDisabled
			_app.DragCoordinates(sliderDisabledPosition.X + 2, sliderDisabledPosition.Y + 2, sliderDisabledPosition.X + 200, sliderDisabledPosition.Y + 2);
			Assert.AreEqual("50", sliderDisabled.GetDependencyPropertyValue("Value")?.ToString());
		}

		[Test]
		public void Slider_InList_Validation()
		{
			Run("UITests.Shared.Windows_UI_Xaml_Controls.Slider.Slider_InList_Automated");

			_app.WaitForElement(_app.Marked("SliderFooter"));

			var sliderHeader = _app.Marked("SliderHeader");
			var sliderItem = _app.Marked("SliderItem");
			var sliderFooter = _app.Marked("SliderFooter");

			var sliderHeaderPosition = sliderHeader.FirstResult().Rect;
			var sliderItemPosition = sliderItem.FirstResult().Rect;
			var sliderFooterPosition = sliderFooter.FirstResult().Rect;

			// Assert inital state 
			Assert.AreEqual("50", sliderHeader.GetDependencyPropertyValue("Value")?.ToString());
			Assert.AreEqual("50", sliderItem.GetDependencyPropertyValue("Value")?.ToString());
			Assert.AreEqual("50", sliderFooter.GetDependencyPropertyValue("Value")?.ToString());

			// Sliding sliderContinuous
			_app.DragCoordinates(sliderHeaderPosition.X + 2, sliderHeaderPosition.Y + 2, sliderHeaderPosition.X + 200, sliderHeaderPosition.Y + 2);
			Assert.AreEqual("100", sliderHeader.GetDependencyPropertyValue("Value")?.ToString());

			// Sliding sliderDiscrete
			_app.DragCoordinates(sliderItemPosition.X + 100, sliderItemPosition.Y + 2, sliderItemPosition.X - 10, sliderItemPosition.Y + 2);
			Assert.AreEqual("0", sliderItem.GetDependencyPropertyValue("Value")?.ToString());

			// Sliding sliderDisabled
			_app.DragCoordinates(sliderFooterPosition.X + 2, sliderFooterPosition.Y + 2, sliderFooterPosition.X + 200, sliderFooterPosition.Y + 2);
			Assert.AreEqual("100", sliderFooter.GetDependencyPropertyValue("Value")?.ToString());
		}
	}
}
