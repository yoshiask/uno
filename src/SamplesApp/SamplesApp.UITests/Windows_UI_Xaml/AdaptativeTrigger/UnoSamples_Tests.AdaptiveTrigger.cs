using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Uno.UITest.Helpers;
using Uno.UITest.Helpers.Queries;
using Xamarin.UITest;

namespace SamplesApp.UITests.Windows_UI_Xaml.AdaptativeTrigger
{
	[TestFixture]
	partial class UnoSamples_Tests : SampleControlUITestBase
	{
		[Test]
		public void AdaptiveTrigger_TextBlock_Validation()
		{
			Run("UITests.Shared.AdaptiveTrigger.AdaptiveTrigger_TextBlock");

			var textblock = _app.Marked("AdaptiveTriggerTextBlock");

			//Verify in Portrait
			_app.SetOrientationPortrait();
			_app.Wait(2);
			Assert.AreEqual("Narrow Text", textblock.GetDependencyPropertyValue("Text")?.ToString());
			Assert.AreEqual("30", textblock.GetDependencyPropertyValue("FontSize")?.ToString());

			//Verify in Landscape
			_app.SetOrientationLandscape();
			_app.Wait(2);
			Assert.AreEqual("Wide Text", textblock.GetDependencyPropertyValue("Text")?.ToString());
			Assert.AreEqual("50", textblock.GetDependencyPropertyValue("FontSize")?.ToString());

			//Again verify in Portrait
			_app.SetOrientationPortrait();
			_app.Wait(2);
			Assert.AreEqual("Narrow Text", textblock.GetDependencyPropertyValue("Text")?.ToString());
			Assert.AreEqual("30", textblock.GetDependencyPropertyValue("FontSize")?.ToString());
		}

		[Test]
		public void AdaptiveTrigger_TextBox_Validation()
		{
			Run("UITests.Shared.AdaptiveTrigger.AdaptiveTrigger_TextBox");

			var textbox = _app.Marked("AdaptiveTriggerTextBox");
			var textBlockFontSizeValue = _app.Marked("TextBlockFontSizeValue");

			_app.DismissKeyboard();
			_app.Wait(1);

			//Verify in Portrait
			_app.SetOrientationPortrait();
			_app.Wait(2);

			Assert.AreEqual("Narrow font textbox", textbox.GetDependencyPropertyValue("Text")?.ToString());
			Assert.AreEqual("20", textBlockFontSizeValue.GetDependencyPropertyValue("Text")?.ToString());

			//Verify in Landscape			
			_app.SetOrientationLandscape();
			_app.Wait(2);

			Assert.AreEqual("Wide font textbox", textbox.GetDependencyPropertyValue("Text")?.ToString());
			Assert.AreEqual("40", textBlockFontSizeValue.GetDependencyPropertyValue("Text")?.ToString());

			//Again verify in Portrait
			_app.SetOrientationPortrait();
			_app.Wait(2);

			Assert.AreEqual("Narrow font textbox", textbox.GetDependencyPropertyValue("Text")?.ToString());
			Assert.AreEqual("20", textBlockFontSizeValue.GetDependencyPropertyValue("Text")?.ToString());
		}
	}
}
