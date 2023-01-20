using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Providers.Dumper.Nodes
{
    public class WzCanvas : WzDirectory
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string FramePath { get; set; }
    }
}
