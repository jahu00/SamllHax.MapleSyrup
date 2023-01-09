using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SamllHax.MapleSyrup.Providers.Dumper.Nodes
{
    public class WzNode
    {
#if DEBUG
        public XElement Xml { get; set; }
#endif

        public string Name { get; set; }
    }
}
