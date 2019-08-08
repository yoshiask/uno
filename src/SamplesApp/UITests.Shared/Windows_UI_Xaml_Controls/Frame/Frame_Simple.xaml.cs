using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Uno.UI.Samples.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UITests.Shared.Windows_UI_Xaml_Controls.Frame
{
	[SampleControlInfo("Frame", "Frame_Simple")]
	public sealed partial class Frame_Simple : UserControl
	{
		public Frame_Simple()
		{
			this.InitializeComponent();

			BackStack.IsItemClickEnabled = true;

			MyFrame.Navigating += (s, e) =>
			{
				if (DisableNavigation.IsChecked ?? false)
				{
					e.Cancel = true;
				}
			};

			MyFrame.Navigated += (s, e) =>
			{
				Update();

				SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = MyFrame.CanGoBack
					? AppViewBackButtonVisibility.Visible
					: AppViewBackButtonVisibility.Collapsed;
			};

			SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) =>
			{
				if (MyFrame.CanGoBack)
				{
					MyFrame.GoBack();
					e.Handled = true;
				}
			};
		}

		private NavigationTransitionInfo GetTransitionInfo()
		{
			return SuppressTransition.IsChecked ?? false
				? new SuppressNavigationTransitionInfo()
				: null;
		}

		private void Page1(object sender, RoutedEventArgs e) => MyFrame.Navigate(typeof(Page1), null, GetTransitionInfo());

		private void Page2(object sender, RoutedEventArgs e) => MyFrame.Navigate(typeof(Page2), null, GetTransitionInfo());

		private void Page3(object sender, RoutedEventArgs e) => MyFrame.Navigate(typeof(Page3), null, GetTransitionInfo());

		private void Page4(object sender, RoutedEventArgs e) => MyFrame.Navigate(typeof(Page4), null, GetTransitionInfo());

		private void Page5(object sender, RoutedEventArgs e) => MyFrame.Navigate(typeof(Page5), null, GetTransitionInfo());

		private void Page6(object sender, RoutedEventArgs e) => MyFrame.Navigate(typeof(Page6), null, GetTransitionInfo());

		private void Back(object sender, RoutedEventArgs e) => MyFrame.GoBack(GetTransitionInfo());

		private void Forward(object sender, RoutedEventArgs e) => MyFrame.GoForward();

		private void DelayedNavigate(object sender, RoutedEventArgs e)
		{
			Task.Run(async () =>
			{
				await Task.Delay(5000);
				await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Page1(sender, e));
			});
		}

		private void ClearHistory(object sender, RoutedEventArgs e)
		{
			MyFrame.BackStack.Clear();
			MyFrame.ForwardStack.Clear();

			Update();
		}

		private void Update()
		{
			this.ForwardButton.IsEnabled = MyFrame.CanGoForward;
			this.BackButton.IsEnabled = MyFrame.CanGoBack;

			var history = new[]
			{
				MyFrame.BackStack.Select(x => x.SourcePageType),
				new [] { MyFrame.CurrentSourcePageType }, // TODO: set CurrentSourcePageType
                MyFrame.ForwardStack.Select(x => x.SourcePageType),
			}
			.SelectMany(x => x)
			.Select(x => x?.ToString().Split('.').LastOrDefault());

			BackStack.ItemsSource = history;

#if XAMARIN
            CoreDispatcherScheduler.MainNormal.Schedule(() =>
            {
                BackStack.SelectedIndex = MyFrame.BackStack.Count;
            });
#endif
		}
	}
}
