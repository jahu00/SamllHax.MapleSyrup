using System;
using System.Collections.Generic;
using System.Text;
using SamllHax.MapleSyrup.Interfaces.Data;

namespace SamllHax.MapleSyrup.Data
{
    public class Map : EntityBase, IMap
    {
        public string MapMark { get; set; }

        public List<IMapLayer> Layers { get; set; }

    }
}
