using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

namespace SamllHax.PlatformerLogic
{
    public interface ILineSegment: ILine, IRect
    {
        float X1 { get; }
        float Y1 { get; }
        float X2 { get; }
        float Y2 { get; }
    }

    public static class ILineSegmentExtensions
    {
        internal static bool ContainsPoint(this ILineSegment lineSegment, IPoint point)
        {
            var outsideHorizontally = lineSegment.OutsideHorizontally(point.X);
            if (lineSegment.Type == LineType.Horizontal && outsideHorizontally)
            {
                return false;
            }
            var outsideVertically = lineSegment.OutsideVertically(point.Y);
            if (lineSegment.Type == LineType.Vertical && outsideVertically)
            {
                return false;
            }

            return !outsideVertically && !outsideHorizontally;
        }
    }
}
