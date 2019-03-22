using System;
using System.Collections.Generic;
using System.Windows.Input;
using Uno.UI.Samples.UITests.Helpers;
using Windows.UI.Core;

namespace SamplesApp.Windows_UI_Xaml_Controls.ItemsControl
{
	public class ItemsControlRecycledTemplateViewModel : ViewModelBase
	{
		private List<ViewModelBase> _items;

		public ItemsControlRecycledTemplateViewModel(CoreDispatcher dispatcher) : base(dispatcher)
		{
#if HAS_UNO
			Windows.UI.Xaml.FrameworkTemplatePool.IsPoolingEnabled = true;
#endif
			RefreshItemsCommand = CreateCommand(RefreshItems);
			Items = new List<ViewModelBase>()
			{
				new CheckBoxViewModel(dispatcher),
				new CheckBoxViewModel(dispatcher),
				new CheckBoxViewModel(dispatcher),
				new CheckBoxViewModel(dispatcher),
				new CheckBoxViewModel(dispatcher),
				new CheckBoxViewModel(dispatcher)
			};
		}

		private void RefreshItems(object obj)
		{
			// Act as a refresh
			Items = Items;
		}

		public List<ViewModelBase> Items
		{
			get => _items;
			set
			{
				_items = value;
				RaisePropertyChanged();
			}
		}

		public ICommand RefreshItemsCommand { get; } 
	}
}
