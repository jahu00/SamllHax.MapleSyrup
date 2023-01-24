using SamllHax.MapleSyrup.Extensions;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Draw
{
    public class Line: IBoundable, IDrawable
    {
        public float X1 { get; }
        public float Y1 { get; }
        public float X2 { get; }
        public float Y2 { get; }
        public LineType Type { get; }
        public bool IsVertical => Type == LineType.Vertical;
        public bool IsHorizontal => Type == LineType.Horizontal;
        public float A { get; }
        public float B { get; }
        public SKRect BoudingBox { get; }
        public SKPaint Paint { get; set; }

        public int OriginX => 0;

        public int OriginY => 0;

        public float X => 0;

        public float Y => 0;

        public float ScaleX => 1;

        public float ScaleY => 1;

        public Line(float x1, float y1, float x2, float y2)
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
            BoudingBox = new SKRect(X1, Math.Min(Y1, Y2), X2, Math.Max(Y1, Y2));
            if (X1 == X2)
            {
                Type = LineType.Vertical;
                A = float.NaN;
                B = float.NaN;
                //BoudingBox = new SKRect(X1 - 10, Math.Min(Y1, Y2), X2, Math.Max(Y1, Y2) + 10);
            }
            else if (Y1 == Y2)
            {
                Type = LineType.Horizontal;
                A = 0;
                B = Y1;
                //BoudingBox = new SKRect(X1, Math.Min(Y1, Y2) - 10, X2, Math.Max(Y1, Y2) + 10);
            } else
            {
                Type = LineType.Oblique;
                A = (float)(Y1 - Y2) / (float)(X1 - X2);
                B = Y1 - A * X1;
                //BoudingBox = new SKRect(X1 - 10, Math.Min(Y1, Y2) - 10, X2 + 10, Math.Max(Y1, Y2) + 10);
            }
        }

        public SKPoint? LineIntersectPoint(Line otherLine)
        {
            if (Type == otherLine.Type)
            {
                return null;
            }
            if (A == otherLine.A)
            {
                return null;
            }
            if (IsVertical)
            {
                if (otherLine.IsHorizontal)
                {
                    return new SKPoint(X1, otherLine.Y1);
                }
                else
                {
                    return new SKPoint(X1, otherLine.A * X1 + otherLine.B);
                }
            }
            if (otherLine.IsVertical)
            {
                if (IsHorizontal)
                {
                    return new SKPoint(otherLine.X1, Y1);
                }
                else
                {
                    return new SKPoint(otherLine.X1, A * otherLine.X1 + B);
                }
            }
            var x = (B - otherLine.B) / (otherLine.A - A);
            var y = GetY(x);
            var result = new SKPoint(x, y);
            /*if (result.X == float.NaN || result.Y == float.NaN)
            {
                Console.WriteLine("Messup");
            }*/
            return result;
        }

        public SKPoint? LineSegmentIntersectPoint(Line otherLine)
        {
            var point = LineIntersectPoint(otherLine);
            if (!point.HasValue)
            {
                return null;
            }
            if (!Contains(point.Value))
            {
                return null;
            }
            if (!otherLine.Contains(point.Value))
            {
                return null;
            }
            return point;
        }

        protected bool Contains(SKPoint point)
        {
            var outsideHorizontally = OutsideHorizontally(point.X);
            if (IsHorizontal && outsideHorizontally)
            {
                return false;
            }
            var outsideVertically = (point.Y < BoudingBox.Top || point.Y > BoudingBox.Bottom);
            if (IsVertical && outsideVertically)
            {
                return false;
            }

            return !outsideVertically && !outsideHorizontally;
        }

        public bool OutsideHorizontally(float x)
        {
            return (x < BoudingBox.Left || x > BoudingBox.Right);
        }

        public bool ContainsHorizontally(float x)
        {
            return !OutsideHorizontally(x);
        }

        public SKRectI GetBoundingBox()
        {
            return BoudingBox.ToRectI();
        }

        public void Draw(SKCanvas canvas, SKMatrix matrix)
        {
            if (Paint == null)
            {
                return;
            }
            canvas.SetMatrix(matrix);
            canvas.DrawLine(X1, Y1, X2, Y2, Paint);
        }

        public float GetY(float x)
        {
            return A * x + B;
        }
    }
}
