using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndroidPointF = Android.Graphics.PointF;

namespace Uno.UI
{
	internal static partial class LayoutHelper
	{
		[Pure]
		internal static AndroidPointF OffsetBy(this AndroidPointF p, float dx = 0, float dy = 0) => new AndroidPointF(p.X + dx, p.Y + dy);
	}
}
