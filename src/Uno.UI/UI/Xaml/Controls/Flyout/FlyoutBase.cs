using System;
using System.Collections.Generic;
using System.Text;
using Uno.Disposables;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Microsoft.Extensions.Logging;
using Uno.Extensions;
using Uno.Logging;
using Uno.UI;
#if XAMARIN_IOS
using View = UIKit.UIView;
#elif XAMARIN_ANDROID
using View = Android.Views.View;
#else
using View = Windows.UI.Xaml.UIElement;
#endif

namespace Windows.UI.Xaml.Controls.Primitives
{
	public partial class FlyoutBase : DependencyObject
	{
		public event EventHandler Opened;
		public event EventHandler Closed;
		public event EventHandler Opening;
		public event TypedEventHandler<FlyoutBase, FlyoutBaseClosingEventArgs> Closing;

		private bool _isOpen = false;

		private Windows.UI.Xaml.Controls.Popup _popup;

		public FlyoutBase()
		{
			_popup = new Windows.UI.Xaml.Controls.Popup
			{
				Child = CreatePresenter(),
				CustomLayouter = new FlyoutLayouter(this)
			};
			_popup.Closed += OnPopupClosed;
		}

		#region Placement DP
		/// <summary>
		/// Preferred placement of the flyout.
		/// </summary>
		/// <remarks>
		/// If there's not enough place, the following logic will be used:
		/// https://docs.microsoft.com/en-us/previous-versions/windows/apps/dn308515(v=win.10)#placing-a-flyout
		/// </remarks>
		public FlyoutPlacementMode Placement
		{
			get { return (FlyoutPlacementMode)GetValue(PlacementProperty); }
			set { SetValue(PlacementProperty, value); }
		}

		public static DependencyProperty PlacementProperty { get; } =
			DependencyProperty.Register(
				"Placement",
				typeof(FlyoutPlacementMode),
				typeof(FlyoutBase),
				new FrameworkPropertyMetadata(default(FlyoutPlacementMode))
			);

		#endregion

		#region Target DP - Readonly
		public static DependencyProperty TargetProperty { get; } = DependencyProperty.Register(
			"Target",
			typeof(FrameworkElement),
			typeof(FlyoutBase),
			new PropertyMetadata(default(FrameworkElement), OnTargetChanged));

		public FrameworkElement Target
		{
			get => (FrameworkElement)GetValue(TargetProperty);
			private set => SetValue(TargetProperty, value);
		}

		private static void OnTargetChanged(DependencyObject snd, DependencyPropertyChangedEventArgs args)
		{
			if (snd is FlyoutBase flyout)
			{
				flyout._popup.Anchor = args.NewValue as UIElement;
			}
		}
		#endregion

		public void Hide()
		{
			Hide(canCancel: true);
		}

		internal void Hide(bool canCancel)
		{
			if (!_isOpen)
			{
				return;
			}

			// As of UWP 17763, closing event is raised every times that the popup is being closed
			// (even in case of ShowAt(anotherPlacementTarget)
			var closing = new FlyoutBaseClosingEventArgs();
			Closing?.Invoke(this, closing);

			if (canCancel && closing.Cancel)
			{
				return;
			}

			Close();
			_isOpen = false;
			Closed?.Invoke(this, EventArgs.Empty);
		}

		public void ShowAt(FrameworkElement placementTarget)
		{
			if (_isOpen && placementTarget == Target)
			{
				return;
			}

			Target = placementTarget;

			if (_isOpen)
			{
				// UWP behavior: Close from previous placement target before opening at new one
				// Note: At this point 'Target' is already the new 'placementTarget', which is the behavior of UWP
				Hide(canCancel: false);
			}

			Opening?.Invoke(this, EventArgs.Empty);
			Open();
			_isOpen = true;
			Opened?.Invoke(this, EventArgs.Empty);
		}

		private void OnPopupClosed(object sender, object e)
		{
			// Safety: Make sure to mark the Flyout as closed if the popup was closed for any external reasons
			Hide(canCancel: false);
		}

		/// <summary>
		/// This is an overridable method to do something **On**Open, but is MUST NOT be invoked directly.
		/// You have to use the ShowAt() method
		/// </summary>
		protected internal virtual void Open()
		{
			_popup.IsOpen = true;
		}

		/// <summary>
		/// This is an overridable method to do something **On**Close, but is MUST NOT be invoked directly.
		/// You have to use the Hide() method
		/// </summary>
		protected internal virtual void Close()
		{
			_popup.IsOpen = false;
		}

		protected virtual Control CreatePresenter()
		{
			return null;
		}

		partial void OnDataContextChangedPartial(DependencyPropertyChangedEventArgs e)
		{
			// This is present to force the dataContext to be passed to the popup of the flyout since it is not directly a child in the visual tree of the flyout. 
			_popup?.SetValue(Popup.DataContextProperty, this.DataContext, precedence: DependencyPropertyValuePrecedences.Local);
		}

		partial void OnTemplatedParentChangedPartial(DependencyPropertyChangedEventArgs e)
		{
			_popup?.SetValue(Popup.TemplatedParentProperty, TemplatedParent, precedence: DependencyPropertyValuePrecedences.Local);
		}

		public static FlyoutBase GetAttachedFlyout(FrameworkElement element)
		{
			return (FlyoutBase)element.GetValue(AttachedFlyoutProperty);
		}

		public static void SetAttachedFlyout(FrameworkElement element, FlyoutBase value)
		{
			element.SetValue(AttachedFlyoutProperty, value);
		}

		public static void ShowAttachedFlyout(FrameworkElement flyoutOwner)
		{
			var flyout = GetAttachedFlyout(flyoutOwner);
			flyout?.ShowAt(flyoutOwner);
		}

		private class FlyoutLayouter : PopupBase.IDynamicPopupLayouter
		{
			/// <summary>
			/// This value is an arbitrary value for the placement of a popup below its anchor.
			/// </summary>
			private const int PopupPlacementTargetMargin = 5;

			private static readonly Dictionary<FlyoutPlacementMode, Memory<FlyoutPlacementMode>> PlacementsToTry =
				new Dictionary<FlyoutPlacementMode, Memory<FlyoutPlacementMode>>()
				{
					{FlyoutPlacementMode.Top, new []
					{
						FlyoutPlacementMode.Top,
						FlyoutPlacementMode.Bottom,
						FlyoutPlacementMode.Left,
						FlyoutPlacementMode.Right,
						FlyoutPlacementMode.Full // last resort placement
					}},
					{FlyoutPlacementMode.Bottom, new []
					{
						FlyoutPlacementMode.Bottom,
						FlyoutPlacementMode.Top,
						FlyoutPlacementMode.Left,
						FlyoutPlacementMode.Right,
						FlyoutPlacementMode.Full // last resort placement
					}},
					{FlyoutPlacementMode.Left, new []
					{
						FlyoutPlacementMode.Left,
						FlyoutPlacementMode.Right,
						FlyoutPlacementMode.Top,
						FlyoutPlacementMode.Bottom,
						FlyoutPlacementMode.Full // last resort placement
					}},
					{FlyoutPlacementMode.Right, new []
					{
						FlyoutPlacementMode.Right,
						FlyoutPlacementMode.Left,
						FlyoutPlacementMode.Top,
						FlyoutPlacementMode.Bottom,
						FlyoutPlacementMode.Full // last resort placement
					}},
				};

			private readonly FlyoutBase _flyout;

			public FlyoutLayouter(FlyoutBase flyout)
			{
				_flyout = flyout;
			}

			/// <inheritdoc />
			public Size Measure(Size available, Size visibleSize)
			{
				if (_flyout.Target == null || !(_flyout._popup.Child is FrameworkElement child))
				{
					return default;
				}

				child.Measure(visibleSize);

				return child.DesiredSize;
			}

			/// <inheritdoc />
			public void Arrange(Size finalSize, Rect visibleBounds, Size desiredSize)
			{
				var anchor = _flyout.Target;
				if (anchor == null || !(_flyout._popup.Child is FrameworkElement child))
				{
					return;
				}

				var anchorRect = anchor.GetAbsoluteBoundsRect();

				// Make sure the desiredSize fits in visibleBounds
				if (desiredSize.Width > visibleBounds.Width)
				{
					desiredSize.Width = visibleBounds.Width;
				}
				if (desiredSize.Height > visibleBounds.Height)
				{
					desiredSize.Height = visibleBounds.Height;
				}

				// Try all placements...
				var placementsToTry = PlacementsToTry.TryGetValue(_flyout.Placement, out var p)
					? p
					: new[] { _flyout.Placement };

				var halfAnchorWidth = anchorRect.Width / 2;
				var halfAnchorHeight = anchorRect.Height / 2;
				var halfChildWidth = desiredSize.Width / 2;
				var halfChildHeight = desiredSize.Height / 2;

				var finalRect = default(Rect);

				for (var i = 0; i < placementsToTry.Length; i++)
				{
					var placement = placementsToTry.Span[i];

					Point finalPosition;

					switch (placement)
					{
						case FlyoutPlacementMode.Top:
							finalPosition = new Point(
								x: anchorRect.Left + halfAnchorWidth - halfChildWidth,
								y: anchorRect.Top - PopupPlacementTargetMargin - desiredSize.Height);
							break;
						case FlyoutPlacementMode.Bottom:
							finalPosition = new Point(
								x: anchorRect.Left + halfAnchorWidth - halfChildWidth,
								y: anchorRect.Bottom + PopupPlacementTargetMargin);
							break;
						case FlyoutPlacementMode.Left:
							finalPosition = new Point(
								x: anchorRect.Left - PopupPlacementTargetMargin - desiredSize.Width,
								y: anchorRect.Top + halfAnchorHeight - halfChildHeight);
							break;
						case FlyoutPlacementMode.Right:
							finalPosition = new Point(
								x: anchorRect.Right + PopupPlacementTargetMargin,
								y: anchorRect.Top + halfAnchorHeight - halfChildHeight);
							break;
						default: // Full + other unsupported placements
							finalPosition = new Point(
								x: (visibleBounds.Width - desiredSize.Width) / 2.0,
								y: (visibleBounds.Height - desiredSize.Height) / 2.0);
							break;
					}

					finalRect = new Rect(finalPosition, desiredSize);

					if (RectHelper.Union(visibleBounds, finalRect).Equals(visibleBounds))
					{
						break; // this placement is acceptable
					}
				}

				Console.WriteLine($"************************ Layout flyout at {finalRect} (desired: {desiredSize} / available: {finalSize} / visible: {visibleBounds})");

				if (this.Log().IsEnabled(LogLevel.Debug))
				{
					this.Log().Debug($"Layout flyout at {finalRect} (desired: {desiredSize} / available: {finalSize} / visible: {visibleBounds})");
				}

				child.Arrange(finalRect);
			}
		}
	}
}
