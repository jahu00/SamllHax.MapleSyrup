﻿using SamllHax.MapleSyrup.Components;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Interfaces.Interfaces.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Helpers
{
    public class ComponentHelper
    {
        private readonly ResourceManager _resourceManager;
        private readonly ObjectFactory _objectFactory;

        public ComponentHelper(ResourceManager resourceManager, ObjectFactory objectFactory)
        {
            _resourceManager = resourceManager;
            _objectFactory = objectFactory;
        }

        public AnimationInstance CreateAnimationInstance(object owner, IAnimation animation, DataFiles dataFile, IEnumerable<string> path, int x, int y)
        {
            var frameIds = animation.Frames.Select(x => x.Key).ToArray();
            var bitmaps = _resourceManager.GetImages(owner, dataFile, path, frameIds);
            var component = new AnimationInstance(animation, bitmaps) { X = x, Y = y };
            return component;
        }

        public MapLayerInstance CreateMapLayerInstance(MapInstance parent, IMapLayer mapLayer)
        {
            var component = _objectFactory.Create<MapLayerInstance>().Init(mapLayer, parent);
            return component;
        }
    }
}
