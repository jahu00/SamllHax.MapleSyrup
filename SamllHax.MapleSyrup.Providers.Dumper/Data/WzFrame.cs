using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzFrame : WzEntity<WzCanvas>, IFrame
    {
        public WzFrame(WzCanvas directory) : base(directory)
        {
            Width = _directory.Width;
            Height = _directory.Height;
            Origin = _directory.GetSingleChild<WzVector>("origin");
            Z = _directory.GetSingleOrDefaultChild<WzIntValue>("z")?.Value;
            Delay = _directory.GetIntValueFromChild("delay");
            Alpha0 = _directory.GetIntValueFromChild("a0");
            Alpha1 = _directory.GetIntValueFromChild("a1");

            var footholdNode = _directory.GetSingleOrDefaultChild<WzNode>("foothold");
            if (footholdNode is WzExtended)
            {
                Footholds = new List<IFoothold>() { new WzFoothold((WzExtended)footholdNode) };
            }
            else if (footholdNode is WzDirectory)
            {
                Footholds = _directory.GetSingleOrDefaultChild<WzDirectory>("foothold").Children.Select(x => (IFoothold)new WzFoothold((WzExtended)x)).ToList();
            }
        }

        public int Width { get; }
        public int Height { get; }
        public IVector Origin { get; }
        public int? Z { get; }
        public int? Delay { get; }
        public int? Alpha0 { get; }
        public int? Alpha1 { get; }
        public List<IFoothold> Footholds { get; }
    }
}
