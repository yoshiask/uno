using System;
using System.Globalization;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using NotImplementedException = System.NotImplementedException;

namespace Windows.UI.Xaml.Wasm
{
	public partial class SvgElement : UIElement
	{
		public SvgElement(string svgTag) : base(svgTag, isSvg: true)
		{
		}
	}

	[Markup.ContentProperty(Name = "Defs")]
	public partial class SvgDefsElement : UIElement
	{
		public UIElementCollection Defs { get; }

		public SvgDefsElement() : base("defs", isSvg: true, isNonVisual: true)
		{
			Defs = new UIElementCollection(this);
		}
	}

	public partial class SvgLinearGradient : UIElement
	{
		public SvgLinearGradient(LinearGradientBrush linearGradientBrush) : base("defs", isSvg: true, isNonVisual: true)
		{
			SetAttribute(
				("x1", Str(linearGradientBrush.StartPoint.X)),
				("y1", Str(linearGradientBrush.StartPoint.Y)),
				("x2", Str(linearGradientBrush.EndPoint.X)),
				("y2", Str(linearGradientBrush.EndPoint.Y))
			);

			var stops = linearGradientBrush
				.GradientStops
				.Select(stop => $"<stop offset=\"{Str(stop.Offset * 100)}%\" style=\"stop-color:{stop.Color.ToCssString()}\" />");

			SetHtmlContent(string.Join(Environment.NewLine, stops));

			string Str(double d)
			{
				return d.ToString(NumberFormatInfo.InvariantInfo);
			}
		}
	}
}
