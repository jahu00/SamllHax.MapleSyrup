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
    }
}
