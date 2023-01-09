using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;

namespace SamllHax.MapleSyrup.Data
{
    public class WzMapLayer : WzEntity, IMapLayer
    {
        public WzMapLayer(WzDirectory directory): base(directory)
        {
            var tilesDirectory = _directory.GetSingleChild<WzDirectory>("tile");
            foreach (var tileNode in tilesDirectory.Children.Cast<WzDirectory>())
            {
                var tile = new WzMapTile(tileNode);
                Tiles.Add(tile);
            }

            var objectsDirectory = _directory.GetSingleChild<WzDirectory>("obj");
            foreach (var objectNode in objectsDirectory.Children.Cast<WzDirectory>())
            {
                var obj = new WzMapObject(objectNode);
                Objects.Add(obj);
            }

            TileSetName = _directory.GetSingleChild<WzDirectory>("info").GetSingleOrDefaultChild<WzStringValue>("tS")?.Value;
        }

        public string TileSetName { get; }
        public List<IMapTile> Tiles { get; } = new List<IMapTile>();
        public List<IMapObject> Objects { get; } = new List<IMapObject>();


    }
}
