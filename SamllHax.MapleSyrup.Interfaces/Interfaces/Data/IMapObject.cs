using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IMapObject : IEntity
    {
        string DirectoryName { get; }
        string[] Path { get; }
        int X { get; }
        int Y { get; }
        int Z { get; }
        bool FlipX { get; }
        int Zm { get; }
    }
}
