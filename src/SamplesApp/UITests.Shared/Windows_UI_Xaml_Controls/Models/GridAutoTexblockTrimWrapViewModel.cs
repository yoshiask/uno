using System;
using System.Collections.Generic;
using System.Text;
using Uno.UI.Samples.UITests.Helpers;
using Windows.UI.Core;
using Windows.UI.Xaml.Data;

namespace Uno.UI.Samples.Presentation.SamplePages
{
	[Bindable]
    public class GridAutoTexblockTrimWrapViewModel : ViewModelBase
    {
		private string _title;
		private string _day;
		private string _month;
		private string _year;

		public GridAutoTexblockTrimWrapViewModel(CoreDispatcher dispatcher) : base(dispatcher)
		{
			Day = "12";
			Month = "Mar";
			Year = "2019";
			Title = string.Empty;
		}

		internal void IncreaseGridHeight()
		{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				System.Threading.Thread.Sleep(3000);
				Title = "This is just a useless text in order to resize the ListView header but it needs to be reeeeeeeeeeeeeeeaaaallly long so I just put some text to make it longer.";
			});
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		}

		public string Day
		{
			get => _day;
			set
			{
				_day = value;
#if XAMARIN
				RaisePropertyChanged();
#endif
			}
		}

		public string Month
		{
			get => _month;
			set
			{
				_month = value;
#if XAMARIN
				RaisePropertyChanged();
#endif
			}
		}

		public string Year
		{
			get => _year;
			set
			{
				_year = value;
#if XAMARIN
				RaisePropertyChanged();
#endif
			}
		}

		public string Title
		{
			get => _title;
			set
			{
				_title = value;
#if XAMARIN
				RaisePropertyChanged();
#endif
			}
		}
	}
}
