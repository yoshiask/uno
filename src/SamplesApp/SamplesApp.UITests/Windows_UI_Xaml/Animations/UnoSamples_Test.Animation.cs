using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Uno.UITest.Helpers;
using Uno.UITest.Helpers.Queries;
using Xamarin.UITest;

namespace SamplesApp.UITests.Windows_UI_Xaml.Animations
{
	[TestFixture]
	partial class UnoSamples_Tests : SampleControlUITestBase
	{
		[Test]
		public void Animation_Translation_Validation()
		{
			Run("Uno.UI.Samples.Content.UITests.Animations.ButtonAnimationTest_Translation_Automated");

			_app.WaitForElement(_app.Marked("StartAnimationButton"));
			var startAnimationButton = _app.Marked("StartAnimationButton");

			startAnimationButton.Tap();

			// Assert after clicking once while startAnimationButton
			_app.Wait(1);
			_app.Screenshot("Animation - Translation");
		}

		[Test]
		public void AnimatedButton_Translation_Validation()
		{
			Run("Uno.UI.Samples.Content.UITests.Animations.ButtonAnimationTest_Translation_Automated");

			_app.WaitForElement(_app.Marked("StartAnimationButton"));

			var clickingButton = _app.Marked("MyTranslatingButton");
			var clickingButtonPosition = clickingButton.FirstResult().Rect;
			var totalClicksText = _app.Marked("TotalClicks");
			var startAnimationButton = _app.Marked("StartAnimationButton");

			// Assert inital state 
			Assert.AreEqual("0", totalClicksText.GetDependencyPropertyValue("Text")?.ToString());

			clickingButton.Tap();

			// Assert after clicking once while clickingButton is in original position
			Assert.AreEqual("1", totalClicksText.GetDependencyPropertyValue("Text")?.ToString());

			startAnimationButton.Tap();
			_app.Wait(1);

			clickingButton.Tap();

			// Assert after clicking once while clickingButton has animated
			Assert.AreEqual("2", totalClicksText.GetDependencyPropertyValue("Text")?.ToString());
		}

		[Test]
		public void Animation_Rotation_Validation()
		{
			Run("Uno.UI.Samples.Content.UITests.Animations.ButtonAnimationTest_Rotation_Automated");

			_app.WaitForElement(_app.Marked("MyRotatingButton"));
			var startAnimationButton = _app.Marked("StartAnimationButton");

			startAnimationButton.Tap();

			// Assert after clicking once while startAnimationButton
			_app.Wait(1);
			_app.Screenshot("Animation - Rotation");
		}

		[Test]
		public void AnimatedButton_Rotation_Validation()
		{
			Run("Uno.UI.Samples.Content.UITests.Animations.ButtonAnimationTest_Rotation_Automated");

			_app.WaitForElement(_app.Marked("MyRotatingButton"));

			var clickingButton = _app.Marked("MyRotatingButton");
			var clickingButtonPosition = clickingButton.FirstResult().Rect;
			var totalClicksText = _app.Marked("TotalClicks");
			var startAnimationButton = _app.Marked("StartAnimationButton");

			// Assert inital state 
			Assert.AreEqual("0", totalClicksText.GetDependencyPropertyValue("Text")?.ToString());

			_app.TapCoordinates(clickingButtonPosition.X + 20, clickingButtonPosition.Y + 20);

			// Assert after clicking once while clickingButton is in original position
			Assert.AreEqual("1", totalClicksText.GetDependencyPropertyValue("Text")?.ToString());

			startAnimationButton.Tap();
			_app.Wait(2);

			_app.TapCoordinates(clickingButtonPosition.X - 20, clickingButtonPosition.Y + 70);

			// Assert after clicking once while clickingButton has animated
			Assert.AreEqual("2", totalClicksText.GetDependencyPropertyValue("Text")?.ToString());
		}

		[Test]
		public void Animation_Scale_Validation()
		{
			Run("Uno.UI.Samples.Content.UITests.Animations.ButtonAnimationTest_Scale_Automated");

			_app.WaitForElement(_app.Marked("StartAnimationButton"));
			var startAnimationButton = _app.Marked("StartAnimationButton");

			startAnimationButton.Tap();

			// Assert after clicking once while startAnimationButton
			_app.Wait(1);
			_app.Screenshot("Animation - Scale");
		}

		[Test]
		public void AnimatedButton_Scale_Validation()
		{
			Run("Uno.UI.Samples.Content.UITests.Animations.ButtonAnimationTest_Scale_Automated");

			_app.WaitForElement(_app.Marked("StartAnimationButton"));

			var clickingButton = _app.Marked("MyScalingButton");
			var clickingButtonPosition = clickingButton.FirstResult().Rect;
			var totalClicksText = _app.Marked("TotalClicks");
			var startAnimationButton = _app.Marked("StartAnimationButton");

			// Assert inital state 
			Assert.AreEqual("0", totalClicksText.GetDependencyPropertyValue("Text")?.ToString());

			clickingButton.Tap();

			// Assert after clicking once while clickingButton is in original position
			Assert.AreEqual("1", totalClicksText.GetDependencyPropertyValue("Text")?.ToString());

			startAnimationButton.Tap();
			_app.Wait(2);

			clickingButton.Tap();

			// Assert after clicking once while clickingButton has animated
			Assert.AreEqual("2", totalClicksText.GetDependencyPropertyValue("Text")?.ToString());
		}

		[Test]
		public void Animation_Skew_Validation()
		{
			Run("Uno.UI.Samples.Content.UITests.Animations.ButtonAnimationTest_Skew_Automated");

			_app.WaitForElement(_app.Marked("StartAnimationButton"));
			var startAnimationButton = _app.Marked("StartAnimationButton");

			startAnimationButton.Tap();

			// Assert after clicking once while startAnimationButton
			_app.Wait(1);
			_app.Screenshot("Animation - Skew");
		}

		[Test]
		public void AnimatedButton_Skew_Validation()
		{
			Run("Uno.UI.Samples.Content.UITests.Animations.ButtonAnimationTest_Skew_Automated");

			_app.WaitForElement(_app.Marked("StartAnimationButton"));

			var clickingButton = _app.Marked("MySkewingButton");
			var clickingButtonPosition = clickingButton.FirstResult().Rect;
			var totalClicksText = _app.Marked("TotalClicks");
			var startAnimationButton = _app.Marked("StartAnimationButton");

			// Assert inital state 
			Assert.AreEqual("0", totalClicksText.GetDependencyPropertyValue("Text")?.ToString());

			_app.TapCoordinates(clickingButtonPosition.X + 25, clickingButtonPosition.Y + 5);

			// Assert after clicking once while clickingButton is in original position
			Assert.AreEqual("1", totalClicksText.GetDependencyPropertyValue("Text")?.ToString());

			startAnimationButton.Tap();
			_app.Wait(2);

			_app.TapCoordinates(clickingButtonPosition.X + 25, clickingButtonPosition.Y + 50);

			// Assert after clicking once while clickingButton has animated
			Assert.AreEqual("2", totalClicksText.GetDependencyPropertyValue("Text")?.ToString());
		}

		[Test]
		public void Animation_Composite_Validation()
		{
			Run("Uno.UI.Samples.Content.UITests.Animations.ButtonAnimationTest_Composite_Automated");

			_app.WaitForElement(_app.Marked("StartAnimationButton"));
			var startAnimationButton = _app.Marked("StartAnimationButton");

			startAnimationButton.Tap();

			// Assert after clicking once while startAnimationButton
			_app.Wait(1);
			_app.Screenshot("Animation - Composite");
		}

		[Test]
		public void AnimatedButton_Composite_Validation()
		{
			Run("Uno.UI.Samples.Content.UITests.Animations.ButtonAnimationTest_Composite_Automated");

			_app.WaitForElement(_app.Marked("StartAnimationButton"));

			var clickingButton = _app.Marked("MyCompositeButton");
			var clickingButtonPosition = clickingButton.FirstResult().Rect;
			var totalClicksText = _app.Marked("TotalClicks");
			var startAnimationButton = _app.Marked("StartAnimationButton");

			// Assert inital state 
			Assert.AreEqual("0", totalClicksText.GetDependencyPropertyValue("Text")?.ToString());

			_app.TapCoordinates(clickingButtonPosition.X + 20, clickingButtonPosition.Y + 20);

			// Assert after clicking once while clickingButton is in original position
			Assert.AreEqual("1", totalClicksText.GetDependencyPropertyValue("Text")?.ToString());

			startAnimationButton.Tap();
			_app.Wait(3);

			_app.TapCoordinates(clickingButtonPosition.X - 20, clickingButtonPosition.Y + 70);

			// Assert after clicking once while clickingButton has animated
			Assert.AreEqual("2", totalClicksText.GetDependencyPropertyValue("Text")?.ToString());
		}
	}
}
