using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Uno.UITest.Helpers;
using Uno.UITest.Helpers.Queries;
using Xamarin.UITest;

namespace SamplesApp.UITests.Windows_UI_Xaml_Controls.DatePickerTests
{
	[TestFixture]
	partial class UnoSamples_Tests : SampleControlUITestBase
	{
		[Test]
		public void DatePicker_IsEnabled_Validation()
		{
			Run("UITests.Shared.Windows_UI_Xaml_Controls.DatePicker.Date_Picker_Automated");

			_app.WaitForElement(_app.Marked("btnApplyNewDate"));

			var txtSelectedDate = _app.Marked("txtSelectedDate");
			var myDatePicker = _app.Marked("myDatePicker");
			var toggleMyDatePickerIsEnable = _app.Marked("ToggleMyDatePickerIsEnable");
			var previousDate = txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString();

			// Assert inital state 
			Assert.AreEqual("1/1/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());
			Assert.AreEqual("True", myDatePicker.GetDependencyPropertyValue("IsEnabled")?.ToString());
			_app.Screenshot("DatePicker - IsEnabled Validation - 1 - InitialState");

			// Disable date Picker
			toggleMyDatePickerIsEnable.Tap();

			// Assert that date Picker is disabled
			Assert.AreEqual("False", myDatePicker.GetDependencyPropertyValue("IsEnabled")?.ToString());

			// Open DatePicker
			myDatePicker.Tap();

			// Assert that date Picker is disabled
			_app.Screenshot("DatePicker - IsEnabled Validation - 2 - After clicking when DatePicker disabled");
		}

		[Test]
		public void DatePicker_Month_Validation()
		{
			Run("UITests.Shared.Windows_UI_Xaml_Controls.DatePicker.Date_Picker_Automated");

			_app.WaitForElement(_app.Marked("btnApplyNewDate"));

			var txtSelectedDate = _app.Marked("txtSelectedDate");
			var myDatePicker = _app.Marked("myDatePicker");

			var btnJan = _app.Marked("btnJan");
			var btnFeb = _app.Marked("btnFeb");
			var btnMar = _app.Marked("btnMar");
			var btnApr = _app.Marked("btnApr");
			var btnMay = _app.Marked("btnMay");
			var btnJune = _app.Marked("btnJune");
			var btnJuly = _app.Marked("btnJuly");
			var btnAug = _app.Marked("btnAug");
			var btnSep = _app.Marked("btnSep");
			var btnOct = _app.Marked("btnOct");
			var btnNov = _app.Marked("btnNov");
			var btnDec = _app.Marked("btnDec");

			// Assert inital state 
			Assert.AreEqual("1/1/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());

			// Setting month to February
			btnFeb.Tap();
			Assert.AreEqual("2/1/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());

			// Setting month to March
			btnMar.Tap();
			Assert.AreEqual("3/1/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());

			// Setting month to April
			btnApr.Tap();
			Assert.AreEqual("4/1/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());

			// Setting month to May
			btnMay.Tap();
			Assert.AreEqual("5/1/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());

			// Setting month to June
			btnJune.Tap();
			Assert.AreEqual("6/1/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());

			// Setting month to July
			btnJuly.Tap();
			Assert.AreEqual("7/1/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());

			// Setting month to August
			btnAug.Tap();
			Assert.AreEqual("8/1/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());

			// Setting month to September
			btnSep.Tap();
			Assert.AreEqual("9/1/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());

			// Setting month to October
			btnOct.Tap();
			Assert.AreEqual("10/1/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());

			// Setting month to November
			btnNov.Tap();
			Assert.AreEqual("11/1/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());

			// Setting month to December
			btnDec.Tap();
			Assert.AreEqual("12/1/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());

			// Setting month to January
			btnJan.Tap();
			Assert.AreEqual("1/1/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());
		}

		[Test]
		public void DatePicker_Day_Validation()
		{
			Run("UITests.Shared.Windows_UI_Xaml_Controls.DatePicker.Date_Picker_Automated");

			_app.WaitForElement(_app.Marked("btnApplyNewDate"));

			var txtSelectedDate = _app.Marked("txtSelectedDate");
			var txtNewDay = _app.Marked("txtNewDay");
			var btnApplyNewDate = _app.Marked("btnApplyNewDate");

			// Assert inital state 
			Assert.AreEqual("1/1/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());

			// Change day
			txtNewDay.Tap();
			_app.EnterText("7");
			_app.DismissKeyboard();
			btnApplyNewDate.Tap();

			// Assert after changing Day 
			Assert.AreEqual("1/7/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());
		}

		[Test]
		public void DatePicker_Year_Validation()
		{
			Run("UITests.Shared.Windows_UI_Xaml_Controls.DatePicker.Date_Picker_Automated");

			_app.WaitForElement(_app.Marked("btnApplyNewDate"));

			var txtSelectedDate = _app.Marked("txtSelectedDate");
			var txtNewYear = _app.Marked("txtNewYear");
			var btnApplyNewDate = _app.Marked("btnApplyNewDate");

			// Assert inital state 
			Assert.AreEqual("1/1/0001 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());

			// Change day
			txtNewYear.Tap();
			_app.EnterText("2014");
			_app.DismissKeyboard();
			btnApplyNewDate.Tap();

			// Assert after changing Day 
			Assert.AreEqual("1/1/2014 12:00:00 AM +00:00", txtSelectedDate.GetDependencyPropertyValue("Text")?.ToString());
		}
	}
}
