using Uno.UI.Samples.Controls;
using Uno.UI.Samples.Presentation.SamplePages;
using Windows.UI.Xaml.Controls;

namespace Uno.UI.Samples.Content.UITests.GridTests
{
	[SampleControlInfoAttribute("Grid", "Grid_Auto_TextBlock_Trimming_And_Wrapping", typeof(GridAutoTexblockTrimWrapViewModel))]
	public sealed partial class Grid_Auto_TextBlock_Trimming_And_Wrapping : Page
	{
		public Grid_Auto_TextBlock_Trimming_And_Wrapping()
		{
			InitializeComponent();
		}

		private void MySampleControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			if(!(DataContext is GridAutoTexblockTrimWrapViewModel dataContext))
			{
				return;
			}

			dataContext.IncreaseGridHeight();
		}
	}
}
