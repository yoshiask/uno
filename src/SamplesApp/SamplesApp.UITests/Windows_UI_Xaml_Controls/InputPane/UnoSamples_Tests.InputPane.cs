using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Uno.UITest.Helpers;
using Uno.UITest.Helpers.Queries;
using Xamarin.UITest;

namespace SamplesApp.UITests.Windows_UI_Xaml_Controls.InputPane
{
	[TestFixture(Uno.UITest.Helpers.Queries.Platform.Android)]
	partial class UnoSamples_Tests_Android : SampleControlUITestBase
	{
		public UnoSamples_Tests_Android(Uno.UITest.Helpers.Queries.Platform platform) : base(platform)
		{
		}

		[Test]
		public void InputPane_VirtualNavigationBar_Measurement_Validation()
		{
			const int LimitToReach = 100;
			const int SecondsWaitingTime = 3;

			Run("UITests.Shared.Windows_UI_Xaml_Controls.InputPane.InputPane_Simple");

			// Wait for the page to be loaded
			_app.WaitForElement("IsVisible");

			// Make sure to show navigation bar
			_app.Tap("ShowNavigationBar");
			_app.Wait(SecondsWaitingTime);

			// Assert InputPane is not visible and OccludedRect is NavigationBar only
			Assert.AreEqual(IsInputPaneVisibleValue(), false);
			Assert.Less(GetOccludedRectValue().Height, LimitToReach);

			// Show keyboard
			_app.Tap("TryShow");
			_app.Wait(SecondsWaitingTime);

			// Assert InputPane is visible and OccludedRect is NavigationBar + Keyboard
			Assert.AreEqual(IsInputPaneVisibleValue(), true);
			Assert.Greater(GetOccludedRectValue().Height, LimitToReach);

			// Hide keyboard
			_app.Tap("TryHide");
			_app.Wait(SecondsWaitingTime);

			// Assert InputPane is not visible and OccludedRect is NavigationBar only (back to previous state)
			Assert.AreEqual(IsInputPaneVisibleValue(), false);
			Assert.Less(GetOccludedRectValue().Height, LimitToReach);

			// Hide navigation bar
			_app.Tap("HideNavigationBar");
			_app.Wait(SecondsWaitingTime);

			// Assert InputPane is not visible and OccludedRect is nothing
			Assert.AreEqual(IsInputPaneVisibleValue(), false);
			Assert.AreEqual(GetOccludedRectValue().Height, 0);

			// Show keyboard
			_app.Tap("TryShow");
			_app.Wait(SecondsWaitingTime);

			// Assert InputPane is visible and OccludedRect is Keyboard only
			Assert.AreEqual(IsInputPaneVisibleValue(), true);
			Assert.Greater(GetOccludedRectValue().Height, LimitToReach);

			// Dismiss keyboard
			_app.Tap("TryHide");
			_app.Wait(SecondsWaitingTime);

			// Assert InputPane is visible and OccludedRect is nothing
			Assert.AreEqual(IsInputPaneVisibleValue(), false);
			Assert.AreEqual(GetOccludedRectValue().Height, 0);

			// Make sure to show navigation bar
			_app.Tap("ShowNavigationBar");
			_app.Wait(SecondsWaitingTime);

			// Assert InputPane is not visible and OccludedRect is NavigationBar only
			Assert.AreEqual(IsInputPaneVisibleValue(), false);
			Assert.Less(GetOccludedRectValue().Height, LimitToReach);
		}

		#region Private methods
		private bool IsInputPaneVisibleValue()
		{
			var isVisibleText = _app.Marked("IsVisible").GetDependencyPropertyValue("Text").ToString().Split(':')[1];
			bool.TryParse(isVisibleText, out var isInputPaneVisible);

			return isInputPaneVisible;
		}

		private Rect GetOccludedRectValue()
		{
			var occludedRectText = _app.Marked("OccludedRect").GetDependencyPropertyValue("Text").ToString().Split(':')[1];
			var rectProperties = occludedRectText.Split(',');
			double.TryParse(rectProperties[0], out var x);
			double.TryParse(rectProperties[1], out var y);
			double.TryParse(rectProperties[2], out var width);
			double.TryParse(rectProperties[3], out var height);

			return rectProperties.Length == 4
				? new Rect { X = x, Y = y, Width = width, Height = height }
				: new Rect();
		}


		public class Rect
		{
			public double X { get; set; }
			public double Y { get; set; }
			public double Width { get; set; }
			public double Height { get; set; }
		}
		#endregion
	}
}
