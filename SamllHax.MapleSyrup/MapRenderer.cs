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
            var matrix = SKMatrix.CreateTranslation(x, y);
            var bitmap = new SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
            var canvas = new SKCanvas(bitmap);
            foreach (var layer in map.Layers)
            {
                var objectSprites = new List<IDrawable>();
                foreach (var obj in layer.Objects.OrderBy(x=> x.Z))
                {
                    var objectGroupData = _resourceManager.GetObjectDirectory(this, obj.DirectoryName);
                    var objectData = objectGroupData.GetEntityByPath(obj.Path);
                    var objectPartFrameData = objectData.Frames.Values.First();
                    var objectBitmap = _resourceManager.GetObjectImage(this, obj.DirectoryName, obj.Path, objectPartFrameData.Name);
                    objectSprites.Add(new Sprite() { Image = objectBitmap, X = obj.X - objectPartFrameData.Origin.X, Y = obj.Y - objectPartFrameData.Origin.Y });
                }
                new DrawableCollection(objectSprites).Draw(canvas, matrix);

                var tileSprites = new List<IDrawable>();
                if (layer.TileSetName == null || layer.Tiles.Count == 0)
                {
                    continue;
                }
                var tileSet = _resourceManager.GetTileSet(this, layer.TileSetName);
                foreach (var tile in layer.Tiles.OrderBy(x => x.Z))
                {
                    var tileBitmap = _resourceManager.GetTileImage(this, layer.TileSetName, tile.Path);
                    var tileData = tileSet.GetEntityByPath(tile.Path);
                    //var z = tile.Z
                    tileSprites.Add(new Sprite() { Image = tileBitmap, X = tile.X - tileData.Origin.X, Y = tile.Y - tileData.Origin.Y });
                    //canvas.DrawBitmap(tileBitmap, tile.X - tileData.Origin.X + x, tile.Y - tileData.Origin.Y + y);
                }
                new DrawableCollection(tileSprites).Draw(canvas, matrix);
            }

            return bitmap;
        }
    }
}
