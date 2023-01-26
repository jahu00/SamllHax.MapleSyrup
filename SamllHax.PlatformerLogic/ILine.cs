using System;
using System.Drawing;

namespace SamllHax.PlatformerLogic
{
    public interface ILine
    {
        float A { get; }
        float B { get; }
        LineType Type { get; }
    }

    public static class ILineExtensions
    {
        public static float GetY(this ILine line, float x)
        {
            return line.A * x + line.B;
        }
    }
}
