using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.Extensions;
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
	public sealed partial class Page3 : Page
	{
		public Page3()
		{
			this.InitializeComponent();

			Theme.ItemsSource = Themes;
			Theme.SelectionChanged += (s, e) =>
			{
				Update();
			};

			Theme.SelectedItem = Themes[new Random().Next(Themes.Length)];
		}

		private void OnCommandClicked(object sender, RoutedEventArgs e)
		{
			new Windows.UI.Popups.MessageDialog("Click").ShowAsync();
		}

		// TODO: Make into XAML
		private static AppBarTheme[] Themes = new AppBarTheme[]
		{
			new AppBarTheme
			{
				Name = "Default",
			},

			new AppBarTheme
			{
				Name = "Transparent",
				Background = GetColorFromHex("#00000000"),
				Foreground = GetColorFromHex("#FFFFFF"),
				Tint = GetColorFromHex("#FFFFFF")
			},

			// IOS

			new AppBarTheme
			{
				Name = "Facebook",
				Background = GetColorFromHex("#3b5998"),
				Foreground = GetColorFromHex("#FFFFFF"),
				Tint = GetColorFromHex("#FFFFFF"),
			},
			new AppBarTheme
			{
				Name = "Vine",
				Background = GetColorFromHex("#00BF8F"),
				Foreground = GetColorFromHex("#FFFFFF"),
				Tint = GetColorFromHex("#FFFFFF"),
			},
			new AppBarTheme
			{
				Name = "Yelp",
				Background = GetColorFromHex("#FF0000"),
				Foreground = GetColorFromHex("#FFFFFF"),
				Tint = GetColorFromHex("#FFFFFF"),
			},
			new AppBarTheme
			{
				Name = "Mint",
				Background = GetColorFromHex("#0AC775"),
				Foreground = GetColorFromHex("#FFFFFF"),
				Tint = GetColorFromHex("#FFFFFF"),
			},
			new AppBarTheme
			{
				Name = "Notes",
				Background = GetColorFromHex("#F1F0F8"),
				Tint = GetColorFromHex("#E7BA07"),
			},
			new AppBarTheme
			{
				Name = "Slack",
				Background = GetColorFromHex("#52374f"),
				Foreground = GetColorFromHex("#FFFFFF"),
				Tint = GetColorFromHex("#FFFFFF"),
			},
			new AppBarTheme
			{
				Name = "Calendar",
				Background = GetColorFromHex("#F8F8F8"),
				Tint = GetColorFromHex("#FF3B30"),
			},

			// ANDROID

			new AppBarTheme
			{
				Name = "Keep",
				Background = GetColorFromHex("#FFBB00"),
				Foreground = GetColorFromHex("#FFFFFF"),
				Tint = GetColorFromHex("#FFFFFF"),
			},

			new AppBarTheme
			{
				Name = "Outlook",
				Background = GetColorFromHex("#0F73C3"),
				Foreground = GetColorFromHex("#FFFFFF"),
				Tint = GetColorFromHex("#FFFFFF"),
			},

			new AppBarTheme
			{
				Name = "Gmail",
				Background = GetColorFromHex("#DA4336"),
				Foreground = GetColorFromHex("#FFFFFF"),
				Tint = GetColorFromHex("#FFFFFF"),
			},

			new AppBarTheme
			{
				Name = "Evernote",
				Background = GetColorFromHex("#3CC166"),
				Foreground = GetColorFromHex("#FFFFFF"),
				Tint = GetColorFromHex("#88000000"), // TODO
			},
		};

		private void Update()
		{
			if (Theme.SelectedItem == null)
			{
				return;
			}

			var appBarTheme = Theme.SelectedItem as AppBarTheme;
			MyAppBar.Content = appBarTheme.Name;
			MyAppBar.Foreground = appBarTheme.Foreground;
			MyAppBar.Background = appBarTheme.Background;

			MyAppBar.PrimaryCommands.OfType<Control>().ForEach(fe =>
			{
				fe.Foreground = appBarTheme.Tint;
				fe.Background = appBarTheme.Tint;
			});
		}

		public static SolidColorBrush GetColorFromHex(string hexString)
		{
			if (!System.Text.RegularExpressions.Regex.IsMatch(hexString, @"[#]([0-9]|[a-f]|[A-F]){6}\b") &&
				!System.Text.RegularExpressions.Regex.IsMatch(hexString, @"[#]([0-9]|[a-f]|[A-F]){8}\b"))
				throw new ArgumentException();

			//remove the # at the front
			hexString = hexString.Replace("#", "");

			byte a = 255;
			byte r = 255;
			byte g = 255;
			byte b = 255;

			int start = 0;

			//handle ARGB strings (8 characters long)
			if (hexString.Length == 8)
			{
				a = byte.Parse(hexString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
				start = 2;
			}

			//convert RGB characters to bytes
			r = byte.Parse(hexString.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
			g = byte.Parse(hexString.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
			b = byte.Parse(hexString.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

			return new SolidColorBrush(Color.FromArgb(a, r, g, b));
		}

		private class AppBarTheme
		{
			public string Name { get; set; }
			public SolidColorBrush Background { get; set; }
			public SolidColorBrush Foreground { get; set; }
			public SolidColorBrush Tint { get; set; }

			public override string ToString() => Name;
		}
	}
}
