using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Extensions
{
    public static class SKRectExtensions
    {
        public static SKRectI ToRectI(this SKRect rect)
        {
            return new SKRectI((int)rect.Left, (int)rect.Top, (int)rect.Right, (int)rect.Bottom);
        }
    }
}
