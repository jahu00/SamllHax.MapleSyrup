using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.PlatformerLogic
{
    public class LineSegment : ILineSegment
    {
        public float X1 { get; }

        public float Y1 { get; }

        public float X2 { get; }

        public float Y2 { get; }

        public float Top { get; }

        public float Bottom { get; }

        public float Left { get; }

        public float Right { get; }

        public float A { get; }

        public float B { get; }

        public LineType Type { get; }

        public LineSegment(float x1, float y1, float x2, float y2)
        {
            if (x2 < x1)
            {
                X1 = x2;
                Y1 = y2;
                X2 = x1;
                Y2 = y1;
            }
            else
            {
                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;
            }
            Left = X1;
            Right = X2;
            Top = Math.Min(Y1, Y2);
            Bottom = Math.Max(Y1, Y2);
            if (X1 == X2)
            {
                Type = LineType.Vertical;
                A = float.NaN;
                B = float.NaN;
            }
            else if (Y1 == Y2)
            {
                Type = LineType.Horizontal;
                A = 0;
                B = Y1;
            }
            else
            {
                Type = LineType.Oblique;
                A = (float)(Y1 - Y2) / (float)(X1 - X2);
                B = Y1 - A * X1;
            }
        }
    }
}
