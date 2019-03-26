using System;
using Uno.UI.Samples.UITests.Helpers;
using Windows.UI.Core;

namespace SamplesApp.Windows_UI_Xaml_Controls.ItemsControl
{
	public class RecycledTemplateViewModel : ViewModelBase
    {
		private string _recycledText;

		public RecycledTemplateViewModel(CoreDispatcher dispatcher) : base(dispatcher)
		{
			RecycledText = $"New item {new Random().Next()}";
		}

		public string RecycledText
		{
			get => _recycledText;
			set
			{
				_recycledText = value;
				RaisePropertyChanged();
			}
		}
    }
}
