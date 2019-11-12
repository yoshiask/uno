using System;
using System.Globalization;
using System.Threading;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Input;
using Uno;
using Uno.Extensions;
using Uno.Foundation;

namespace Windows.UI.Xaml.Input
{
	partial class PointerRoutedEventArgs
	{
		private readonly double _timestamp;
		private readonly Point _absolutePosition;
		private readonly VirtualKey _button;
		private readonly PointerUpdateKind _updateKind;

		internal PointerRoutedEventArgs(UIElement sender, UIElement.WindowManagerPointerEventArgs_Return nativeArgs)
		{
			_timestamp = nativeArgs.Timestamp;
			_absolutePosition = new Point(nativeArgs.RawX, nativeArgs.RawY);
			_button = nativeArgs.PressedButton == 0 ? VirtualKey.LeftButton
				: nativeArgs.PressedButton == 1 ? VirtualKey.MiddleButton
				: nativeArgs.PressedButton == 2 ? VirtualKey.RightButton
				: VirtualKey.None; // includes -1 == none

			var frameId = ToFrameId(_timestamp);
			var pointerId = (uint)nativeArgs.PointerId;
			var pointerType = (PointerDeviceType)nativeArgs.PointerType;

			var keyModifiers = VirtualKeyModifiers.None;
			if (nativeArgs.IsCtrlPressed) keyModifiers |= VirtualKeyModifiers.Control;
			if (nativeArgs.IsShiftPressed) keyModifiers |= VirtualKeyModifiers.Shift;

			bool isInContact, canBubbleNatively = true;
			switch ((UIElement.NativePointerEvent)nativeArgs.Event)
			{
				case UIElement.NativePointerEvent.PointerEnter:
				case UIElement.NativePointerEvent.PointerLeave:
					isInContact = false;
					canBubbleNatively = false;
					break;

				case UIElement.NativePointerEvent.PointerDown:
					_updateKind = _button == VirtualKey.LeftButton ? PointerUpdateKind.LeftButtonPressed
						: _button == VirtualKey.MiddleButton ? PointerUpdateKind.MiddleButtonPressed
						: _button == VirtualKey.RightButton ? PointerUpdateKind.RightButtonPressed
						: PointerUpdateKind.Other;
					isInContact = true;
					break;

				case UIElement.NativePointerEvent.PointerMove:
					isInContact = true;
					break;

				case UIElement.NativePointerEvent.PointerUp:
					_updateKind = _button == VirtualKey.LeftButton ? PointerUpdateKind.LeftButtonReleased
						: _button == VirtualKey.MiddleButton ? PointerUpdateKind.MiddleButtonReleased
						: _button == VirtualKey.RightButton ? PointerUpdateKind.RightButtonReleased
						: PointerUpdateKind.Other;
					isInContact = true;
					break;

				case UIElement.NativePointerEvent.PointerCancel:
					_updateKind = _button == VirtualKey.LeftButton ? PointerUpdateKind.LeftButtonReleased
						: _button == VirtualKey.MiddleButton ? PointerUpdateKind.MiddleButtonReleased
						: _button == VirtualKey.RightButton ? PointerUpdateKind.RightButtonReleased
						: PointerUpdateKind.Other;
					isInContact = false;
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(nativeArgs.Event));
			}

			var originalSource = UIElement.GetElementFromHandle(nativeArgs.OriginalSourceHandle) ?? sender;

			FrameId = frameId;
			Pointer = new Pointer(pointerId, pointerType, isInContact, isInRange: true);
			KeyModifiers = keyModifiers;
			OriginalSource = originalSource;
			CanBubbleNatively = canBubbleNatively;
		}

		internal PointerRoutedEventArgs(
			double timestamp,
			uint pointerId,
			PointerDeviceType pointerType,
			Point absolutePosition,
			bool isInContact,
			VirtualKey button,
			VirtualKeyModifiers keys,
			PointerUpdateKind updateKind,
			UIElement source,
			bool canBubbleNatively)
			: this()
		{
			_timestamp = timestamp;
			_absolutePosition = absolutePosition;
			_button = button;
			_updateKind = updateKind;

			FrameId = ToFrameId(timestamp);
			Pointer = new Pointer(pointerId, pointerType, isInContact, isInRange: true);
			KeyModifiers = keys;
			OriginalSource = source;
			CanBubbleNatively = canBubbleNatively;
		}

		public PointerPoint GetCurrentPoint(UIElement relativeTo)
		{
			var timestamp = ToTimeStamp(_timestamp);
			var device = PointerDevice.For(Pointer.PointerDeviceType);
			var rawPosition = _absolutePosition;
			var position = relativeTo == null
				? rawPosition
				: relativeTo.TransformToVisual(null).Inverse.TransformPoint(_absolutePosition);
			var properties = GetProperties();

			return new PointerPoint(FrameId, timestamp, device, Pointer.PointerId, rawPosition, position, Pointer.IsInContact, properties);
		}

		private PointerPointProperties GetProperties()
		{
			var props = new PointerPointProperties()
			{
				IsPrimary = true,
				IsInRange = Pointer.IsInRange,
				PointerUpdateKind = _updateKind
			};

			if (Pointer.IsInContact)
			{
				props.IsLeftButtonPressed = _button == VirtualKey.LeftButton;
				props.IsMiddleButtonPressed = _button == VirtualKey.MiddleButton;
				props.IsRightButtonPressed = _button == VirtualKey.RightButton;
			}

			return props;
		}

		#region Misc static helpers
		private static ulong? _bootTime;

		private static ulong ToTimeStamp(double timestamp)
		{
			if (!_bootTime.HasValue)
			{
				_bootTime = (ulong) (double.Parse(WebAssemblyRuntime.InvokeJS("Date.now() - performance.now()")) * TimeSpan.TicksPerMillisecond);
			}

			return _bootTime.Value + (ulong)(timestamp * TimeSpan.TicksPerMillisecond);
		}

		private static uint ToFrameId(double timestamp)
		{
			// Known limitation: After 49 days, we will overflow the uint and frame IDs will restart at 0.
			return (uint)(timestamp % uint.MaxValue);
		}
		#endregion
	}
}
