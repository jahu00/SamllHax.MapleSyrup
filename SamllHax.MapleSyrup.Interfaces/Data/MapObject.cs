using SamllHax.MapleSyrup.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Data
{
    public class MapObject : EntityBase, IMapObject
    {
        public string DirectoryName { get; set; }

        public string[] Path { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public bool FlipX { get; set; }

        public int Zm { get; set; }

    }
}
