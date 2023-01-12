using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Extensions
{
    public static class SKBitmapExtenstions
    {
        public static SKRectI GetBoundingBox(this SKBitmap bitmap)
        {
            return SKRectI.Create(bitmap.Width, bitmap.Height);
        }
    }
}
