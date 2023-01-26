using OpenTK.Windowing.Common;
using SamllHax.MapleSyrup.Draw;
using SamllHax.MapleSyrup.Helpers;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Interfaces.Interfaces.Providers;
using SamllHax.PlatformerLogic;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Components
{
    public class MapLayerInstance: DrawableCollection, IBoundable, IUpdatable
    {
        private readonly ResourceManager _resourceManager;
        private readonly ComponentHelper _componentHelper;

        public MapInstance Parent { get; private set; }

        public DrawableCollection Tiles { get; private set; }
        public DrawableCollection Objects { get; private set; }

        private SKRectI? BoudingBox { get; set; }

        public MapLayerInstance(ResourceManager resourceManager, ComponentHelper componentHelper)
        {
            _resourceManager = resourceManager;
            _componentHelper = componentHelper;
        }

        public MapLayerInstance Init(IMapLayer mapLayer, MapInstance parent)
        {
            Parent = parent;
            Objects = GetObjects(mapLayer);
            Tiles = GetTiles(mapLayer);
            Children.Add(Objects);
            if (Tiles != null)
            {
                Children.Add(Tiles);
            }
            return this;
        }

        private DrawableCollection GetTiles (IMapLayer mapLayer)
        {
            if (mapLayer.TileSetName == null)
            {
                return null;
            }
            var tileSet = _resourceManager.GetTileSet(this, mapLayer.TileSetName);
            var drawables = mapLayer.Tiles.Select
            (
                tile =>
                {
                    var tileBitmap = _resourceManager.GetTileImage(this, mapLayer.TileSetName, tile.Path);
                    var tileData = tileSet.GetEntityByPath(tile.Path);
                    return new TileInstance() { Image = tileBitmap, X = tile.X, OriginX = tileData.Origin.X, Y = tile.Y, OriginY = tileData.Origin.Y, Z = tileData.Z ?? 0 };
                }
            ).OrderBy(x => x.Z);
            return new DrawableCollection(drawables);
        }

        private DrawableCollection GetObjects(IMapLayer mapLayer)
        {
            var drawables = mapLayer.Objects.OrderBy(x => x.Z).Select
            (
                obj =>
                {
                    var objectDirectory = _resourceManager.GetObjectDirectory(this, obj.DirectoryName);
                    var objectData = objectDirectory.GetEntityByPath(obj.Path);
                    var component = _componentHelper.CreateAnimationInstance
                    (
                        owner: Parent,
                        animation: objectData,
                        dataFile: DataFiles.Map,
                        path: obj.GetFullPath()
                    );
                    component.X = obj.X;
                    component.Y = obj.Y;
                    if (obj.FlipX)
                    {
                        component.ScaleX = -1;
                    }
                    return component;
                }
            );
            return new DrawableCollection(drawables);
        }

        public override void OnUpdate(UpdateEvents events)
        {
            Objects.Update(events);
        }

        public override SKRectI GetBoundingBox()
        {
            if (BoudingBox == null)
            {
                BoudingBox = base.GetBoundingBox();
            }
            return BoudingBox.Value;
        }
    }
}
