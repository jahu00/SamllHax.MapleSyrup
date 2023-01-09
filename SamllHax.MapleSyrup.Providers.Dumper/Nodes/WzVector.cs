using SamllHax.MapleSyrup.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Providers.Dumper.Nodes
{
    public class WzVector : WzNode, IVector
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
