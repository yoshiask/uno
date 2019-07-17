using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using SamplesApp.Windows_UI_Xaml_Controls.Models;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UITests.Shared.Windows_UI_Xaml_Controls.DatePicker
{

	[SampleControlInfo("Date Picker", nameof(Date_Picker_Automated))]
	public sealed partial class Date_Picker_Automated : UserControl
	{

		public Date_Picker_Automated()
		{
			this.InitializeComponent();
			txtSelectedDate.Text = myDatePicker.Date.ToString();
		}

		private void changeMonth(object sender, RoutedEventArgs e)
		{
			var btn = sender as Windows.UI.Xaml.Controls.Button;
			var btn_value = btn.Content.ToString();

			myDatePicker.Date = new DateTimeOffset(
				new DateTime(
					myDatePicker.Date.Year,
					int.Parse((string)btn.Tag),
					myDatePicker.Date.Day),
				new TimeSpan(0)
			);

			txtSelectedDate.Text = myDatePicker.Date.ToString();
		}

		private void applyNewDate(object sender, RoutedEventArgs e)
		{
			var newDay = txtNewDay.Text != "" ? int.Parse(txtNewDay.Text) : myDatePicker.Date.Day;
			var newYear = txtNewYear.Text != "" ? int.Parse(txtNewYear.Text) : myDatePicker.Date.Year;

			myDatePicker.Date = new DateTimeOffset(
				new DateTime(
					newYear,
					myDatePicker.Date.Month,
					newDay),
				new TimeSpan(0)
			);

			txtSelectedDate.Text = myDatePicker.Date.ToString();
		}
	}
}
