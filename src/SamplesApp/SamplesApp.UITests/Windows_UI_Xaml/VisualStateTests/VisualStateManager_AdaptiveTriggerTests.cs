using NUnit.Framework;
using SamplesApp.UITests.TestFramework;
using Uno.UITest.Helpers;
using Uno.UITest.Helpers.Queries;

namespace SamplesApp.UITests.Windows_UI_Xaml.VisualStateTests
{
	[TestFixture]
	public class VisualStateManager_AdaptiveTriggerTests : SampleControlUITestBase
	{
		[Test]
		[AutoRetry]
		public void NormalTest()
		{
			Run("UITests.Shared.Windows_UI_Xaml.ViusalStateTests.VisualState_AdaptiveTrigger_Storyboard");

			_app.WaitForElement("txt");  // Wait for UI to be loaded

			var currentWidth = _app.Marked("currentWidth").GetDependencyPropertyValue<string>("Text").Replace("px", "");

			var windowWidth = double.Parse(currentWidth);

			var slider = _app.Marked("slider");

			_app.WaitForDependencyPropertyValue<string>(_app.Marked("currentState"), "Text", "Large");

			_app.SetDependencyPropertyValue(_app.Marked("currentState"), "Text", "Large");

			_app.SetSliderValue(slider, windowWidth - 50.0d);

			_app.WaitForDependencyPropertyValue<string>(_app.Marked("currentState"), "Text", "Large");

			_app.SetSliderValue(slider, windowWidth + 50.0d);

			_app.WaitForDependencyPropertyValue<string>(_app.Marked("currentState"), "Text", "Small");

			_app.SetSliderValue(slider, windowWidth);

			_app.WaitForDependencyPropertyValue<string>(_app.Marked("currentState"), "Text", "Large");

			_app.SetSliderValue(slider, windowWidth + 10.0d);

			_app.WaitForDependencyPropertyValue<string>(_app.Marked("currentState"), "Text", "Large");
		}
	}
}
