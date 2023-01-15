using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IMapObject : IMapEntityBase
    {
        string DirectoryName { get; }
        string[] Path { get; }
        int Z { get; }
        bool FlipX { get; }
        int Zm { get; }
    }

    public static class IMapObjectExtensions
    {
        public static IEnumerable<string> GetFullPath(this IMapObject obj)
        {
            var pathPrefix = new string[] { "Obj", obj.DirectoryName + ".img" };
            return pathPrefix.Concat(obj.Path);
        }
    }
}
