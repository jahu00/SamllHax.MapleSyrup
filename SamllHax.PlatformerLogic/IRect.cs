using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.PlatformerLogic
{
    public interface IRect
    {
        float Top { get; }
        float Bottom { get; }
        float Left { get; }
        float Right { get; }
    }

    public static class IRectExtensions
    {
        public static bool ContainsHorizontally(this ILineSegment lineSegment, float x) => !OutsideHorizontally(lineSegment, x);
        public static bool OutsideHorizontally(this ILineSegment lineSegment, float x)
        {
            return x < lineSegment.Left || x > lineSegment.Right;
        }

        public static bool ContainsVertically(this ILineSegment lineSegment, float y) => !OutsideVertically(lineSegment, y);

        public static bool OutsideVertically(this ILineSegment lineSegment, float y)
        {
            return y < lineSegment.Top || y > lineSegment.Bottom;
        }
    }
}
