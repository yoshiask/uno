using System.Diagnostics;
using System.Runtime.CompilerServices;
using Uno.UI.Xaml;

namespace Windows.UI.Xaml
{
	[DebuggerDisplay("{" + nameof(Name) + "}")]
	public partial class RoutedEvent
	{
		public string Name { get; }

		internal RoutedEventFlag Flag { get; }

		internal bool IsTunneling { get; }

		public RoutedEvent([CallerMemberName] string name = null)
			: this(RoutedEventFlag.None, name)
		{
		}

		internal RoutedEvent(
			RoutedEventFlag flag,
			[CallerMemberName] string name = null,
			bool isTunneling = false)
		{
			Flag = flag;
			Name = name;
			IsTunneling = isTunneling;
		}
	}
}
