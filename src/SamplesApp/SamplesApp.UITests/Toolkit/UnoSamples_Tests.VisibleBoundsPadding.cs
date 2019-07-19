using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Uno.UITest.Helpers;
using Uno.UITest.Helpers.Queries;
using Xamarin.UITest;

namespace SamplesApp.UITests.Toolkit
{
	[TestFixture]
	partial class UnoSamples_Tests : SampleControlUITestBase
	{
		[Test]
		public void VisibleBoundsPadding_Grid_Validation()
		{
			Run("UITests.Shared.Toolkit.VisibleBoundsPadding.VisibleBoundsPadding_Grid");

			_app.WaitForElement(_app.Marked("rootGrid"));

			var rootGrid = _app.Marked("rootGrid");

			var hideTitleBar = _app.Marked("HideTitleBarBtn");
			hideTitleBar.Tap();

			// Assert inital state 
			_app.Screenshot("VisibleBoundsPadding - Grid - 1 - Initial State");

			_app.SetOrientationLandscape();
			_app.Wait(new TimeSpan(0, 0, 1));

			// Assert after changing to Lanscape 
			_app.Screenshot("VisibleBoundsPadding - Grid - 2 - After SetOrientationLandscape");

			_app.SetOrientationPortrait();
			_app.Wait(new TimeSpan(0, 0, 1));

			// Assert after changing to Portrait 
			_app.Screenshot("VisibleBoundsPadding - Grid - 3 - After SetOrientationPortrait");

			_app.SetOrientationLandscape();
			_app.Wait(new TimeSpan(0, 0, 1));

			// Assert after changing to Portrait
			_app.Screenshot("VisibleBoundsPadding - Grid - 4 - After SetOrientationLandscape Again");

			_app.SetOrientationLandscape();
			_app.Wait(new TimeSpan(0, 0, 1));

			// Assert after changing to Portrait
			_app.Screenshot("VisibleBoundsPadding - Grid - 5 - After SetOrientationPortrait Again");
		}

		[Test]
		public void VisibleBoundsPadding_Border_Validation()
		{
			Run("UITests.Shared.Toolkit.VisibleBoundsPadding.VisibleBoundsPadding_Border");

			_app.WaitForElement(_app.Marked("rootBorder"));

			var rootBorder = _app.Marked("rootBorder");

			var hideTitleBar = _app.Marked("HideTitleBarBtn");
			hideTitleBar.Tap();

			// Assert inital state 
			_app.Screenshot("VisibleBoundsPadding - Border - 1 - Initial State");

			_app.SetOrientationLandscape();
			_app.Wait(new TimeSpan(0, 0, 1));

			// Assert after changing to Lanscape 
			_app.Screenshot("VisibleBoundsPadding - Border - 2 - After SetOrientationLandscape");

			_app.SetOrientationPortrait();
			_app.Wait(new TimeSpan(0, 0, 1));

			// Assert after changing to Portrait 
			_app.Screenshot("VisibleBoundsPadding - Border - 3 - After SetOrientationPortrait");

			_app.SetOrientationLandscape();
			_app.Wait(new TimeSpan(0, 0, 1));

			// Assert after changing to Portrait
			_app.Screenshot("VisibleBoundsPadding - Border - 4 - After SetOrientationLandscape Again");

			_app.SetOrientationLandscape(); _app.Wait(new TimeSpan(0, 0, 1));

			// Assert after changing to Portrait
			_app.Screenshot("VisibleBoundsPadding - Border - 5 - After SetOrientationPortrait Again");
		}

		[Test]
		public void VisibleBoundsPadding_ListView_Validation()
		{
			Run("UITests.Shared.Toolkit.VisibleBoundsPadding.VisibleBoundsPadding_ListView");

			_app.WaitForElement(_app.Marked("myListView"));

			var myListView = _app.Marked("myListView");
			var myListViewPosition = myListView.FirstResult().Rect;

			var hideTitleBar = _app.Marked("HideTitleBarBtn");
			hideTitleBar.Tap();

			// Assert inital state 
			_app.Screenshot("VisibleBoundsPadding - ListView - 1 - Initial State");

			// No Change in Orientation just scroll to bottom
			{
				// Scroll To bottom 
				_app.DragCoordinates(
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y + myListViewPosition.Height - 5,
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y
					);

				_app.DragCoordinates(
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y + myListViewPosition.Height - 5,
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y
					);

				// Assert after Scrolling to bottom of ListView
				_app.Screenshot("VisibleBoundsPadding - ListView - 2 - Max Scroll");
			}

			// SetOrientationLandscape
			{
				_app.SetOrientationLandscape();
				_app.Wait(new TimeSpan(0, 0, 1));

				// Scroll To bottom 
				_app.DragCoordinates(
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y + myListViewPosition.Height - 5,
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y
					);

				_app.DragCoordinates(
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y + myListViewPosition.Height - 5,
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y
					);

				// Assert after SetOrientationLandscape
				_app.Screenshot("VisibleBoundsPadding - ListView - 3 - SetOrientationLandscape");
			}

			// SetOrientationPortrait
			{
				_app.SetOrientationPortrait();
				_app.Wait(new TimeSpan(0, 0, 1));

				// Scroll To bottom 
				_app.DragCoordinates(
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y + myListViewPosition.Height - 5,
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y
					);

				_app.DragCoordinates(
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y + myListViewPosition.Height - 5,
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y
					);

				// Assert after SetOrientationPortrait
				_app.Screenshot("VisibleBoundsPadding - ListView - 4 - SetOrientationPortrait");
			}

			// SetOrientationLandscape (Again) 
			{

				_app.SetOrientationLandscape();
				_app.Wait(new TimeSpan(0, 0, 1));

				// Scroll To bottom 
				_app.DragCoordinates(
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y + myListViewPosition.Height - 5,
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y
					);

				_app.DragCoordinates(
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y + myListViewPosition.Height - 5,
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y
					);

				// Assert after SetOrientationLandscape (Again) 
				_app.Screenshot("VisibleBoundsPadding - ListView - 5 - SetOrientationLandscape Again");
			}

			// SetOrientationPortrait (Again)
			{
				_app.SetOrientationPortrait();
				_app.Wait(new TimeSpan(0, 0, 1));

				// Scroll To bottom 
				_app.DragCoordinates(
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y + myListViewPosition.Height - 5,
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y
					);

				_app.DragCoordinates(
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y + myListViewPosition.Height - 5,
					myListViewPosition.X + myListViewPosition.Width / 2,
					myListViewPosition.Y
					);

				// Assert after SetOrientationPortrait (Again)
				_app.Screenshot("VisibleBoundsPadding - ListView - 6 - SetOrientationPortrait Again");
			}
		}

		[Test]
		public void VisibleBoundsPadding_ScrollViewer_Validation()
		{
			Run("UITests.Shared.Toolkit.VisibleBoundsPadding.VisibleBoundsPadding_ScrollViewer");

			_app.WaitForElement(_app.Marked("myScrollViewer"));

			var myScrollViewer = _app.Marked("myScrollViewer");
			var myScrollViewerPosition = myScrollViewer.FirstResult().Rect;

			var hideTitleBar = _app.Marked("HideTitleBarBtn");
			hideTitleBar.Tap();

			// Assert inital state 
			_app.Screenshot("VisibleBoundsPadding - ScrollViewer - 1 - Initial State");

			// No Change in Orientation just scroll to bottom
			{
				// Scroll To bottom 
				_app.DragCoordinates(
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y + myScrollViewerPosition.Height - 5,
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y
					);

				_app.DragCoordinates(
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y + myScrollViewerPosition.Height - 5,
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y
					);

				// Assert after Max Scroll
				_app.Screenshot("VisibleBoundsPadding - ScrollViewer - 2 - Max Scroll");
			}

			// SetOrientationLandscape
			{
				_app.SetOrientationLandscape();
				_app.Wait(new TimeSpan(0, 0, 1));

				// Scroll To bottom 
				_app.DragCoordinates(
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y + myScrollViewerPosition.Height - 5,
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y
					);

				_app.DragCoordinates(
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y + myScrollViewerPosition.Height - 5,
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y
					);

				// Assert after SetOrientationLandscape
				_app.Screenshot("VisibleBoundsPadding - ScrollViewer - 3 - SetOrientationLandscape");
			}

			// SetOrientationPortrait
			{
				_app.SetOrientationPortrait();
				_app.Wait(new TimeSpan(0, 0, 1));

				// Scroll To bottom 
				_app.DragCoordinates(
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y + myScrollViewerPosition.Height - 5,
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y
					);

				_app.DragCoordinates(
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y + myScrollViewerPosition.Height - 5,
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y
					);

				// Assert after SetOrientationPortrait
				_app.Screenshot("VisibleBoundsPadding - ScrollViewer - 4 - SetOrientationPortrait");
			}

			// SetOrientationLandscape (Again)
			{
				_app.SetOrientationLandscape();
				_app.Wait(new TimeSpan(0, 0, 1));

				// Scroll To bottom 
				_app.DragCoordinates(
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y + myScrollViewerPosition.Height - 5,
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y
					);

				_app.DragCoordinates(
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y + myScrollViewerPosition.Height - 5,
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y
					);

				// Assert after SetOrientationLandscape (Again)
				_app.Screenshot("VisibleBoundsPadding - ScrollViewer - 5 - SetOrientationLandscape Again");
			}

			//  Change in Orientation just scroll to bottom
			{
				_app.SetOrientationPortrait();
				_app.Wait(new TimeSpan(0, 0, 1));

				// Scroll To bottom 
				_app.DragCoordinates(
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y + myScrollViewerPosition.Height - 5,
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y
					);

				_app.DragCoordinates(
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y + myScrollViewerPosition.Height - 5,
					myScrollViewerPosition.X + myScrollViewerPosition.Width / 2,
					myScrollViewerPosition.Y
					);

				// Assert after SetOrientationPortrait (Again)
				_app.Screenshot("VisibleBoundsPadding - ScrollViewer - 6 - SetOrientationPortrait Again");
			}
		}
	}
}
