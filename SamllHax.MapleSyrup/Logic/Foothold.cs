using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.PlatformerLogic;

namespace SamllHax.MapleSyrup.Logic
{
    public class Foothold : LineSegment
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
