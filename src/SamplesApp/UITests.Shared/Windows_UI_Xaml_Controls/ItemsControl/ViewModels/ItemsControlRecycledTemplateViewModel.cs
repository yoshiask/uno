using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Uno.UI.Samples.UITests.Helpers;
using Windows.UI.Core;

namespace SamplesApp.Windows_UI_Xaml_Controls.ItemsControl
{
	public class ItemsControlRecycledTemplateViewModel : ViewModelBase
	{
		private RecycledTemplateViewModel[] _items;

		public ItemsControlRecycledTemplateViewModel(CoreDispatcher dispatcher) : base(dispatcher)
		{
			AddNewItemCommand = CreateCommand(AddNewItem);
			RefreshCommand = CreateCommand(RefreshItems);

			Items = new RecycledTemplateViewModel[]
			{
				new RecycledTemplateViewModel(dispatcher),
				new RecycledTemplateViewModel(dispatcher),
				new RecycledTemplateViewModel(dispatcher)
			};
		}

		public ICommand RefreshCommand { get; }
		public ICommand AddNewItemCommand { get; }

		public RecycledTemplateViewModel[] Items
		{
			get => _items;
			set
			{
				_items = value;
				RaisePropertyChanged();
			}
		}

		private void RefreshItems()
		{
			// Act as a refresh
			Items = Items;
		}

		private void AddNewItem()
		{
			var list = Items.ToList();
			list.Add(new RecycledTemplateViewModel(Dispatcher));
			Items = list.ToArray();
		}
	}
}
