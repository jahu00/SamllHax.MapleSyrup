using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Extensions
{
    public static class SKImageExtenstions
    {
        public static SKRectI GetBoundingBox(this SKImage image)
        {
            return SKRectI.Create(image.Width, image.Height);
        }
    }
}
