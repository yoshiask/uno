#if __ANDROID__ || __IOS__
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Windows.UI.Xaml.Controls
{
	public partial class SwipableItem
	{
#region NEAR PROPERTIES

		public DataTemplate NearContentTemplate
		{
			get { return (DataTemplate)GetValue(NearContentTemplateProperty); }
			set { SetValue(NearContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty NearContentTemplateProperty =
			DependencyProperty.Register("NearContentTemplate", typeof(DataTemplate), typeof(SwipableItem), new PropertyMetadata(null));

		public object NearContent
		{
			get { return (object)GetValue(NearContentProperty); }
			set { SetValue(NearContentProperty, value); }
		}

		public static readonly DependencyProperty NearContentProperty =
			DependencyProperty.Register("NearContent", typeof(object), typeof(SwipableItem), new PropertyMetadata(null));

		public DataTemplateSelector NearContentTemplateSelector
		{
			get { return (DataTemplateSelector)GetValue(NearContentTemplateSelectorProperty); }
			set { SetValue(NearContentTemplateSelectorProperty, value); }
		}

		public static readonly DependencyProperty NearContentTemplateSelectorProperty =
			DependencyProperty.Register("NearContentTemplateSelector", typeof(DataTemplateSelector), typeof(SwipableItem), new PropertyMetadata(null));

#endregion

#region FAR PROPERTIES

		public DataTemplate FarContentTemplate
		{
			get { return (DataTemplate)GetValue(FarContentTemplateProperty); }
			set { SetValue(FarContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty FarContentTemplateProperty =
			DependencyProperty.Register("FarContentTemplate", typeof(DataTemplate), typeof(SwipableItem), new PropertyMetadata(null));

		public object FarContent
		{
			get { return (object)GetValue(FarContentProperty); }
			set { SetValue(FarContentProperty, value); }
		}

		public static readonly DependencyProperty FarContentProperty =
			DependencyProperty.Register("FarContent", typeof(object), typeof(SwipableItem), new PropertyMetadata(null));


		public DataTemplateSelector FarContentTemplateSelector
		{
			get { return (DataTemplateSelector)GetValue(FarContentTemplateSelectorProperty); }
			set { SetValue(FarContentTemplateSelectorProperty, value); }
		}

		public static readonly DependencyProperty FarContentTemplateSelectorProperty =
			DependencyProperty.Register("FarContentTemplateSelector", typeof(DataTemplateSelector), typeof(SwipableItem), new PropertyMetadata(null));

#endregion

#region RESET PROPERTIES
		/// <summary>
		/// Determines if we reset the control to NotSwiped when the datacontext changes (typically when recycling)
		/// or when the user is interacting outside of the control.
		/// Should set to false if we use binding on SwipeState
		/// </summary>
		public bool IsAutoReset
		{
			get { return (bool)GetValue(IsAutoResetProperty); }
			set { SetValue(IsAutoResetProperty, value); }
		}

		public static readonly DependencyProperty IsAutoResetProperty =
			DependencyProperty.Register("IsAutoReset", typeof(bool), typeof(SwipableItem), new PropertyMetadata(true));

#endregion

#region SNAP PROPERTIES

		public double NearSnapWidth
		{
			get { return (double)GetValue(NearSnapWidthProperty); }
			set { SetValue(NearSnapWidthProperty, value); }
		}

		public static readonly DependencyProperty NearSnapWidthProperty =
			DependencyProperty.Register("NearSnapWidth", typeof(double), typeof(SwipableItem), new PropertyMetadata(0.0, OnSnapChanged));

		public double FarSnapWidth
		{
			get { return (double)GetValue(FarSnapWidthProperty); }
			set { SetValue(FarSnapWidthProperty, value); }
		}

		public static readonly DependencyProperty FarSnapWidthProperty =
			DependencyProperty.Register("FarSnapWidth", typeof(double), typeof(SwipableItem), new PropertyMetadata(0.0, OnSnapChanged));

		private static void OnSnapChanged(object o, DependencyPropertyChangedEventArgs e)
		{
			var item = (SwipableItem)o;
			item.InitializeSnapPoints();
		}

#endregion

#region SWIPESTATE PROPERTIES

		public SwipingState? SwipeState
		{
			get { return (SwipingState?)GetValue(SwipeStateProperty); }
			set { SetValue(SwipeStateProperty, value); }
		}

		public static readonly DependencyProperty SwipeStateProperty =
			DependencyProperty.Register("SwipeState", typeof(SwipingState?), typeof(SwipableItem), new PropertyMetadata(null, OnSwipeStateChanged));

		private static void OnSwipeStateChanged(object o, DependencyPropertyChangedEventArgs e)
		{
			var item = (SwipableItem)o;
			var newState = e.NewValue as SwipingState?;

			if (newState.HasValue)
			{
				item.OnStateChanged(newState.Value, useTransitions: !item.IsDataContextChanging);
			}
		}

#endregion

#region SWIPEDECISIONPOINT PROPERTIES

		/// <summary>
		/// Expected value between 0 and 1.
		/// </summary>
		public double SwipeDecisionPoint
		{
			get { return (double)GetValue(SwipeDecisionPointProperty); }
			set { SetValue(SwipeDecisionPointProperty, value); }
		}

		public static readonly DependencyProperty SwipeDecisionPointProperty =
			DependencyProperty.Register("SwipeDecisionPoint", typeof(double), typeof(SwipableItem), new PropertyMetadata(0.8d));

#endregion
	}
}
#endif
