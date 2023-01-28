using SamllHax.PlatformerLogic;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Extensions
{
    public static class SKCanvasExtensions
    {
        public static void DrawBitmap(this SKCanvas canvas, SKBitmap bitmap, SKMatrix matrix)
        {
            canvas.SetMatrix(matrix);
            canvas.DrawBitmap(bitmap, 0, 0);
        }

        public static void DrawImage(this SKCanvas canvas, SKImage bitmap, SKMatrix matrix)
        {
            canvas.SetMatrix(matrix);
            canvas.DrawImage(bitmap, 0, 0);
        }

        public static void DrawLine(this SKCanvas canvas, ILineSegment lineSegment, SKPaint paint, SKMatrix matrix)
        {
            canvas.SetMatrix(matrix);
            canvas.DrawLine(lineSegment.X1, lineSegment.Y1, lineSegment.X2, lineSegment.Y2, paint);
        }

        public static void DrawRect(this SKCanvas canvas, SKRect rect, SKPaint paint, SKMatrix matrix)
        {
            canvas.SetMatrix(matrix);
            canvas.DrawRect(rect, paint);
        }
    }
}
