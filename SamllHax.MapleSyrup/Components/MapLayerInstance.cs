﻿using SamllHax.MapleSyrup.Draw;
using SamllHax.MapleSyrup.Helpers;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Interfaces.Interfaces.Providers;
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
            var drawables = mapLayer.Tiles.OrderBy(x => x.Z).Select
            (
                tile =>
                {
                    var tileBitmap = _resourceManager.GetTileImage(this, mapLayer.TileSetName, tile.Path);
                    var tileData = tileSet.GetEntityByPath(tile.Path);
                    return (IDrawable)new Sprite() { Bitmap = tileBitmap, X = tile.X, OriginX = tileData.Origin.X, Y = tile.Y, OriginY = tileData.Origin.Y };
                }
            ).ToList();
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
                    return (IDrawable)_componentHelper.CreateAnimationInstance
                    (
                        owner: Parent,
                        animation: objectData,
                        dataFile: DataFiles.Map,
                        path: obj.GetFullPath(),
                        x: obj.X,
                        y: obj.Y
                    );
                }
            ).ToList();
            return new DrawableCollection(drawables);
        }
    }
}