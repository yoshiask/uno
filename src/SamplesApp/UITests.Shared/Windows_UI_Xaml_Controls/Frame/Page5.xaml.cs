using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.UI.Toolkit;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UITests.Shared.Windows_UI_Xaml_Controls.Frame
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Page5 : Page
	{
		public Page5()
		{
			this.InitializeComponent();
		}

		private void ChangeForeground(object sender, RoutedEventArgs e)
		{
			CommandBarExtensions.SetBackButtonForeground(MyCommandBar, (Brush)PickRandomly(new object[]
			{
				new SolidColorBrush(Colors.Red),
				new SolidColorBrush(Colors.Blue),
				null,
			}));
		}

		private void ChangeVisibility(object sender, RoutedEventArgs e)
		{
			CommandBarExtensions.SetBackButtonVisibility(MyCommandBar, (Visibility)PickRandomly(new object[]
			{
				Visibility.Visible,
				Visibility.Collapsed,
			}));
		}

		private void ChangeIcon(object sender, RoutedEventArgs e)
		{
			CommandBarExtensions.SetBackButtonIcon(MyCommandBar, (BitmapIcon)PickRandomly(new object[]
			{
				new BitmapIcon { UriSource = new Uri("ms-appx:///Assets/Icons/menu.png") },
				new BitmapIcon { UriSource = new Uri("ms-appx:///Assets/Icons/search.png") },
				null,
			}));
		}

		private void ChangeTitle(object sender, RoutedEventArgs e)
		{
			var backButtonTitle = (string)PickRandomly(new object[]
			{
				"",
				"Title",
				null,
			});

			MyCommandBar.Content = backButtonTitle;
			CommandBarExtensions.SetBackButtonTitle(MyCommandBar, backButtonTitle);
		}

		private static object PickRandomly(object[] options)
		{
			return options[new Random().Next(options.Length)];
		}
	}
}
