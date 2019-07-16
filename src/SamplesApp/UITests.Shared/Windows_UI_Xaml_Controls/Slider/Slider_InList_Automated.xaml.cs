using Windows.UI.Xaml.Controls;
using Uno.UI.Samples.Controls;
using Windows.UI.Xaml.Data;
using Uno;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UITests.Shared.Windows_UI_Xaml_Controls.Slider
{
	[SampleControlInfo("Slider", nameof(Slider_InList_Automated))]
	public sealed partial class Slider_InList_Automated : UserControl
	{
		public Slider_InList_Automated()
		{
			this.InitializeComponent();

			int[] list = { 1, 2 };
			var section1 = new Grouping<string, int>("First section", list);
			var section2 = new Grouping<string, int>("Second section", list);

			var cvs = new CollectionViewSource
			{
				Source = new[] { section1, section2 },
				IsSourceGrouped = true,
			};

			DataContext = cvs.View;
		}
	}
}
