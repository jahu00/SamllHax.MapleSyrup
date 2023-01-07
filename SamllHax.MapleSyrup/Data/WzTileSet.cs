using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Data
{
    public class WzTileSet: WzEntity
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
                Tiles.Add(tile.Id, tile);
            }
        }

        public Dictionary<string,WzTile> Tiles { get; } = new Dictionary<string,WzTile>();
    }
}
