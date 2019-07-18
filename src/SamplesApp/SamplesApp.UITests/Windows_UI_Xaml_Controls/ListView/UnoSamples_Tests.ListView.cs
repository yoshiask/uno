using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Uno.UITest.Helpers;
using Uno.UITest.Helpers.Queries;
using Xamarin.UITest;

namespace SamplesApp.UITests.Windows_UI_Xaml_Controls.ListView
{
	[TestFixture]
	partial class UnoSamples_Tests : SampleControlUITestBase
	{
		[Test]
		public void ListView_Selection_Validation()
		{
			Run("UITests.Shared.Windows_UI_Xaml_Controls.ListView.ListView_Selection_Automated");

			_app.WaitForElement(_app.Marked("Multi_SelectedItemsCount"));

			var singleSelectionListView = _app.Marked("SingleSelectionListView");
			var multiSelectionListView = _app.Marked("MultiSelectionListView");
			var multi_SelectedItemsCount = _app.Marked("Multi_SelectedItemsCount");

			var singleSelectionListViewPosition = singleSelectionListView.FirstResult().Rect;
			var multiSelectionListViewPosition = multiSelectionListView.FirstResult().Rect;

			// Assert inital state 
			Assert.AreEqual("-1", singleSelectionListView.GetDependencyPropertyValue("SelectedIndex")?.ToString());
			Assert.AreEqual("0", multi_SelectedItemsCount.GetDependencyPropertyValue("Text")?.ToString());

			{
				// Click SingleSelectionListView first item
				_app.TapCoordinates(singleSelectionListViewPosition.X + 20, singleSelectionListViewPosition.Y + 100);
				Assert.AreEqual("0", singleSelectionListView.GetDependencyPropertyValue("SelectedIndex")?.ToString());

				// Click SingleSelectionListView second item
				_app.TapCoordinates(singleSelectionListViewPosition.X + 20, singleSelectionListViewPosition.Y + 150);
				Assert.AreEqual("1", singleSelectionListView.GetDependencyPropertyValue("SelectedIndex")?.ToString());

				// Click SingleSelectionListView second item (again)
				_app.TapCoordinates(singleSelectionListViewPosition.X + 20, singleSelectionListViewPosition.Y + 150);
				Assert.AreEqual("1", singleSelectionListView.GetDependencyPropertyValue("SelectedIndex")?.ToString());
			}

			{
				// Take note here that we are checking the count of SelectedItems for MultiSelectionListView

				// Click MultiSelectionListView first item
				_app.TapCoordinates(multiSelectionListViewPosition.X + 20, multiSelectionListViewPosition.Y + 100);
				Assert.AreEqual("1", multi_SelectedItemsCount.GetDependencyPropertyValue("Text")?.ToString());

				// Click MultiSelectionListView second item
				_app.TapCoordinates(multiSelectionListViewPosition.X + 20, multiSelectionListViewPosition.Y + 150);
				Assert.AreEqual("2", multi_SelectedItemsCount.GetDependencyPropertyValue("Text")?.ToString());

				// Click MultiSelectionListView second item (again)
				_app.TapCoordinates(multiSelectionListViewPosition.X + 20, multiSelectionListViewPosition.Y + 150);
				Assert.AreEqual("1", multi_SelectedItemsCount.GetDependencyPropertyValue("Text")?.ToString());
			}
		}

		[Test]
		public void ListView_GroupedItems_Selection_Validation()
		{
			Run("UITests.Shared.Windows_UI_Xaml_Controls.ListView.ListView_GroupedItems_Selection_Automated");

			_app.WaitForElement(_app.Marked("Multi_SelectedItemsCount"));

			var singleSelectionListView = _app.Marked("SingleSelectionListView");
			var multiSelectionListView = _app.Marked("MultiSelectionListView");
			var multi_SelectedItemsCount = _app.Marked("Multi_SelectedItemsCount");

			var singleSelectionListViewPosition = singleSelectionListView.FirstResult().Rect;
			var multiSelectionListViewPosition = multiSelectionListView.FirstResult().Rect;

			//// Assert inital state 
			Assert.AreEqual("-1", singleSelectionListView.GetDependencyPropertyValue("SelectedIndex")?.ToString());
			Assert.AreEqual("0", multi_SelectedItemsCount.GetDependencyPropertyValue("Text")?.ToString());

			{
				// Click SingleSelectionListView first item
				_app.TapCoordinates(singleSelectionListViewPosition.X + 20, singleSelectionListViewPosition.Y + 100);
				Assert.AreEqual("0", singleSelectionListView.GetDependencyPropertyValue("SelectedIndex")?.ToString());

				// Click SingleSelectionListView second item
				_app.TapCoordinates(singleSelectionListViewPosition.X + 20, singleSelectionListViewPosition.Y + 200);
				Assert.AreEqual("1", singleSelectionListView.GetDependencyPropertyValue("SelectedIndex")?.ToString());

				// Click SingleSelectionListView item in second groupe
				_app.TapCoordinates(singleSelectionListViewPosition.X + 20, singleSelectionListViewPosition.Y + 475);
				Assert.AreEqual("3", singleSelectionListView.GetDependencyPropertyValue("SelectedIndex")?.ToString());

				// Click SingleSelectionListView item in second groupe (again)
				_app.TapCoordinates(singleSelectionListViewPosition.X + 20, singleSelectionListViewPosition.Y + 475);
				Assert.AreEqual("3", singleSelectionListView.GetDependencyPropertyValue("SelectedIndex")?.ToString());
			}

			{
				// Take note here that we are checking the count of SelectedItems for MultiSelectionListView

				// Click MultiSelectionListView first item
				_app.TapCoordinates(multiSelectionListViewPosition.X + 20, multiSelectionListViewPosition.Y + 100);
				Assert.AreEqual("1", multi_SelectedItemsCount.GetDependencyPropertyValue("Text")?.ToString());

				// Click MultiSelectionListView second item
				_app.TapCoordinates(multiSelectionListViewPosition.X + 20, multiSelectionListViewPosition.Y + 200);
				Assert.AreEqual("2", multi_SelectedItemsCount.GetDependencyPropertyValue("Text")?.ToString());

				// Click MultiSelectionListView fourth item (first item of second group)
				_app.TapCoordinates(multiSelectionListViewPosition.X + 20, multiSelectionListViewPosition.Y + 475);
				Assert.AreEqual("3", multi_SelectedItemsCount.GetDependencyPropertyValue("Text")?.ToString());

				// Click MultiSelectionListView second item (again)
				_app.TapCoordinates(multiSelectionListViewPosition.X + 20, multiSelectionListViewPosition.Y + 200);
				Assert.AreEqual("2", multi_SelectedItemsCount.GetDependencyPropertyValue("Text")?.ToString());
			}
		}

		[Test]
		public void ListView_Scrolling_Validation()
		{
			Run("UITests.Shared.Windows_UI_Xaml_Controls.ListView.ListView_Scrolling_Vertical_Automated");

			_app.WaitForElement(_app.Marked("ListViewNoGroupedItem"));

			var listView = _app.Marked("ListViewNoGroupedItem");
			var listViewPosition = listView.FirstResult().Rect;

			// Assert inital state 
			_app.Screenshot("ListView Scrolling - Vertical Orientation - 1 - InitialState");
			Assert.AreEqual("-1", listView.GetDependencyPropertyValue("SelectedIndex")?.ToString());

			// Scroll To bottom 
			{
				_app.DragCoordinates(
					listViewPosition.X + listViewPosition.Width / 2,
					listViewPosition.Y + listViewPosition.Height - 5,
					listViewPosition.X + listViewPosition.Width / 2,
					listViewPosition.Y
					);

				_app.DragCoordinates(
					listViewPosition.X + listViewPosition.Width / 2,
					listViewPosition.Y + listViewPosition.Height - 5,
					listViewPosition.X + listViewPosition.Width / 2,
					listViewPosition.Y
					);

				_app.Screenshot("ListView Scrolling - Vertical Orientation - 2 - Max Scroll");
			}

			// Make sure SelectedIndex did not change during the scrolling
			Assert.AreEqual("-1", listView.GetDependencyPropertyValue("SelectedIndex")?.ToString());

			// Scroll To Top 
			{
				_app.DragCoordinates(
					listViewPosition.X + listViewPosition.Width / 2,
					listViewPosition.Y + 5,
					listViewPosition.X + listViewPosition.Width / 2,
					listViewPosition.Y + listViewPosition.Height - 5
					);

				_app.DragCoordinates(
					listViewPosition.X + listViewPosition.Width / 2,
					listViewPosition.Y + 5,
					listViewPosition.X + listViewPosition.Width / 2,
					listViewPosition.Y + listViewPosition.Height - 5
					);

				_app.Screenshot("ListView Scrolling - Vertical Orientation - 3 - Back to Top");
			}

			// Make sure SelectedIndex did not change during the scrolling
			Assert.AreEqual("-1", listView.GetDependencyPropertyValue("SelectedIndex")?.ToString());
		}

		[Test]
		public void ListView_GroupedItems_Scrolling_Validation()
		{
			Run("UITests.Shared.Windows_UI_Xaml_Controls.ListView.ListView_Scrolling_Vertical_Automated");

			_app.WaitForElement(_app.Marked("ListViewWithGroupedItem"));

			var listView = _app.Marked("ListViewWithGroupedItem");
			var listViewPosition = listView.FirstResult().Rect;

			// Assert inital state 
			_app.Screenshot("ListView Scrolling - Vertical Orientation - Grouped Items - 1 - InitialState");
			Assert.AreEqual("-1", listView.GetDependencyPropertyValue("SelectedIndex")?.ToString());

			// Scroll To bottom 
			{
				_app.DragCoordinates(
					listViewPosition.X + listViewPosition.Width / 2,
					listViewPosition.Y + listViewPosition.Height - 5,
					listViewPosition.X + listViewPosition.Width / 2,
					listViewPosition.Y
					);

				_app.DragCoordinates(
					listViewPosition.X + listViewPosition.Width / 2,
					listViewPosition.Y + listViewPosition.Height - 5,
					listViewPosition.X + listViewPosition.Width / 2,
					listViewPosition.Y
					);

				_app.Screenshot("ListView Scrolling - Vertical Orientation - Grouped Items - 2 - Max Scroll");
			}

			// Make sure SelectedIndex did not change during the scrolling
			Assert.AreEqual("-1", listView.GetDependencyPropertyValue("SelectedIndex")?.ToString());

			// Scroll To Top 
			{
				_app.DragCoordinates(
					listViewPosition.X + listViewPosition.Width / 2,
					listViewPosition.Y + 5,
					listViewPosition.X + listViewPosition.Width / 2,
					listViewPosition.Y + listViewPosition.Height - 5
					);

				_app.DragCoordinates(
					listViewPosition.X + listViewPosition.Width / 2,
					listViewPosition.Y + 5,
					listViewPosition.X + listViewPosition.Width / 2,
					listViewPosition.Y + listViewPosition.Height - 5
					);

				_app.Screenshot("ListView Scrolling - Vertical Orientation - Grouped Items - 3 - Back to Top");
			}

			// Make sure SelectedIndex did not change during the scrolling
			Assert.AreEqual("-1", listView.GetDependencyPropertyValue("SelectedIndex")?.ToString());
		}






		[Test]
		public void ListView_HorizontalScrolling_Validation()
		{
			Run("UITests.Shared.Windows_UI_Xaml_Controls.ListView.ListView_Scrolling_Horizontal_Automated");

			_app.WaitForElement(_app.Marked("ListViewNoGroupedItem"));

			var listView = _app.Marked("ListViewNoGroupedItem");
			var listViewPosition = listView.FirstResult().Rect;

			// Assert inital state 
			_app.Screenshot("ListView Scrolling - Horizontal Orientation - 1 - InitialState");
			Assert.AreEqual("-1", listView.GetDependencyPropertyValue("SelectedIndex")?.ToString());

			// Scroll To Right 
			{
				_app.DragCoordinates(
					listViewPosition.X + listViewPosition.Width - 5,
					listViewPosition.Y + listViewPosition.Height / 2,
					listViewPosition.X,
					listViewPosition.Y + listViewPosition.Height / 2
					);

				_app.DragCoordinates(
					listViewPosition.X + listViewPosition.Width - 5,
					listViewPosition.Y + listViewPosition.Height / 2,
					listViewPosition.X,
					listViewPosition.Y + listViewPosition.Height / 2
					);

				_app.Screenshot("ListView Scrolling - Horizontal Orientation - 2 - Max Scroll");
			}

			// Make sure SelectedIndex did not change during the scrolling
			Assert.AreEqual("-1", listView.GetDependencyPropertyValue("SelectedIndex")?.ToString());

			// Scroll To Left 
			{
				_app.DragCoordinates(
					listViewPosition.X + 5,
					listViewPosition.Y + listViewPosition.Height / 2,
					listViewPosition.X + listViewPosition.Width,
					listViewPosition.Y + listViewPosition.Height / 2
					);

				_app.DragCoordinates(
					listViewPosition.X + 5,
					listViewPosition.Y + listViewPosition.Height / 2,
					listViewPosition.X + listViewPosition.Width,
					listViewPosition.Y + listViewPosition.Height / 2
					);

				_app.Screenshot("ListView Scrolling - Horizontal Orientation - 3 - Back to Top");
			}

			// Make sure SelectedIndex did not change during the scrolling
			Assert.AreEqual("-1", listView.GetDependencyPropertyValue("SelectedIndex")?.ToString());
		}

		[Test]
		public void ListView_GroupedItems_HorizontalScrolling_Validation()
		{
			Run("UITests.Shared.Windows_UI_Xaml_Controls.ListView.ListView_Scrolling_Horizontal_Automated");

			_app.WaitForElement(_app.Marked("ListViewWithGroupedItem"));

			var listView = _app.Marked("ListViewWithGroupedItem");
			var listViewPosition = listView.FirstResult().Rect;

			// Assert inital state 
			_app.Screenshot("ListView Scrolling - Horizontal Orientation - Grouped Items - 1 - InitialState");
			Assert.AreEqual("-1", listView.GetDependencyPropertyValue("SelectedIndex")?.ToString());

			// Scroll To Right 
			{
				_app.DragCoordinates(
					listViewPosition.X + listViewPosition.Width - 5,
					listViewPosition.Y + listViewPosition.Height / 2,
					listViewPosition.X,
					listViewPosition.Y + listViewPosition.Height / 2
					);

				_app.DragCoordinates(
					listViewPosition.X + listViewPosition.Width - 5,
					listViewPosition.Y + listViewPosition.Height / 2,
					listViewPosition.X,
					listViewPosition.Y + listViewPosition.Height / 2
					);

				_app.DragCoordinates(
					listViewPosition.X + listViewPosition.Width - 5,
					listViewPosition.Y + listViewPosition.Height / 2,
					listViewPosition.X,
					listViewPosition.Y + listViewPosition.Height / 2
					);

				_app.Screenshot("ListView Scrolling - Horizontal Orientation - Grouped Items - 2 - Max Scroll");
			}

			// Make sure SelectedIndex did not change during the scrolling
			Assert.AreEqual("-1", listView.GetDependencyPropertyValue("SelectedIndex")?.ToString());

			// Scroll To Left 
			{
				_app.DragCoordinates(
					listViewPosition.X + 5,
					listViewPosition.Y + listViewPosition.Height / 2,
					listViewPosition.X + listViewPosition.Width,
					listViewPosition.Y + listViewPosition.Height / 2
					);

				_app.DragCoordinates(
					listViewPosition.X + 5,
					listViewPosition.Y + listViewPosition.Height / 2,
					listViewPosition.X + listViewPosition.Width,
					listViewPosition.Y + listViewPosition.Height / 2
					);

				_app.DragCoordinates(
					listViewPosition.X + 5,
					listViewPosition.Y + listViewPosition.Height / 2,
					listViewPosition.X + listViewPosition.Width,
					listViewPosition.Y + listViewPosition.Height / 2
					);

				_app.Screenshot("ListView Scrolling - Horizontal Orientation - Grouped Items - 3 - Back to Top");
			}

			// Make sure SelectedIndex did not change during the scrolling
			Assert.AreEqual("-1", listView.GetDependencyPropertyValue("SelectedIndex")?.ToString());
		}

		[Test]
		public void ListView_Scrolling_ShortList_Validation()
		{
			Run("UITests.Shared.Windows_UI_Xaml_Controls.ListView.ListView_ShortLists_Automated");

			_app.WaitForElement(_app.Marked("ListViewNoGroupedItemHorizontal"));

			var listViewWithGroupedItemVertical = _app.Marked("ListViewWithGroupedItemVertical");
			var listViewWithGroupedItemVerticalPosition = listViewWithGroupedItemVertical.FirstResult().Rect;

			var listViewNoGroupedItemVertical = _app.Marked("ListViewNoGroupedItemVertical");
			var listViewNoGroupedItemVerticalPosition = listViewNoGroupedItemVertical.FirstResult().Rect;

			// Assert inital state 
			_app.Screenshot("ListView Scrolling - ShortLists - 1 - InitialState");
			Assert.AreEqual("-1", listViewWithGroupedItemVertical.GetDependencyPropertyValue("SelectedIndex")?.ToString());
			Assert.AreEqual("-1", listViewNoGroupedItemVertical.GetDependencyPropertyValue("SelectedIndex")?.ToString());

			// 1. Validating ListView with GroupedItem and vertical orientation
			{
				// Scroll to bottom 
				{
					_app.DragCoordinates(
						listViewWithGroupedItemVerticalPosition.X + listViewWithGroupedItemVerticalPosition.Width / 2,
						listViewWithGroupedItemVerticalPosition.Y + listViewWithGroupedItemVerticalPosition.Height - 5,
						listViewWithGroupedItemVerticalPosition.X + listViewWithGroupedItemVerticalPosition.Width / 2,
						listViewWithGroupedItemVerticalPosition.Y
						);

					_app.Screenshot("ListView Scrolling - ShortLists - GroupedItem - Vertical Orientation - 2 - Max Scroll");

					// Make sure SelectedIndex did not change during the scrolling
					Assert.AreEqual("-1", listViewWithGroupedItemVertical.GetDependencyPropertyValue("SelectedIndex")?.ToString());
					Assert.AreEqual("-1", listViewNoGroupedItemVertical.GetDependencyPropertyValue("SelectedIndex")?.ToString());
				}

				// Scroll to top 
				{
					_app.DragCoordinates(
						listViewWithGroupedItemVerticalPosition.X + listViewWithGroupedItemVerticalPosition.Width / 2,
						listViewWithGroupedItemVerticalPosition.Y + 5,
						listViewWithGroupedItemVerticalPosition.X + listViewWithGroupedItemVerticalPosition.Width / 2,
						listViewWithGroupedItemVerticalPosition.Y + listViewWithGroupedItemVerticalPosition.Height - 5
						);

					_app.Screenshot("ListView Scrolling - ShortLists - GroupedItem - Vertical Orientation - 2 - Back to Top");

					// Make sure SelectedIndex did not change during the scrolling
					Assert.AreEqual("-1", listViewWithGroupedItemVertical.GetDependencyPropertyValue("SelectedIndex")?.ToString());
					Assert.AreEqual("-1", listViewNoGroupedItemVertical.GetDependencyPropertyValue("SelectedIndex")?.ToString());
				}
			}

			// 2. Validating ListView with no GroupedItem and vertical orientation
			{
				// Scroll to bottom 
				{
					_app.DragCoordinates(
						listViewNoGroupedItemVerticalPosition.X + listViewNoGroupedItemVerticalPosition.Width / 2,
						listViewNoGroupedItemVerticalPosition.Y + listViewNoGroupedItemVerticalPosition.Height - 5,
						listViewNoGroupedItemVerticalPosition.X + listViewNoGroupedItemVerticalPosition.Width / 2,
						listViewNoGroupedItemVerticalPosition.Y
						);

					_app.Screenshot("ListView Scrolling - ShortLists - No GroupedItem - Vertical Orientation - 4 - Max Scroll");

					// Make sure SelectedIndex did not change during the scrolling
					Assert.AreEqual("-1", listViewWithGroupedItemVertical.GetDependencyPropertyValue("SelectedIndex")?.ToString());
					Assert.AreEqual("-1", listViewNoGroupedItemVertical.GetDependencyPropertyValue("SelectedIndex")?.ToString());
				}

				// Scroll to top 
				{
					_app.DragCoordinates(
						listViewNoGroupedItemVerticalPosition.X + listViewNoGroupedItemVerticalPosition.Width / 2,
						listViewNoGroupedItemVerticalPosition.Y + 5,
						listViewNoGroupedItemVerticalPosition.X + listViewNoGroupedItemVerticalPosition.Width / 2,
						listViewNoGroupedItemVerticalPosition.Y + listViewNoGroupedItemVerticalPosition.Height - 5
						);

					_app.Screenshot("ListView Scrolling - ShortLists - No GroupedItem - Vertical Orientation - 5 - Back to Top");

					// Make sure SelectedIndex did not change during the scrolling
					Assert.AreEqual("-1", listViewWithGroupedItemVertical.GetDependencyPropertyValue("SelectedIndex")?.ToString());
					Assert.AreEqual("-1", listViewNoGroupedItemVertical.GetDependencyPropertyValue("SelectedIndex")?.ToString());
				}
			}
		}

		[Test]
		public void ListView_HorizontalScrolling_ShortList_Validation()
		{
			Run("UITests.Shared.Windows_UI_Xaml_Controls.ListView.ListView_ShortLists_Automated");

			_app.WaitForElement(_app.Marked("ListViewNoGroupedItemHorizontal"));

			var listViewWithGroupedItemHorizontal = _app.Marked("ListViewWithGroupedItemHorizontal");
			var listViewWithGroupedItemHorizontalPosition = listViewWithGroupedItemHorizontal.FirstResult().Rect;

			var listViewNoGroupedItemHorizontal = _app.Marked("ListViewNoGroupedItemHorizontal");
			var listViewNoGroupedItemHorizontalPosition = listViewNoGroupedItemHorizontal.FirstResult().Rect;

			// Assert inital state 
			_app.Screenshot("ListView Scrolling - ShortLists - 1 - InitialState");
			Assert.AreEqual("-1", listViewWithGroupedItemHorizontal.GetDependencyPropertyValue("SelectedIndex")?.ToString());
			Assert.AreEqual("-1", listViewNoGroupedItemHorizontal.GetDependencyPropertyValue("SelectedIndex")?.ToString());

			// 1. Validating ListView with GroupedItem and horizontal orientation
			{
				// Scroll to right 
				{
					_app.DragCoordinates(
						listViewWithGroupedItemHorizontalPosition.X + listViewWithGroupedItemHorizontalPosition.Width - 5,
						listViewWithGroupedItemHorizontalPosition.Y + listViewWithGroupedItemHorizontalPosition.Height / 2,
						listViewWithGroupedItemHorizontalPosition.X,
						listViewWithGroupedItemHorizontalPosition.Y + listViewWithGroupedItemHorizontalPosition.Height / 2
						);

					_app.Screenshot("ListView Scrolling - ShortLists - GroupedItem - Horizontal Orientation - 2 - Max Scroll");

					// Make sure SelectedIndex did not change during the scrolling
					Assert.AreEqual("-1", listViewWithGroupedItemHorizontal.GetDependencyPropertyValue("SelectedIndex")?.ToString());
					Assert.AreEqual("-1", listViewNoGroupedItemHorizontal.GetDependencyPropertyValue("SelectedIndex")?.ToString());
				}

				// Scroll to left 
				{
					_app.DragCoordinates(
						listViewWithGroupedItemHorizontalPosition.X + 5,
						listViewWithGroupedItemHorizontalPosition.Y + listViewWithGroupedItemHorizontalPosition.Height / 2,
						listViewWithGroupedItemHorizontalPosition.X + listViewWithGroupedItemHorizontalPosition.Width,
						listViewWithGroupedItemHorizontalPosition.Y + listViewWithGroupedItemHorizontalPosition.Height / 2
						);

					_app.Screenshot("ListView Scrolling - ShortLists - GroupedItem - Horizontal Orientation - 3 - Back to Left");

					// Make sure SelectedIndex did not change during the scrolling
					Assert.AreEqual("-1", listViewWithGroupedItemHorizontal.GetDependencyPropertyValue("SelectedIndex")?.ToString());
					Assert.AreEqual("-1", listViewNoGroupedItemHorizontal.GetDependencyPropertyValue("SelectedIndex")?.ToString());
				}
			}

			// 2. Validating ListView with no GroupedItem and horizontal orientation
			{
				// Scroll to right 
				{
					_app.DragCoordinates(
						listViewNoGroupedItemHorizontalPosition.X + listViewNoGroupedItemHorizontalPosition.Width - 5,
						listViewNoGroupedItemHorizontalPosition.Y + listViewNoGroupedItemHorizontalPosition.Height / 2,
						listViewNoGroupedItemHorizontalPosition.X,
						listViewNoGroupedItemHorizontalPosition.Y + listViewNoGroupedItemHorizontalPosition.Height / 2
						);

					_app.Screenshot("ListView Scrolling - ShortLists - No GroupedItem - Horizontal Orientation - 4 - Max Scroll");

					// Make sure SelectedIndex did not change during the scrolling
					Assert.AreEqual("-1", listViewWithGroupedItemHorizontal.GetDependencyPropertyValue("SelectedIndex")?.ToString());
					Assert.AreEqual("-1", listViewNoGroupedItemHorizontal.GetDependencyPropertyValue("SelectedIndex")?.ToString());
				}

				// Scroll to left 
				{
					_app.DragCoordinates(
						listViewNoGroupedItemHorizontalPosition.X + 5,
						listViewNoGroupedItemHorizontalPosition.Y + listViewNoGroupedItemHorizontalPosition.Height / 2,
						listViewNoGroupedItemHorizontalPosition.X + listViewNoGroupedItemHorizontalPosition.Width,
						listViewNoGroupedItemHorizontalPosition.Y + listViewNoGroupedItemHorizontalPosition.Height / 2
						);

					_app.Screenshot("ListView Scrolling - ShortLists - No GroupedItem - Horizontal Orientation - 5 - Back to Left");

					// Make sure SelectedIndex did not change during the scrolling
					Assert.AreEqual("-1", listViewWithGroupedItemHorizontal.GetDependencyPropertyValue("SelectedIndex")?.ToString());
					Assert.AreEqual("-1", listViewNoGroupedItemHorizontal.GetDependencyPropertyValue("SelectedIndex")?.ToString());
				}
			}
		}
	}
}
