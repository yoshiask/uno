using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace UITests.Shared.Windows_UI_Xaml_Controls.ControlTemplate
{
	public sealed partial class ControlTemplateTest
	{
		public string MyCustomContent
		{
			get { return (string)this.GetValue(MyCustomContentProperty); }
			set { this.SetValue(MyCustomContentProperty, value); }
		}

		public static readonly DependencyProperty MyCustomContentProperty =
			DependencyProperty.Register("MyCustomContent", typeof(string), typeof(ControlTemplateTest), new PropertyMetadata(null));

		public string MyTwoWayContent1
		{
			get { return (string)this.GetValue(MyTwoWayContent1Property); }
			set { this.SetValue(MyTwoWayContent1Property, value); }
		}

		public static readonly DependencyProperty MyTwoWayContent1Property =
			DependencyProperty.Register("MyTwoWayContent1", typeof(string), typeof(ControlTemplateTest), new PropertyMetadata(""));


		public string MyTwoWayContent2
		{
			get { return (string)this.GetValue(MyTwoWayContent2Property); }
			set { this.SetValue(MyTwoWayContent2Property, value); }
		}

		public static readonly DependencyProperty MyTwoWayContent2Property =
			DependencyProperty.Register("MyTwoWayContent2", typeof(string), typeof(ControlTemplateTest), new PropertyMetadata(""));
	}
}
