using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzMapHelpers : WzEntity
    {
        public WzMapHelpers(WzDirectory directory) : base(directory)
        {
            Portals = new WzAnimationDirectory(directory.GetSingleChild<WzDirectory>("portal"));
        }

        IEntityDirectory<IAnimation> Portals { get; set; }
    }
}
