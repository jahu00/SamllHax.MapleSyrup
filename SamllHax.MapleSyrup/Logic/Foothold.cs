using SamllHax.MapleSyrup.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Draw
{
    public class Foothold : Line
    {
        public IMapFoothold Data { get; set; }
        public int LayerId { get; set; }
        public Foothold Previous { get; set; }
        public Foothold Next { get; set; }
        public Foothold(float x1, float y1, float x2, float y2) : base(x1, y1, x2, y2)
        {
        }
    }
}
