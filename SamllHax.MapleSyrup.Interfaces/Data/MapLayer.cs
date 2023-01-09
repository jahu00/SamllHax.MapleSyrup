using SamllHax.MapleSyrup.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Data
{
    public class MapLayer : EntityBase, IMapLayer
    {
        public string TileSetName { get; set; }

        public List<IMapTile> Tiles { get; set; }

        public List<IMapObject> Objects { get; set; }

    }
}
