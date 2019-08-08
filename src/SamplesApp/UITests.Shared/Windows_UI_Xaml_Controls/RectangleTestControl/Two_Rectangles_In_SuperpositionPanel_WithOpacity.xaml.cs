using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.UI.Samples.Controls;
using Uno.UI.Samples.UITests.Helpers;
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

namespace UITests.Shared.Windows_UI_Xaml_Controls.RectangleTestControl
{
	[SampleControlInfoAttribute("RectangleTestControl", nameof(Two_Rectangles_In_SuperpositionPanel_WithOpacity), typeof(ViewModelBase))]
	public sealed partial class Two_Rectangles_In_SuperpositionPanel_WithOpacity : UserControl
	{
		public Two_Rectangles_In_SuperpositionPanel_WithOpacity()
		{
			this.InitializeComponent();
		}
	}
}
