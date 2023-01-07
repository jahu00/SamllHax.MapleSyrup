using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Data
{
    public class WzMapLayer : WzEntity
    {
        public WzMapLayer(WzDirectory directory): base(directory)
        {
            AddTiles();
        }

        private void AddTiles()
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
        }

        public string TileSetName => _directory.GetSingleChild<WzDirectory>("info").GetSingleOrDefaultChild<WzStringValue>("tS")?.Value;
        public List<WzMapTile> Tiles { get; } = new List<WzMapTile>();
        public List<WzMapObject> Objects { get; } = new List<WzMapObject>();


    }
}
