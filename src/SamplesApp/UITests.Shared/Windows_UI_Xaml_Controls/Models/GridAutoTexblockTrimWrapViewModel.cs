using System;
using Uno.UI.Samples.UITests.Helpers;
using Windows.UI.Core;
using Windows.UI.Xaml.Data;

namespace Uno.UI.Samples.Presentation.SamplePages
{
	[Bindable]
    public class GridAutoTexblockTrimWrapViewModel : ViewModelBase
    {
		private string _title;

		public GridAutoTexblockTrimWrapViewModel(CoreDispatcher dispatcher) : base(dispatcher)
		{
			Title = string.Empty;
		}

		internal void IncreaseGridHeight()
		{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				System.Threading.Thread.Sleep(100);
				Title = "This is just a useless text in order to resize the ListView header but it needs to be reeeeeeeeeeeeeeeaaaallly long so I just put some text to make it longer.";
			});
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		}

		#region Date
		public DateTimeOffset CurrentDate => DateTimeOffset.Now;

		public string Day => CurrentDate.Day.ToString();

		public string Month
		{
			get
			{
				var month = CurrentDate.ToString("MMM");
				return char.ToUpper(month[0]) + month.Substring(1);
			}
		}

		public string Year => CurrentDate.ToString("yyyy");
		#endregion

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
