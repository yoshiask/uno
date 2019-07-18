using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.UI.Samples.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UITests.Shared.Windows_UI_Xaml_Controls.ListView
{
	[SampleControlInfo("ListViewTests", nameof(ListView_Selection_Automated))]
	public sealed partial class ListView_Selection_Automated : UserControl
	{
		public ListView_Selection_Automated()
		{
			this.InitializeComponent(); int[] list = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

#if __WASM__
			DataContext = list;
#else
			var cvs = new CollectionViewSource
			{
				Source = list,
				IsSourceGrouped = false,
			};

			DataContext = list;
#endif
		}
	}
}
