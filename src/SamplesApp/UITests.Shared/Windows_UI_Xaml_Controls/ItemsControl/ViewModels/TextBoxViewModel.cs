using Uno.UI.Samples.UITests.Helpers;
using Windows.UI.Core;

namespace SamplesApp.Windows_UI_Xaml_Controls.ItemsControl
{
	public class TextBoxViewModel : ViewModelBase
	{
		private string _text;

		public TextBoxViewModel(CoreDispatcher dispatcher) : base(dispatcher)
		{
			Text = string.Empty;
		}

		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				RaisePropertyChanged();
			}
		}
	}
}
