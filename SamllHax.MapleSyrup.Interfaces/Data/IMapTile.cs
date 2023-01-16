using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IMapTile : IMapEntityBase
    {
        string[] Path { get; }
        int Z { get; }
    }
}
