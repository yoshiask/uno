using Android.Graphics;
using Uno.UI;

namespace Windows.UI.Xaml
{
	partial struct CornerRadius
	{
		internal Path GetFillPath(RectF rect)
		{
			var radii = new float[]
			{
				ViewHelper.LogicalToPhysicalPixels(TopLeft),
				ViewHelper.LogicalToPhysicalPixels(TopLeft),
				ViewHelper.LogicalToPhysicalPixels(TopRight),
				ViewHelper.LogicalToPhysicalPixels(TopRight),
				ViewHelper.LogicalToPhysicalPixels(BottomRight),
				ViewHelper.LogicalToPhysicalPixels(BottomRight),
				ViewHelper.LogicalToPhysicalPixels(BottomLeft),
				ViewHelper.LogicalToPhysicalPixels(BottomLeft)
			};

			var path = new Path();
			path.AddRoundRect(rect, radii, Path.Direction.Cw);

			return path;
		}

		internal Path GetOutlinePath(RectF rect, Thickness thickness)
		{
			if (thickness.IsUniform())
			{
				return GetFillPath(rect);
			}

			// normally, an uniform round rectangle would look like this:
			// ╭1───────────2╮
			// 0             3
			// │             │
			// 7             4
			// ╰6───────────5╯
			// we could draw this in one step with Path.AddRoundRect, or multiple steps a combinaison of Arc+Line.
			// however, if an edge is missing (Thickness part equals to zero), the edge line will be skipped along with the two adjacent corner.
			// note: if a corner is missing (when CornerRadius part equals to zero), the 2 points would just merge into one at where the corner is.
			// fixme: uwp would draw a partial round corner half way through and then fade (ex: Thickness=0-1-0-0 (top), CornerRadius=16-16-0-0 (top-left + top right))

			var physicalValues = this.LogicalToPhysicalPixels();
			var corners = new[] // top-left, top-right, bottom-right, bottom-left
			{
				new PointF(rect.Left, rect.Top),
				new PointF(rect.Right, rect.Top),
				new PointF(rect.Right, rect.Bottom),
				new PointF(rect.Left, rect.Bottom),
			};
			var bbox = new RectF[] // top-left, top-right, bottom-right, bottom-left
			{
				new RectF(rect.Left, rect.Top, rect.Left + 2 * (float)physicalValues.TopLeft, rect.Top + 2 * (float)physicalValues.TopLeft),
				new RectF(rect.Right - 2 * (float)physicalValues.TopRight, rect.Top, rect.Right, rect.Top + 2 * (float)physicalValues.TopRight),
				new RectF(rect.Right - 2 * (float)physicalValues.BottomRight, rect.Bottom - 2 * (float)physicalValues.BottomRight, rect.Right, rect.Bottom),
				new RectF(rect.Left, rect.Bottom - 2 * (float)physicalValues.BottomLeft, rect.Left + 2 * (float)physicalValues.BottomLeft, rect.Bottom),
			};
			var points = new PointF[] // refers to diagram above for index
			{
				corners[0].OffsetBy(dy: +ViewHelper.LogicalToPhysicalPixels(TopLeft)), // 0
				corners[0].OffsetBy(dx: +ViewHelper.LogicalToPhysicalPixels(TopLeft)), // 1
				corners[1].OffsetBy(dx: -ViewHelper.LogicalToPhysicalPixels(TopRight)), // 2
				corners[1].OffsetBy(dy: +ViewHelper.LogicalToPhysicalPixels(TopRight)), // 3
				corners[2].OffsetBy(dy: -ViewHelper.LogicalToPhysicalPixels(BottomRight)), // 4
				corners[2].OffsetBy(dx: -ViewHelper.LogicalToPhysicalPixels(BottomRight)), // 5
				corners[3].OffsetBy(dx: +ViewHelper.LogicalToPhysicalPixels(BottomLeft)), // 6
				corners[3].OffsetBy(dy: -ViewHelper.LogicalToPhysicalPixels(BottomLeft)), // 7
			};

			var path = new Path();
			// ╭1───────────2╮
			// 0             3
			// │             │
			// 7             4
			// ╰6───────────5╯

			//path.MoveTo(points[0].X, points[0].Y);	// 0
			//path.ArcTo(bbox[0], 180, 90, false);	// 0-1
			//path.LineTo(points[2].X, points[2].Y);	// 1-2
			//path.ArcTo(bbox[1], 270, 90, false);	// 2-3
			//path.LineTo(points[4].X, points[4].Y);	// 3-4
			//path.ArcTo(bbox[2], 0, 90, false);		// 4-5
			//path.LineTo(points[6].X, points[6].Y);  // 5-6
			//path.ArcTo(bbox[3], 90, 90, false);    // 6-7
			//path.LineTo(points[0].X, points[0].Y);  // 7-0

			path.MoveTo(points[0].X, points[0].Y);
			ArcToIf(bbox[0], points[1], 180, thickness.Left, thickness.Top);
			LineToIf(points[2], thickness.Top);
			ArcToIf(bbox[1], points[3], 270, thickness.Top, thickness.Right);
			LineToIf(points[4], thickness.Right);
			ArcToIf(bbox[2], points[5], 0, thickness.Right, thickness.Bottom);
			LineToIf(points[6], thickness.Bottom);
			ArcToIf(bbox[3], points[7], 90, thickness.Bottom, thickness.Left);
			LineToIf(points[0], thickness.Left);

			return path;

			void ArcToIf(RectF bbox, PointF end, float baseStartAngle, double firstHalfWidth, double secondHalfWidth)
			{
				if (!bbox.IsEmpty)
				{
					var startAngle = baseStartAngle + (firstHalfWidth > 0 ? 0 : 45);
					var sweepAngle = (firstHalfWidth > 0 ? 45 : 0) + (secondHalfWidth > 0 ? 45 : 0);

					path.ArcTo(bbox, startAngle, sweepAngle);
				}
				else
				{
					path.MoveTo(end.X, end.Y);
				}
			}
			void LineToIf(PointF p, double width)
			{
				if (width > 0)
				{
					path.LineTo(p.X, p.Y);
				}
				else
				{
					path.MoveTo(p.X, p.Y);
				}
			}
		}
	}
}
