using SamllHax.MapleSyrup.Data;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup
{
    public class MapRenderer
    {
        private ResourceManager _resourceManager;

        public MapRenderer(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public SKBitmap Render(WzMap map, int width, int height, int x, int y)
        {
            var bitmap = new SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
            var canvas = new SKCanvas(bitmap);
            foreach (var layer in map.Layers)
            {
                var objectSprites = new SpriteCollection();
                foreach (var obj in layer.Objects)
                {
                    var objectGroupData = _resourceManager.GetObjectGroup(obj.GroupName);
                    var objectData = objectGroupData.Objects[obj.Name];
                    var objectSubsetData = objectData.Subsets[obj.SubsetName];
                    var objectPartData = objectSubsetData.Parts[obj.PartId];
                    var objectPartFrameData = objectPartData.Frames.Values.First();
                    var objectBitmap = _resourceManager.GetObjectImage(obj.GroupName, obj.Name, obj.SubsetName, obj.PartId, objectPartFrameData.Id);
                    objectSprites.Add(new Sprite() { Bitmap = objectBitmap, X = obj.X - objectPartFrameData.Origin.X, Y = obj.Y - objectPartFrameData.Origin.Y, Z = obj.Z });
                    //canvas.DrawBitmap(objectBitmap, obj.X - objectPartFrameData.Origin.X + x, obj.Y - objectPartFrameData.Origin.Y + y);
                }
                foreach (var sprite in objectSprites.GetOrderdSprites())
                {
                    canvas.DrawBitmap(sprite.Bitmap, sprite.X + x, sprite.Y + y);
                }
                var tileSprites = new SpriteCollection();
                if (layer.TileSetName == null || layer.Tiles.Count == 0)
                {
                    continue;
                }
                var tileSet = _resourceManager.GetTileSet(layer.TileSetName);
                foreach (var tile in layer.Tiles)
                {
                    var tileBitmap = _resourceManager.GetTileImage(layer.TileSetName, tile.Name, tile.Variant);
                    var tileData = tileSet.Tiles[tile.Name].Variants[tile.Variant];
                    //var z = tile.Z
                    tileSprites.Add(new Sprite() { Bitmap = tileBitmap, X = tile.X - tileData.Origin.X, Y = tile.Y - tileData.Origin.Y, Z = tile.Z });
                    //canvas.DrawBitmap(tileBitmap, tile.X - tileData.Origin.X + x, tile.Y - tileData.Origin.Y + y);
                }
                foreach (var sprite in tileSprites.GetOrderdSprites())
                {
                    canvas.DrawBitmap(sprite.Bitmap, sprite.X + x, sprite.Y + y);
                }
            }

            return bitmap;
        }
    }
}
