using NUnit.Framework;
using SamplesApp.UITests.TestFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uno.UITest.Helpers;
using Uno.UITest.Helpers.Queries;

namespace SamplesApp.UITests.Windows_UI_Xaml_Controls.ListViewTests
{
	[TestFixture]
	public partial class ListViewTests_Tests : SampleControlUITestBase
	{
		[Test]
		[ActivePlatforms(Platform.Android, Platform.iOS)]
		public void SnapPoints_Validation()
		{
			const string nearListID = "snap_list_near";
			const string centerListID = "snap_list_center";
			const string nearListItemID = "snap_list_near_item";
			const string centerListItemID = "snap_list_center_item";

			Run("SamplesApp.Windows_UI_Xaml_Controls.ListView.ListViewSnapPointsMandatorySingle");

			_app.WaitForElement(nearListID);
			_app.Screenshot("Snap points initial");

			var nearList = _app.Marked(nearListID);
			var nearListBounds = nearList.FirstResult().Rect;
			_app.SwipeRightToLeft(nearListID);
			WaitForSwipe();
			_app.WaitForElement(nearListItemID);

			_app.Screenshot("Snap near swiped once");

			var nearItemsBounds = _app.Marked(nearListItemID).Results().Select(r => r.Rect).ToArray();
			Assert.True(nearItemsBounds.Any(bounds => ApproxEquals(bounds.X, nearListBounds.X)));

			_app.WaitForElement(centerListID);
			var centerListBounds = _app.Marked(centerListID).FirstResult().Rect;
			_app.SwipeRightToLeft(centerListID);
			WaitForSwipe();

			_app.Screenshot("Snap center swiped once");

			_app.SwipeRightToLeft(centerListID);
			WaitForSwipe();

			_app.Screenshot("Snap center swiped twice");

			_app.WaitForElement(centerListItemID);
			var centerItemsBounds = _app.Marked(centerListItemID).Results().Select(r => r.Rect).ToArray();
			Assert.True(centerItemsBounds.Any(bounds => ApproxEquals(bounds.CenterX, centerListBounds.CenterX)));
		}

		private void WaitForSwipe()
		{
			_app.Wait(.5f);
		}

		private static bool ApproxEquals(float f1, float f2)
		{
			//Not hoping for pixel-perfection here
			return Math.Abs(f1 - f2) < 1;
		}
	}
}
