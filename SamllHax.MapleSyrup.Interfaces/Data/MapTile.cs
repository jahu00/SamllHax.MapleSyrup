using SamllHax.MapleSyrup.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Data
{
    public class MapTile : EntityBase, IMapTile
    {
        public string[] Path { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

    }
}
