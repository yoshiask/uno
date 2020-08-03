#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Devices.Input;
using Windows.UI.Input;
using Windows.UI.Xaml.Input;
using Microsoft.Extensions.Logging;
using Uno.Extensions;
using Uno.Logging;
using Uno.Foundation.Extensibility;
using Windows.UI.Core;
using Windows.Foundation;
using System.Threading;
using System.Numerics;
using Uno.UI.DataBinding;

namespace Windows.UI.Xaml
{
	partial class UIElement
	{
		private class PointerManager
		{
			public PointerManager()
			{
				Window.Current.CoreWindow.PointerMoved += CoreWindow_PointerMoved;
				Window.Current.CoreWindow.PointerEntered += CoreWindow_PointerEntered;
				Window.Current.CoreWindow.PointerExited += CoreWindow_PointerExited;
				Window.Current.CoreWindow.PointerPressed += CoreWindow_PointerPressed;
				Window.Current.CoreWindow.PointerReleased += CoreWindow_PointerReleased;
				Window.Current.CoreWindow.PointerWheelChanged += CoreWindow_PointerWheelChanged;
			}

			private void CoreWindow_PointerWheelChanged(CoreWindow sender, PointerEventArgs args)
			{
				if (this.Log().IsEnabled(LogLevel.Trace))
				{
					this.Log().Trace($"CoreWindow_PointerWheelChanged ({args.CurrentPoint.Position})");
				}

				PropagateEvent(args, e =>
				{
					var pointerArgs = new PointerRoutedEventArgs(args, e);

					e.OnNativePointerWheel(pointerArgs);
				});
			}

			private void CoreWindow_PointerEntered(CoreWindow sender, PointerEventArgs args)
			{
				if (this.Log().IsEnabled(LogLevel.Trace))
				{
					this.Log().Trace($"CoreWindow_PointerEntered ({args.CurrentPoint.Position})");
				}
			}

			private void CoreWindow_PointerExited(CoreWindow sender, PointerEventArgs args)
			{
				if (this.Log().IsEnabled(LogLevel.Trace))
				{
					this.Log().Trace($"CoreWindow_PointerExited ({args.CurrentPoint.Position})");
				}
			}

			private void CoreWindow_PointerReleased(CoreWindow sender, PointerEventArgs args)
			{
				if (FindOriginalSource(args) is { } originalSource)
				{
					if (this.Log().IsEnabled(LogLevel.Trace))
					{
						this.Log().Trace($"CoreWindow_PointerReleased [{originalSource}/{originalSource.GetHashCode():X8}");
					}

					var routedArgs = new PointerRoutedEventArgs(args, originalSource);

					if (UIElement.PointerCapture.TryGet(routedArgs.Pointer, out var capture))
					{
						foreach (var target in capture.Targets.ToArray())
						{
							target.Element.OnNativePointerUp(routedArgs);
						}
					}
					else
					{
						originalSource.OnNativePointerUp(routedArgs);
					}
				}
				else if (this.Log().IsEnabled(LogLevel.Trace))
				{
					this.Log().Trace($"CoreWindow_PointerReleased ({args.CurrentPoint.Position}) **undispatched**");
				}
			}

			private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs args)
			{
				var source = FindOriginalSource(args, _pressedCache, isStale: _isPressed);
				if (source.element is null)
				{
					if (this.Log().IsEnabled(LogLevel.Trace))
					{
						this.Log().Trace($"CoreWindow_PointerMoved ({args.CurrentPoint.Position}) **undispatched**");
					}

					return;
				}

				if (FindOriginalSource(args) is { } originalSource)
				{
					if (this.Log().IsEnabled(LogLevel.Trace))
					{
						this.Log().Trace($"CoreWindow_PointerPressed ({args.CurrentPoint.Position}) [{originalSource}/{originalSource.GetHashCode():X8}");
					}

					var routedArgs = new PointerRoutedEventArgs(args, originalSource);

					originalSource.OnNativePointerDown(routedArgs);
				}
				else if (this.Log().IsEnabled(LogLevel.Trace))
				{
					this.Log().Trace($"CoreWindow_PointerPressed ({args.CurrentPoint.Position}) **undispatched**");
				}
			}

			private void CoreWindow_PointerMoved(CoreWindow sender, PointerEventArgs args)
			{
				var source = FindOriginalSource(args, _overCache, isStale: _isOver);
				if (source.element is null)
				{
					if (this.Log().IsEnabled(LogLevel.Trace))
					{
						this.Log().Trace($"CoreWindow_PointerMoved ({args.CurrentPoint.Position}) **undispatched**");
					}

					return;
				}

				if (this.Log().IsEnabled(LogLevel.Trace))
				{
					this.Log().Trace($"CoreWindow_PointerMoved [{source.element}/{source.element.GetHashCode():X8}");
				}

				var routedArgs = new PointerRoutedEventArgs(args, source.element);

				if (source.staleBranch.HasValue)
				{
					routedArgs.CanBubbleNatively = true;
					var (root, stale) = source.staleBranch.Value;
					do
					{
						stale.SetOver(routedArgs, false);
						stale = stale.GetParent() as UIElement;
					} while (stale is { } && stale != root);
					routedArgs.CanBubbleNatively = false;
				}

				if (UIElement.PointerCapture.TryGet(routedArgs.Pointer, out var capture))
				{
					foreach (var target in capture.Targets.ToArray())
					{
						target.Element.OnNativePointerMove(routedArgs);
					}
				}
				else
				{
					source.element.OnNativePointerMove(routedArgs);
				}
			}

			// TODO: Use pointer ID for the predicates
			private static Predicate<UIElement> _isOver = e => e.IsPointerOver;
			private static Predicate<UIElement> _isPressed = e => e.IsPointerPressed;

			private Dictionary<uint, (Rect validity, ManagedWeakReference orginalSource)> _pressedCache = new Dictionary<uint, (Rect, ManagedWeakReference)>();
			private Dictionary<uint, (Rect validity, ManagedWeakReference orginalSource)> _overCache = new Dictionary<uint, (Rect, ManagedWeakReference)>();

			private (UIElement? element, (UIElement root, UIElement leaf)? staleBranch) FindOriginalSource(
				PointerEventArgs args,
				Dictionary<uint, (Rect validity, ManagedWeakReference orginalSource)> cache,
				Predicate<UIElement>? isStale = null)
			{
				var pointerId = args.CurrentPoint.PointerId;
				if (cache.TryGetValue(pointerId, out var cached)
					&& cached.validity.Contains(args.CurrentPoint.RawPosition)
					&& cached.orginalSource.Target is UIElement cachedOriginalSource
					&& cachedOriginalSource.IsLoaded)
				{
					var rootToCachedElement = UIElement.GetTransform(cachedOriginalSource, null);
					var positionInCachedElementCoordinates = rootToCachedElement.Transform(args.CurrentPoint.Position);

					var (originalSource, staleBranchRoot) = SearchUpAndDownForTopMostElementAt(
						positionInCachedElementCoordinates,
						cachedOriginalSource,
						isStale);

					if (originalSource is {})
					{
						UpdateCache(cache, pointerId, (cached.orginalSource, cachedOriginalSource), originalSource);
						return (originalSource, staleBranchRoot is null ? default : (staleBranchRoot, cachedOriginalSource));
					}

					// We walked all the tree up from the provided element, but were not able to find any target!
					// Maybe the cached element has been removed from the tree (but the IsLoaded should have been false :/)

					this.Log().Warn(
						"Enable to find any acceptable original source by walking up the tree from the cached element, "
						+ "which is suspicious as the element has not been flag as unloaded."
						+ "Trying now by looking down from the root.");
				}

				if (Window.Current.RootElement is UIElement root)
				{
					var (originalSource, staleBranchRoot) = SearchDownForTopMostElementAt(args.CurrentPoint.Position, root, isStale);
					if (staleBranchRoot is null)
					{
						UpdateCache(cache, pointerId, default, originalSource);
						return (originalSource, default);
					}
					else
					{
						var staleBranchLeaf = SearchDownForStaleLeaf(staleBranchRoot, isStale!);
						return (originalSource, (staleBranchRoot, staleBranchLeaf));
					}
				}

				this.Log().Warn("The root element not set yet, impossible to find the original source.");

				return default;
			}

			private void UpdateCache(
				Dictionary<uint, (Rect validity, ManagedWeakReference orginalSource)> cache,
				uint pointerId,
				(ManagedWeakReference weak, UIElement instance)? currentEntry,
				UIElement updated)
			{
				if (currentEntry.HasValue)
				{
					if (updated == currentEntry.Value.instance)
					{
						return;
					}

					WeakReferencePool.ReturnWeakReference(this, currentEntry.Value.weak);
				}

				cache[pointerId] = (
					validity: new Rect(new Point(), new Size(double.PositiveInfinity, double.PositiveInfinity)), // TODO
					orginalSource: WeakReferencePool.RentWeakReference(this, updated)
				);
			}

			private static (UIElement? element, UIElement? staleRoot) SearchUpAndDownForTopMostElementAt(
				Point position,
				UIElement element,
				Predicate<UIElement>? isStale = null)
			{
				var (foundElement, staleRoot) = SearchDownForTopMostElementAt(position, element, isStale);
				if (foundElement is { })
				{
					return (foundElement, staleRoot); // Success match
				}

				double offsetX = 0, offsetY = 0;
				while (element.TryGetParentUIElementForTransformToVisual(out var parent, ref offsetX, ref offsetY))
				{
					position.X += offsetX;
					position.Y += offsetY;

					if (staleRoot is null)
					{
						(foundElement, staleRoot) = SearchDownForTopMostElementAt(position, parent, isStale, excludedChild: element);
					}
					else
					{
						(foundElement, _) = SearchDownForTopMostElementAt(position, element);
						if (foundElement is null && (isStale?.Invoke(element) ?? false))
						{
							staleRoot = element;
						}
					}

					if (foundElement is { })
					{
						return (foundElement, staleRoot);
					}

					element = parent;
				}

				return default;
			}

			private static (UIElement? element, UIElement? staleRoot) SearchDownForTopMostElementAt(
				Point position,
				UIElement element,
				Predicate<UIElement>? isStale = null,
				UIElement? excludedChild = null)
			{
				// If the element is not hit testable, do not even try to validate it nor its children.
				if (element.IsHitTestVisibleCoalesced)
				{
					return (default, isStale?.Invoke(element) ?? false ? element : default);
				}

				var layoutSlot = element.LayoutSlotWithMarginsAndAlignments;
				var renderBounds = new Rect(new Point(), layoutSlot.Size);
				var clippingBounds = renderBounds; // TODO: Get the real clipping rect!

				// First compute the 'location' in the current element coordinate space
				position.X -= layoutSlot.X;
				position.Y -= layoutSlot.Y;
				var renderTransform = element.RenderTransform;
				if (renderTransform != null)
				{
					var parentToElement = renderTransform.MatrixCore;
					//Matrix3x2.Invert(parentToElement, out var currentToParent);

					position = parentToElement.Transform(position);
					renderBounds = parentToElement.Transform(renderBounds);
					clippingBounds = parentToElement.Transform(clippingBounds);
				}
				// TODO: IScroller

				// Then try to find nested element, but only if the position is in the clipping bounds!
				if (clippingBounds.Contains(position))
				{
					var staleRoot = default(UIElement?);

					// Validate if any child is an acceptable target
					var children = excludedChild is null ? element.GetChildren() : element.GetChildren().Except(excludedChild);
					using var child = children.Reverse().GetEnumerator();
					while (child.MoveNext())
					{
						var childResult = SearchDownForTopMostElementAt(position, child.Current, isStale);

						if (staleRoot is null && childResult.staleRoot is { })
						{
							Debug.Assert(childResult.staleRoot == child.Current);
							staleRoot = childResult.staleRoot;
							isStale = null; // Do not search in other stale children if we found a stale element
						}

						if (childResult.element is { })
						{
							if (staleRoot is null && isStale is { })
							{
								// If we didn't find any stale root in previous children or in the child's sub tree,
								// we continue to enumerate the children to detect a potential stale root

								while (child.MoveNext())
								{
									if (isStale(child.Current))
									{
										staleRoot = child.Current;
										break;
									}
								}
							}

							return (childResult.element, staleRoot);
						}
					}

					// We didn't find any child at the given position, validate the position is in actual bounds (which might be different than the clipping bounds)
					if (renderBounds.Contains(position))
					{
						return (element, staleRoot);
					}
				}

				// The element is invalid, still validate if it's not stale
				return (default, isStale?.Invoke(element) ?? false ? element : default);
			}

			private static UIElement SearchDownForStaleLeaf(UIElement element, Predicate<UIElement> isStale)
			{
				foreach (var child in element.GetChildren().Reverse())
				{
					if (isStale(child))
					{
						SearchDownForStaleLeaf(child, isStale);
					}
				}

				return element;
			}
		}

		// TODO Should be per CoreWindow
		private static PointerManager _pointerManager;

		partial void InitializePointersPartial()
		{
			if (_pointerManager == null)
			{
				_pointerManager = new PointerManager();
			}
		}

		partial void AddPointerHandler(RoutedEvent routedEvent, int handlersCount, object handler, bool handledEventsToo)
		{

		}


		#region HitTestVisibility
		internal void UpdateHitTest()
		{
			this.CoerceValue(HitTestVisibilityProperty);
		}

		private enum HitTestVisibility
		{
			/// <summary>
			/// The element and its children can't be targeted by hit-testing.
			/// </summary>
			/// <remarks>
			/// This occurs when IsHitTestVisible="False", IsEnabled="False", or Visibility="Collapsed".
			/// </remarks>
			Collapsed,

			/// <summary>
			/// The element can't be targeted by hit-testing.
			/// </summary>
			/// <remarks>
			/// This usually occurs if an element doesn't have a Background/Fill.
			/// </remarks>
			Invisible,

			/// <summary>
			/// The element can be targeted by hit-testing.
			/// </summary>
			Visible,
		}

		/// <summary>
		/// Represents the final calculated hit-test visibility of the element.
		/// </summary>
		/// <remarks>
		/// This property should never be directly set, and its value should always be calculated through coercion (see <see cref="CoerceHitTestVisibility(DependencyObject, object, bool)"/>.
		/// </remarks>
		private static readonly DependencyProperty HitTestVisibilityProperty =
			DependencyProperty.Register(
				"HitTestVisibility",
				typeof(HitTestVisibility),
				typeof(UIElement),
				new FrameworkPropertyMetadata(
					HitTestVisibility.Visible,
					FrameworkPropertyMetadataOptions.Inherits,
					coerceValueCallback: (s, e) => CoerceHitTestVisibility(s, e),
					propertyChangedCallback: (s, e) => OnHitTestVisibilityChanged(s, e)
				)
			);

		/// <summary>
		/// This calculates the final hit-test visibility of an element.
		/// </summary>
		/// <returns></returns>
		private static object CoerceHitTestVisibility(DependencyObject dependencyObject, object baseValue)
		{
			var element = (UIElement)dependencyObject;

			// The HitTestVisibilityProperty is never set directly. This means that baseValue is always the result of the parent's CoerceHitTestVisibility.
			var baseHitTestVisibility = (HitTestVisibility)baseValue;

			// If the parent is collapsed, we should be collapsed as well. This takes priority over everything else, even if we would be visible otherwise.
			if (baseHitTestVisibility == HitTestVisibility.Collapsed)
			{
				return HitTestVisibility.Collapsed;
			}

			// If we're not locally hit-test visible, visible, or enabled, we should be collapsed. Our children will be collapsed as well.
			if (!element.IsHitTestVisible || element.Visibility != Visibility.Visible || !element.IsEnabledOverride())
			{
				return HitTestVisibility.Collapsed;
			}

			// If we're not hit (usually means we don't have a Background/Fill), we're invisible. Our children will be visible or not, depending on their state.
			if (!element.IsViewHit())
			{
				return HitTestVisibility.Invisible;
			}

			// If we're not collapsed or invisible, we can be targeted by hit-testing. This means that we can be the source of pointer events.
			return HitTestVisibility.Visible;
		}

		private static void OnHitTestVisibilityChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
		}
		#endregion

	}
}
