using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzTileSet: WzEntity, IEntityDirectory<IFrame>
    {
        public WzTileSet(WzDirectory directory): base(directory)
        {
            foreach(var tileNode in _directory.Children)
            {
                if (tileNode.Name == "info")
                {
                    continue;
                }
                var tile = new WzTile((WzDirectory)tileNode);
                Directories.Add(tile.Name, tile);
            }
        }

        public IDictionary<string, IFrame> Entities { get; } = new Dictionary<string, IFrame>();

        public IDictionary<string, IEntityDirectory<IFrame>> Directories { get; } = new Dictionary<string, IEntityDirectory<IFrame>>();
    }
}
