using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UITests.Shared.Windows_UI_Xaml_Controls.ControlTemplate
{
	public partial class ControlEmpty : Control
	{

		public Windows.UI.Xaml.Controls.ControlTemplate SecondTemplate
		{
			get { return (Windows.UI.Xaml.Controls.ControlTemplate)GetValue(SecondTemplateProperty); }
			set { SetValue(SecondTemplateProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SecondTemplate.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SecondTemplateProperty =
			DependencyProperty.Register("SecondTemplate", typeof(Windows.UI.Xaml.Controls.ControlTemplate), typeof(ControlEmpty), new PropertyMetadata(null));
	}
}
