using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Draw
{
    public abstract class DrawableBase
    {
        public virtual int OffsetX { get; set; } = 0;
        public virtual int OffsetY { get; set; } = 0;
        public virtual float X { get; set; } = 0;
        public virtual float Y { get; set; } = 0;
        public virtual float ScaleX { get; set; } = 1;
        public virtual float ScaleY { get; set; } = 1;
    }
}
