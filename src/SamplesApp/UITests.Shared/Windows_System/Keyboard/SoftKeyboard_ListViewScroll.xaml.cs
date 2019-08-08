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

namespace UITests.Shared.Windows_System.Keyboard
{
	[SampleControlInfo("Keyboard", "SoftKeyboard_ListViewScroll")]
	public sealed partial class SoftKeyboard_ListViewScroll : UserControl
	{
		public SoftKeyboard_ListViewScroll()
		{
			this.InitializeComponent();
			this.MainList.ItemsSource = Enumerable.Range(0, 15);
		}
	}
}
