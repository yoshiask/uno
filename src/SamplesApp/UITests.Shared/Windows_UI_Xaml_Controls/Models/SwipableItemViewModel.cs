using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uno.UI.Samples.UITests.Helpers;
using Windows.UI.Core;

namespace UITests.Shared.Windows_UI_Xaml_Controls.Models
{
	public class SwipableItemViewModel : ViewModelBase
	{
		public SwipableItemViewModel(CoreDispatcher dispatcher) : base(dispatcher)
		{

		}

		public SwipeItem[] Items => GetItems();

		public SwipeItem SingleItem => GetSingleItem();

		private SwipeItem[] GetItems()
		{
			return new SwipeItem[]
			{
					new SwipeItem(){Content = "Content 1", NearContent = "Near 1", FarContent = "Far 1" },
					new SwipeItem(){Content = "Content 2", NearContent = "Near 2", FarContent = "Far 2" },
					new SwipeItem(){Content = "Content 3", NearContent = "Near 3", FarContent = "Far 3" },
					new SwipeItem(){Content = "Content 4", NearContent = "Near 4", FarContent = "Far 4" },
					new SwipeItem(){Content = "Content 5", NearContent = "Near 5", FarContent = "Far 5" },
					new SwipeItem(){Content = "Content 6", NearContent = "Near 6", FarContent = "Far 6" },
					new SwipeItem(){Content = "Content 7", NearContent = "Near 7", FarContent = "Far 7" },
					new SwipeItem(){Content = "Content 8", NearContent = "Near 8", FarContent = "Far 8" },
					new SwipeItem(){Content = "Content 9", NearContent = "Near 9", FarContent = "Far 9" },
					new SwipeItem(){Content = "Content 10", NearContent = "Near 10", FarContent = "Far 10" },
			};
		}

		private SwipeItem GetSingleItem()
		{
			return new SwipeItem() { Content = "Content", NearContent = "Near", FarContent = "Far" };
		}

		public class SwipeItem
		{
			public string Content { get; set; }

			public string NearContent { get; set; }

			public string FarContent { get; set; }
		}
	}
}
