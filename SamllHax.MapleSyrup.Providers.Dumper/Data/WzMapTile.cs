using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;

namespace SamllHax.MapleSyrup.Data
{
    public class WzMapTile : WzEntity, IMapTile
    {
        public WzMapTile(WzDirectory directory) : base(directory)
        {
            X = _directory.GetSingleChild<WzIntValue>("x").Value;
            Y = _directory.GetSingleChild<WzIntValue>("y").Value;
            Z = _directory.GetSingleChild<WzIntValue>("zM").Value;
            Path = new string[]
            {
                _directory.GetSingleChild<WzStringValue>("u").Value,
                _directory.GetSingleChild<WzIntValue>("no").Value.ToString()
            };
        }

        public string[] Path { get; }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }
    }
}
