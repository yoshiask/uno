using Uno.UI.Samples.UITests.Helpers;
using Windows.UI.Core;

namespace SamplesApp.Windows_UI_Xaml_Controls.ItemsControl
{
	public class ToggleButtonViewModel : ViewModelBase
	{
		private bool _isChecked;

		public ToggleButtonViewModel(CoreDispatcher dispatcher) : base(dispatcher)
		{
			IsChecked = false;
		}

		public bool IsChecked
		{
			get => _isChecked;
			set
			{
				_isChecked = value;
				RaisePropertyChanged();
			}
		}
	}
}
