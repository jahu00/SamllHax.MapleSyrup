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
        public Foothold(IMapFoothold data, int layerId): base(x1: data.X1, y1: data.Y1, x2: data.X2, y2: data.Y2)
        {
            Data = data;
            LayerId = layerId;
        }
    }
}
