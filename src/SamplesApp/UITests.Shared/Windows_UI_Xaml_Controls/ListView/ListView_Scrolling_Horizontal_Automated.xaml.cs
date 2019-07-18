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
	[SampleControlInfo("ListViewTests", nameof(ListView_Scrolling_Horizontal_Automated))]
	public sealed partial class ListView_Scrolling_Horizontal_Automated : UserControl
	{
		public ListView_Scrolling_Horizontal_Automated()
		{
			this.InitializeComponent();
			{
				this.InitializeComponent();

				int[] list_1 = { 1, 2, 3 };
				int[] list_2 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
				var section1 = new Grouping<string, int>("First section", list_1);
				var section2 = new Grouping<string, int>("Second section", list_1);
				var section3 = new Grouping<string, int>("Third section", list_1);
				var section4 = new Grouping<string, int>("Fourth section", list_1);

				var listViewNoGroupedItem = ListViewNoGroupedItem;
				var listViewWithGroupedItem = ListViewWithGroupedItem;

				listViewNoGroupedItem.ItemsSource = list_2;

#if __WASM__
			ListViewWithGroupedItem.ItemsSource = section1
				.Concat(section2)
				.Concat(section3)
				.Concat(section4)
				.ToArray();
#else
				var cvs = new CollectionViewSource
				{
					Source = new[] { section1, section2, section3, section4 },
					IsSourceGrouped = true,
				};

				listViewWithGroupedItem.ItemsSource = cvs.View;
#endif
			}
		}
	}
}
