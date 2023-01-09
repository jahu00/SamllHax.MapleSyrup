using SamllHax.MapleSyrup.Draw;
using SamllHax.MapleSyrup.Interfaces.Data;
using SkiaSharp;

namespace SamllHax.MapleSyrup
{
    public class MapRenderer
    {
        private ResourceManager _resourceManager;

        public MapRenderer(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public SKBitmap Render(IMap map, int width, int height, int x, int y)
        {
            var bitmap = new SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
            var canvas = new SKCanvas(bitmap);
            foreach (var layer in map.Layers)
            {
                var objectSprites = new List<IDrawable>();
                foreach (var obj in layer.Objects.OrderBy(x=> x.Z))
                {
                    var objectGroupData = _resourceManager.GetObjectDirectory(obj.DirectoryName);
                    var objectData = objectGroupData.GetEntityByPath(obj.Path);
                    var objectPartFrameData = objectData.Frames.Values.First();
                    var objectBitmap = _resourceManager.GetObjectImage(obj.DirectoryName, obj.Path, objectPartFrameData.Name);
                    objectSprites.Add(new Sprite() { Bitmap = objectBitmap, X = obj.X - objectPartFrameData.Origin.X, Y = obj.Y - objectPartFrameData.Origin.Y });
                }
                new SpriteCollection() { Sprites = objectSprites }.Draw(canvas, x, y);

                var tileSprites = new List<IDrawable>();
                if (layer.TileSetName == null || layer.Tiles.Count == 0)
                {
                    continue;
                }
                var tileSet = _resourceManager.GetTileSet(layer.TileSetName);
                foreach (var tile in layer.Tiles.OrderBy(x => x.Z))
                {
                    var tileBitmap = _resourceManager.GetTileImage(layer.TileSetName, tile.Path);
                    var tileData = tileSet.GetEntityByPath(tile.Path);
                    //var z = tile.Z
                    tileSprites.Add(new Sprite() { Bitmap = tileBitmap, X = tile.X - tileData.Origin.X, Y = tile.Y - tileData.Origin.Y });
                    //canvas.DrawBitmap(tileBitmap, tile.X - tileData.Origin.X + x, tile.Y - tileData.Origin.Y + y);
                }
                new SpriteCollection() { Sprites = tileSprites }.Draw(canvas, x, y);
            }

            return bitmap;
        }
    }
}
