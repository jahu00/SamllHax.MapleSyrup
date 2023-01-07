using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Data
{
    public class WzMapTile : WzEntity
    {
        public WzMapTile(WzDirectory directory) : base(directory)
        {
        }

        public int X => _directory.GetSingleChild<WzIntValue>("x").Value;
        public int Y => _directory.GetSingleChild<WzIntValue>("y").Value;
        public int Z => _directory.GetSingleChild<WzIntValue>("zM").Value;
        public string Name => _directory.GetSingleChild<WzStringValue>("u").Value;
        public int Variant => _directory.GetSingleChild<WzIntValue>("no").Value;
    }
}
