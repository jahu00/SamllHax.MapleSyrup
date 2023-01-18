using SamllHax.MapleSyrup.Providers.Dumper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Providers.Dumper.Nodes
{
    public class WzExtended : WzDirectory
    {
        public WzVectorCollection ToVectorCollection()
        {
            return new WzVectorCollection(this, Children.Cast<WzVector>());
        }
    }
}
