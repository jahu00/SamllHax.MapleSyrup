using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Data
{
    public class WzFrame : WzEntity<WzCanvas>
    {
        public WzFrame(WzCanvas directory) : base(directory)
        {
        }

        public int Width => _directory.Width;
        public int Height => _directory.Height;
        public WzVector Origin => _directory.GetSingleChild<WzVector>("origin");
        public int Z => _directory.GetSingleChild<WzIntValue>("z").Value;
        public int? Delay => _directory.GetSingleOrDefaultChild<WzIntValue>("delay")?.Value;
        public int? Alpha0 => _directory.GetSingleOrDefaultChild<WzIntValue>("a0")?.Value;
        public int? Alpha1 => _directory.GetSingleOrDefaultChild<WzIntValue>("a1")?.Value;

        /// <summary>
        /// Foothold as used by tiles
        /// </summary>
        public List<WzVector> Foothold => _directory.GetSingleOrDefaultChild<WzExtended>("foothold")?.Children.Select(x => (WzVector)x).ToList();
    }
}
