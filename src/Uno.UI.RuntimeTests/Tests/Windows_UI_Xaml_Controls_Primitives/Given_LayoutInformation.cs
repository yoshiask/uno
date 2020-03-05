using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Private.Infrastructure;

namespace Uno.UI.RuntimeTests.Tests.Windows_UI_Xaml_Controls_Primitives
{
	[TestClass]
	public class Given_LayoutInformation
	{
		[TestMethod]
		[RunsOnUIThread]
		public async Task TestAvailableSizeAndLayoutSlot()
		{
			var inner = new Rectangle {Fill = new SolidColorBrush(Colors.Cyan), MinWidth = 7, MinHeight = 13};
			var innerContainer = new Border
			{
				Background = new SolidColorBrush(Colors.BlueViolet),
				BorderBrush = new SolidColorBrush(Colors.Navy),
				VerticalAlignment = VerticalAlignment.Center,
				BorderThickness = new Thickness(5, 12, 3, 1),
				Margin = new Thickness(17),
				Child = inner
			};
			var container = new Border
			{
				Background = new SolidColorBrush(Colors.Firebrick),
				BorderBrush = new SolidColorBrush(Colors.DarkOrange),
				Padding = new Thickness(1),
				Margin = new Thickness(3),
				BorderThickness = new Thickness(5),
				HorizontalAlignment = HorizontalAlignment.Right,
				Child = innerContainer
			};
			var outerBorder = new Border {Width = 200, Height = 200, Child = container};

			TestServices.WindowHelper.WindowContent = outerBorder;

			await TestServices.WindowHelper.WaitForIdle();

			using (new AssertionScope("Measurements"))
			{
				LayoutInformation.GetAvailableSize(container).Should()
					.Be(new Size(200, 200), 0.5, "container availableSize");
				LayoutInformation.GetAvailableSize(innerContainer).Should()
					.Be(new Size(182, 182), 0.5, "innerContainer availableSize");
				LayoutInformation.GetAvailableSize(inner).Should()
					.Be(new Size(140, 135), 0.5, "inner availableSize");

				LayoutInformation.GetLayoutSlot(container).Should()
					.Be(new Rect(0, 0, 200, 200), 0.5, "container LayoutSlot");
				LayoutInformation.GetLayoutSlot(innerContainer).Should()
					.Be(new Rect(6, 6, 49, 182), 0.5, "innerContainer LayoutSlot");
				LayoutInformation.GetLayoutSlot(inner).Should()
					.Be(new Rect(5, 12, 7, 13), 0.5, "inner LayoutSlot");
			}
		}
	}
}
