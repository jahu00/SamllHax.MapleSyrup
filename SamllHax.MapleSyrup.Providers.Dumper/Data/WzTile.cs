using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzTile : WzEntity, IEntityDirectory<IFrame>
    {
        public WzTile(WzDirectory directory) : base(directory)
        {
            foreach(var tileVariantNode in _directory.Children)
            {
                var tileVariant = new WzFrame((WzCanvas)tileVariantNode);
                Entities.Add(tileVariant.Name, tileVariant);
            }
        }

        public IDictionary<string, IFrame> Entities { get; } = new Dictionary<string, IFrame>();

        public IDictionary<string, IEntityDirectory<IFrame>> Directories { get; } = new Dictionary<string, IEntityDirectory<IFrame>>();
    }
}
