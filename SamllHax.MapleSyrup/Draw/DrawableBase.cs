using SamllHax.PlatformerLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Draw
{
    public abstract class DrawableBase: IPoint
    {
        public virtual int OriginX { get; set; } = 0;
        public virtual int OriginY { get; set; } = 0;
        public virtual float X { get; set; } = 0;
        public virtual float Y { get; set; } = 0;
        public virtual int Z { get; set; } = 0;
        public virtual float ScaleX { get; set; } = 1;
        public virtual float ScaleY { get; set; } = 1;
    }
}
