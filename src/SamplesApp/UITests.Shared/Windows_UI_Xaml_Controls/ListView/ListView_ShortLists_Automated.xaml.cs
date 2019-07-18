using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno;
using Uno.UI.Samples.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UITests.Shared.Windows_UI_Xaml_Controls.ListView
{
	[SampleControlInfo("ListViewTests", nameof(ListView_ShortLists_Automated))]
	public sealed partial class ListView_ShortLists_Automated : UserControl
	{
		public ListView_ShortLists_Automated()
		{
			this.InitializeComponent();

			int[] list_1 = { 1 };
			int[] list_2 = { 1, 2 };
			var section1 = new Grouping<string, int>("Section 1", list_1);
			var section2 = new Grouping<string, int>("Section 2", list_1);

			var listViewNoGroupedItemHorizontal = ListViewNoGroupedItemHorizontal;
			var listViewWithGroupedItemHorizontal = ListViewWithGroupedItemHorizontal;
			var listViewNoGroupedItemVertical = ListViewNoGroupedItemVertical;
			var listViewWithGroupedItemVertical = ListViewWithGroupedItemVertical;

			listViewNoGroupedItemHorizontal.ItemsSource = list_2;
			listViewNoGroupedItemVertical.ItemsSource = list_2;

#if __WASM__
			listViewWithGroupedItemVertical.ItemsSource = listViewWithGroupedItemHorizontal.ItemsSource = section1
				.Concat(section2)
				.ToArray();
#else
			var cvs = new CollectionViewSource
			{
				Source = new[] { section1, section2 },
				IsSourceGrouped = true,
			};

			listViewWithGroupedItemHorizontal.ItemsSource = cvs.View;
			listViewWithGroupedItemVertical.ItemsSource = cvs.View;
#endif
		}
	}
}
