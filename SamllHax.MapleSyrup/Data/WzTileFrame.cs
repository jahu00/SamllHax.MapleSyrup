using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Data
{
    public class WzTileFrame : WzFrame
    {
        public WzTileFrame(WzCanvas directory) : base(directory)
        {
        }

        public List<WzVector> Foothold => _directory.GetSingleOrDefaultChild<WzExtended>("foothold")?.Children.Select(x => (WzVector)x).ToList();
    }
}
