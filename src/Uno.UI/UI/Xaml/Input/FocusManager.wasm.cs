using System;
using Uno.UI;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Uno;
using Uno.Foundation;

namespace Windows.UI.Xaml.Input
{
	public partial class FocusManager
	{
		/// <summary>
		/// Request to natively select a UI element
		/// </summary>
		/// <param name="element">The element to focus</param>
		/// <returns></returns>
		internal static bool NativeFocus(UIElement element)
		{
			if (element == null)
			{
				return false;
			}

			var command = $"Uno.UI.WindowManager.current.focusView({element.HtmlId});";
			WebAssemblyRuntime.InvokeJS(command);

			return true;
		}

		[Preserve]
		public static void OnNativeGotFocus(int handle)
		{
			var oldElement = _focusedElement;
			var newElement = UIElement.GetElementFromHandle(handle);
			if (newElement == oldElement)
			{
				return;
			}

			// First update the focused element, so **UN**focus event handlers are going to be able to known which element is now focused
			_focusedElement = newElement;

			// Make sure that the managed UI knowns that the element was unfocused, no matter if the new focused element can be focused or not
			(oldElement as Control)?.SetFocused(false);

			// Then try to set the element as focused
			(newElement as Control)?.SetFocused(true);
		}

		private void SetFocused(Control focused, FocusState state)
		{
			// Set new states for both

			
			focused.FocusState = state;
		}

		//private static readonly RoutedEventHandler OnControlFocused = (control, args) =>
		//{
		//	Console.Error.WriteLine($"************* FOCUS {control}");

		//	if (args.OriginalSource != control)
		//	{
		//		return;
		//	}

		//	// Make sure that the managed UI knowns that the element was unfocused, no matter if the new focused element can be focused or not
		//	(_focusedElement as Control)?.SetFocused(false);

		//	// Then set the new control as focused
		//	((Control) control).SetFocused(true);
		//};
		//private static readonly RoutedEventHandler OnElementFocused = (element, args) =>
		//{
		//	Console.Error.WriteLine($"************* FOCUS {element}");

		//	if (args.OriginalSource != element)
		//	{
		//		return;
		//	}

		//	// Make sure that the managed UI knowns that the element was unfocused, no matter if the new focused element can be focused or not
		//	(_focusedElement as Control)?.SetFocused(false);

		//	// Try to find the first focusable parent and set it as focused, otherwise just keep it for reference (GetFocusedElement())
		//	var ownerControl = element.GetParents().OfType<Control>().Where(control => control.IsFocusable).FirstOrDefault();
		//	if (ownerControl == null)
		//	{
		//		_focusedElement = element;
		//	}
		//	else
		//	{
		//		ownerControl.SetFocused(true);
		//		_fallbackFocusedElement = element;
		//	}
		//};

		internal static void Track(UIElement element)
		{
			//var handler = element is Control
				//? OnControlFocused
				//: OnElementFocused;

			//element.GotFocus += handler;
			//element.RegisterEventHandler("focus", handler);
		}

		private static bool InnerTryMoveFocus(FocusNavigationDirection focusNavigationDirection)
		{
			return false;
		}

		private static UIElement InnerFindNextFocusableElement(FocusNavigationDirection focusNavigationDirection)
		{
			return null;
		}
	}
}
