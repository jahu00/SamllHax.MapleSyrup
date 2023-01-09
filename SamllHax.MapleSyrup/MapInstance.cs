using SamllHax.MapleSyrup.Data;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup
{
    public class MapInstance: DrawableBase, IDrawable, IUpdatable
    {
        private readonly ResourceManager _resourceManager;
        private readonly WzMap _map;
        private readonly SpriteCollection _layers;
        public SKRect BoudingBox { get; private set; }

        public MapInstance(ResourceManager resourceManager, int mapId)
        {
            _resourceManager = resourceManager;
            _map = _resourceManager.GetMap(mapId);
            _layers = new SpriteCollection() { Sprites = BuildLayers(_map.Layers) };
            BoudingBox = _layers.GetBoundingBox(0, 0);

        }

        private List<IDrawable> BuildLayers(IEnumerable<WzMapLayer> mapLayers)
        {
            var layers = mapLayers.Select(layer => BuildLayer(layer)).ToList();
            return layers;
        }

        private IDrawable BuildLayer(WzMapLayer layer)
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

        private IDrawable GetTileSprites(WzMapLayer layer)
        {
            // TODO: Replace by mesh
            var tileSet = _resourceManager.GetTileSet(layer.TileSetName);
            var sprites = layer.Tiles.OrderBy(x => x.Z).Select
            (
                tile =>
                {
                    var tileBitmap = _resourceManager.GetTileImage(layer.TileSetName, tile.Name, tile.Variant);
                    var tileData = tileSet.Tiles[tile.Name].Variants[tile.Variant];
                    return (IDrawable)new Sprite() { Bitmap = tileBitmap, X = tile.X - tileData.Origin.X, Y = tile.Y - tileData.Origin.Y };
                }
            ).ToList();
            return new SpriteCollection(){ Sprites = sprites };
        }

        private IDrawable GetObjectSprites(WzMapLayer layer)
        {
            var sprites = layer.Objects.OrderBy(x => x.Z).Select
            (
                obj =>
                {
                    var objectGroupData = _resourceManager.GetObjectGroup(obj.GroupName);
                    var objectData = objectGroupData.Objects[obj.Name];
                    var objectSubsetData = objectData.Subsets[obj.SubsetName];
                    var objectPartData = objectSubsetData.Parts[obj.PartId];
                    if (objectPartData.Frames.Count == 1)
                    {
                        var frameId = objectPartData.Frames.Keys.First();
                        return GetObjectFrame(obj, objectPartData, frameId) as IDrawable;
                    }
                    return GetAnimatedObject(obj, objectPartData) as IDrawable;
                }
            ).ToList();
            return new SpriteCollection() { Sprites = sprites };
        }

        private Sprite GetObjectFrame(WzMapObject obj, WzObjectPart objectPartData, string frameId)
        {
            var frameData = objectPartData.Frames[frameId];
            return GetObjectFrame(obj, frameData);
        }

        private Sprite GetObjectFrame(WzMapObject obj, WzFrame frameData)
        {
            var objectBitmap = _resourceManager.GetObjectImage(obj.GroupName, obj.Name, obj.SubsetName, obj.PartId, frameData.Id);
            return new Sprite() { Bitmap = objectBitmap, X = obj.X - frameData.Origin.X, Y = obj.Y - frameData.Origin.Y };
        }

        private AnimatedSprite GetAnimatedObject(WzMapObject obj, WzObjectPart objectPartData)
        {
            var frames = objectPartData.Frames.OrderBy(x => Convert.ToInt32(x.Key)).Select(x => x.Value).Select
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
