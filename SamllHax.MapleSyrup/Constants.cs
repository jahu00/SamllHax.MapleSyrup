using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup
{
    public static class Constants
    {
        public static int DefaultDelay { get; } = 100;
        public static SKRectI PortalBoundingBox { get; } = new SKRectI(-25, -100, 25, 25);
    }
}
