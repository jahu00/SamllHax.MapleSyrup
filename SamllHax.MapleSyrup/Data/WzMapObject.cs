using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Data
{
    public class WzMapObject : WzEntity
    {
        public WzMapObject(WzDirectory directory) : base(directory)
        {
        }

        public string GroupName => _directory.GetSingleChild<WzStringValue>("oS").Value;
        public string Name => _directory.GetSingleChild<WzStringValue>("l0").Value;
        public string SubsetName => _directory.GetSingleChild<WzStringValue>("l1").Value;
        public string PartId => _directory.GetSingleChild<WzStringValue>("l2").Value;
        public int X => _directory.GetSingleChild<WzIntValue>("x").Value;
        public int Y => _directory.GetSingleChild<WzIntValue>("y").Value;
        public int Z => _directory.GetSingleChild<WzIntValue>("z").Value;
        public bool FlipX => _directory.GetSingleChild<WzIntValue>("f").Value == 1;
        public int Zm => _directory.GetSingleChild<WzIntValue>("zM").Value;
    }
}
