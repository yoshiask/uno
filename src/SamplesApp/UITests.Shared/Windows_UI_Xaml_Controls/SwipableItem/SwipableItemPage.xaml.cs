using System.Threading;
using System.Threading.Tasks;
using UITests.Shared.Windows_UI_Xaml_Controls.Models;
using Uno.UI.Samples.Controls;
using Uno.UI.Samples.Presentation.SamplePages;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UITests.Shared.Windows_UI_Xaml_Controls.SwipableItem
{
	[SampleControlInfo("SwipableItemPage", "SwipableItem_Example", typeof(SwipableItemViewModel))]
	public sealed partial class SwipableItemPage : Page
	{
		public SwipableItemPage()
		{
			this.InitializeComponent();
		}
	}
}
