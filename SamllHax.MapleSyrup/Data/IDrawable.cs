using OpenTK.Graphics.ES11;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Draw
{
    public interface IDrawable
    {
        int X { get; set; }
        int Y { get; set; }
        void Draw(SKCanvas canvas, int x, int y);
    }
}
