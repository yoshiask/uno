using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.UI.Samples.Controls;
using Uno.UI.Samples.UITests.Helpers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UITests.Shared.Windows_UI_Xaml.BindableDrawerLayout
{
	[SampleControlInfoAttribute("BindableDrawerLayout", "BindableDrawerLayout_LeftRightPaneEnabled", typeof(BindableDrawerLayoutViewModel))]
	public sealed partial class BindableDrawerLayout_LeftRightPaneEnabled : UserControl
	{
		public BindableDrawerLayout_LeftRightPaneEnabled()
		{
			this.InitializeComponent();
		}
	}

	[Bindable]
	public class BindableDrawerLayoutViewModel : ViewModelBase
	{
		public BindableDrawerLayoutViewModel(CoreDispatcher dispatcher) : base(dispatcher)
		{
			//Build(b => b
			//	.Properties(pb => pb
			//		.Attach("LeftPaneEnabled", () => true)
			//		.Attach("RightPaneEnabled", () => true)
			//		.Attach("Enabled", () => true)
			//	)
			//);
		}
	}
}
