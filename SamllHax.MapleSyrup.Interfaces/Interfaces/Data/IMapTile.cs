using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IMapTile : IEntity
    {
        string[] Path { get; }
        int X { get; }
        int Y { get; }
        int Z { get; }
    }
}
