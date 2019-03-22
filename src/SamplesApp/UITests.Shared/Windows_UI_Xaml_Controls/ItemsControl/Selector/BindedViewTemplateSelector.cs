using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SamplesApp.Windows_UI_Xaml_Controls.ItemsControl
{
	public class BindedViewTemplateSelector : DataTemplateSelector
	{
		public DataTemplate CheckBoxContainerTemplate { get; set; }
		public DataTemplate ToggleButtonContainerTemplate { get; set; }
		public DataTemplate TextBoxContainerTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			return SelectTemplateCore(item);
		}

		protected override DataTemplate SelectTemplateCore(object item)
		{
			if (item is CheckBoxViewModel)
			{
				return CheckBoxContainerTemplate;
			}
			else if (item is TextBoxViewModel)
			{
				return TextBoxContainerTemplate;
			}
			else if (item is ToggleButtonViewModel)
			{
				return ToggleButtonContainerTemplate;
			}

			throw new InvalidCastException("This view is not supported for the following tests");
		}
	}
}
