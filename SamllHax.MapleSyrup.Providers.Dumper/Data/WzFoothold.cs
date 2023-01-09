using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzFoothold : WzEntity, IFoothold
    {
        public WzFoothold(WzDirectory directory) : base(directory)
        {
            Vectors = _directory.Children.Select(x => (IVector)x).ToList();
        }

        public List<IVector> Vectors { get; }
    }
}
