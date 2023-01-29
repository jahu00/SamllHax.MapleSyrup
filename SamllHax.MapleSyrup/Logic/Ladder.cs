using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.PlatformerLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Logic
{
    public class Ladder : LineSegment
    {
        public IMapLadder Data { get; }
        public Ladder(IMapLadder data): base(data.X, Math.Min(data.Y1, data.Y2), Math.Max(data.Y1, data.Y2))
        {
            Data = data;
        }
    }
}
