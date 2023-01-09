using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;

namespace SamllHax.MapleSyrup.Data
{
    public class WzMapObject : WzEntity, IMapObject
    {
        public WzMapObject(WzDirectory directory) : base(directory)
        {
            DirectoryName = _directory.GetSingleChild<WzStringValue>("oS").Value;
            Path = new string[]
            {
                _directory.GetSingleChild<WzStringValue>("l0").Value,
                _directory.GetSingleChild<WzStringValue>("l1").Value,
                _directory.GetSingleChild<WzStringValue>("l2").Value
            };
            X = _directory.GetSingleChild<WzIntValue>("x").Value;
            Y = _directory.GetSingleChild<WzIntValue>("y").Value;
            Z = _directory.GetSingleChild<WzIntValue>("z").Value;
            FlipX = _directory.GetSingleChild<WzIntValue>("f").Value == 1;
            Zm = _directory.GetSingleChild<WzIntValue>("zM").Value;
        }
        public string DirectoryName { get; }

        public string[] Path { get; }
        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        public bool FlipX { get; }
        public int Zm { get; }
    }
}
