using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace UITests.Shared.AutomationPeer
{
	[SampleControlInfo("AutomationPeer", nameof(AutomationPeer_Interactive))]
	public sealed partial class AutomationPeer_Interactive : UserControl
	{
		public AutomationPeer_Interactive()
		{
			this.InitializeComponent();
		}
		private void OnClick(object sender, RoutedEventArgs e)
		{
			new Windows.UI.Popups.MessageDialog("Clicked!").ShowAsync();
		}
	}
}
