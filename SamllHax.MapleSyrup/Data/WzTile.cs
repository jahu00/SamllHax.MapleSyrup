using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Data
{
    public class WzTile : WzEntity
    {
        public WzTile(WzDirectory directory) : base(directory)
        {
            foreach(var tileVariantNode in _directory.Children)
            {
                var tileVariant = new WzFrame((WzCanvas)tileVariantNode);
                var tileVariantId = Convert.ToInt32(tileVariant.Id);
                Tiles.Add(tileVariantId, tileVariant);
            }
        }

        public Dictionary<int, WzFrame> Tiles { get; } = new Dictionary<int, WzFrame>();
        public string Name { get; set; }
    }
}
