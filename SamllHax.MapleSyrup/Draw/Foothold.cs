using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Draw
{
    public class Foothold : Line
    {
        public int LayerId { get; private set; }
        public Foothold(float x1, float y1, float x2, float y2, int layerId) : base(x1, y1, x2, y2)
        {
            LayerId = layerId;
        }
    }
}
