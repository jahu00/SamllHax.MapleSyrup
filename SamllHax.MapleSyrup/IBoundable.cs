using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Draw
{
    public interface IBoundable
    {
        SKRectI GetBoundingBox(int x, int y);
    }
}
