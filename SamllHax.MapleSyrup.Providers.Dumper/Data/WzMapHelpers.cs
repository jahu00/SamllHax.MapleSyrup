using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzMapHelpers : WzEntity, IMapHelpers
    {
        public WzMapHelpers(WzDirectory directory) : base(directory)
        {
            Portals = new WzAnimationDirectory(directory.GetSingleChild<WzDirectory>("portal"));
        }

        public IEntityDirectory<IAnimation> Portals { get; set; }
    }
}
