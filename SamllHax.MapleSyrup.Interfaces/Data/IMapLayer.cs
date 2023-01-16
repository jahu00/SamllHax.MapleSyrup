using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IMapLayer : IEntity
    {
        string TileSetName { get; }
        List<IMapTile> Tiles { get; }
        List<IMapObject> Objects { get; }
    }
}
