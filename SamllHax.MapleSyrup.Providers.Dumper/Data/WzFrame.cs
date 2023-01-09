using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;

namespace SamllHax.MapleSyrup.Data
{
    public class WzFrame : WzEntity<WzCanvas>, IFrame
    {
        public WzFrame(WzCanvas directory) : base(directory)
        {
            Width = _directory.Width;
            Height = _directory.Height;
            Origin = _directory.GetSingleChild<WzVector>("origin");
            Z = _directory.GetSingleChild<WzIntValue>("z").Value;
            Delay = _directory.GetSingleOrDefaultChild<WzIntValue>("delay")?.Value;
            Alpha0 = _directory.GetSingleOrDefaultChild<WzIntValue>("a0")?.Value;
            Alpha1 = _directory.GetSingleOrDefaultChild<WzIntValue>("a1")?.Value;

            Foothold = _directory.GetSingleOrDefaultChild<WzExtended>("foothold")?.Children.Select(x => (IVector)x).ToList();
        }

        public int Width { get; }
        public int Height { get; }
        public IVector Origin { get; }
        public int Z { get; }
        public int? Delay { get; }
        public int? Alpha0 { get; }
        public int? Alpha1 { get; }
        public List<IVector> Foothold { get; }
    }
}
