using SamllHax.MapleSyrup.Draw;
using SamllHax.MapleSyrup.Interfaces.Data;
using SkiaSharp;

namespace SamllHax.MapleSyrup
{
    public class MapInstance: DrawableBase, IDrawable, IUpdatable
    {
        private readonly ResourceManager _resourceManager;
        private readonly IMap _map;
        private readonly SpriteCollection _layers;
        public SKRect BoudingBox { get; private set; }

        public MapInstance(ResourceManager resourceManager, int mapId)
        {
            _resourceManager = resourceManager;
            _map = _resourceManager.GetMap(mapId);
            _layers = new SpriteCollection() { Sprites = BuildLayers(_map.Layers) };
            BoudingBox = _layers.GetBoundingBox(0, 0);

        }

        private List<IDrawable> BuildLayers(IEnumerable<IMapLayer> mapLayers)
        {
            var layers = mapLayers.Select(layer => BuildLayer(layer)).ToList();
            return layers;
        }

        private IDrawable BuildLayer(IMapLayer layer)
        {
            var sprites = new List<IDrawable>();
            if (layer.Objects.Count > 0)
            {
                var objectSprites = GetObjectSprites(layer);
                sprites.Add(objectSprites);
            }
            if (layer.TileSetName != null)
            {
                var tileSprites = GetTileSprites(layer);
                sprites.Add(tileSprites);
            }
            return new SpriteCollection(){ Sprites = sprites };
        }

        private IDrawable GetTileSprites(IMapLayer layer)
        {
            // TODO: Replace by mesh
            var tileSet = _resourceManager.GetTileSet(layer.TileSetName);
            var sprites = layer.Tiles.OrderBy(x => x.Z).Select
            (
                tile =>
                {
                    var tileBitmap = _resourceManager.GetTileImage(layer.TileSetName, tile.Path);
                    var tileData = tileSet.GetEntityByPath(tile.Path);
                    return (IDrawable)new Sprite() { Bitmap = tileBitmap, X = tile.X - tileData.Origin.X, Y = tile.Y - tileData.Origin.Y };
                }
            ).ToList();
            return new SpriteCollection(){ Sprites = sprites };
        }

        private IDrawable GetObjectSprites(IMapLayer layer)
        {
            var sprites = layer.Objects.OrderBy(x => x.Z).Select
            (
                obj =>
                {
                    var objectDirectory = _resourceManager.GetObjectDirectory(obj.DirectoryName);
                    var objectData = objectDirectory.GetEntityByPath(obj.Path);
                    if (objectData.Frames.Count == 1)
                    {
                        var frameId = objectData.Frames.Keys.First();
                        return GetObjectFrame(obj, objectData, frameId) as IDrawable;
                    }
                    return GetAnimatedObject(obj, objectData) as IDrawable;
                }
            ).ToList();
            return new SpriteCollection() { Sprites = sprites };
        }

        private Sprite GetObjectFrame(IMapObject obj, IAnimation objectPartData, string frameId)
        {
            var frameData = objectPartData.Frames[frameId];
            return GetObjectFrame(obj, frameData);
        }

        private Sprite GetObjectFrame(IMapObject obj, IFrame frameData)
        {
            var objectBitmap = _resourceManager.GetObjectImage(obj.DirectoryName, obj.Path, frameData.Name);
            return new Sprite() { Bitmap = objectBitmap, X = obj.X - frameData.Origin.X, Y = obj.Y - frameData.Origin.Y };
        }

        private AnimatedSprite GetAnimatedObject(IMapObject obj, IAnimation objectData)
        {
            var frames = objectData.Frames.OrderBy(x => Convert.ToInt32(x.Key)).Select(x => x.Value).Select
            (
                (frameData) => new Frame()
                {
                    Delay = frameData.Delay.Value,
                    Sprite = GetObjectFrame(obj, frameData)
                }
            ).ToList();
            return new AnimatedSprite() { Frames = frames };
        }

        public void Draw(SKCanvas canvas, int x, int y)
        {
            _layers.Draw(canvas, X + x, Y + y);
        }

        public void Update(int delta)
        {
            _layers.Update(delta);
        }
    }
}
