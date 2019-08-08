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

namespace UITests.Shared.Windows_UI_Xaml_Controls.Frame
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Page1 : Page
	{
		public Page1()
		{
			this.InitializeComponent();
		}

		private void OpenSplitView(object sender, RoutedEventArgs e)
		{
			var splitView = Frame?.Tag as Windows.UI.Xaml.Controls.SplitView;
			if (splitView != null)
			{
				splitView.IsPaneOpen = true;
			}
		}
	}
}
