using SamllHax.MapleSyrup.Components;
using SamllHax.MapleSyrup.Draw;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Interfaces.Interfaces.Providers;
using SkiaSharp;
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
        private readonly CommonData _commonData;

        public ComponentHelper(ResourceManager resourceManager, ObjectFactory objectFactory, CommonData commonData)
        {
            _resourceManager = resourceManager;
            _objectFactory = objectFactory;
            _commonData = commonData;
        }

        public MapInstance CreateMapInstance(int mapId, string portalName)
        {
            return _objectFactory.Create<MapInstance>().Init(mapId, portalName);
        }

        public AnimationInstance CreateAnimationInstance(object owner, IAnimation animation, DataFiles dataFile, IEnumerable<string> path)
        {
            var frameIdMappings = animation.Frames.ToDictionary(x => x.Key, x => x.Value.FramePath ?? x.Key);
            var bitmaps = _resourceManager.GetImages(owner, dataFile, path, frameIdMappings);
            var component = new AnimationInstance(animation, bitmaps);
            return component;
        }

        public MapLayerInstance CreateMapLayerInstance(MapInstance parent, IMapLayer mapLayer)
        {
            var component = _objectFactory.Create<MapLayerInstance>().Init(mapLayer, parent);
            return component;
        }

        public IDrawable CreatePortalInstance(IMapPortal mapPortal)
        {
            AnimationInstance animationInstance;
            switch (mapPortal.PortalType)
            {
                case PortalType.REGULAR:
                    animationInstance = new AnimationInstance(_commonData.PvPortalAnimation, _commonData.PvPortalBitmaps);
                    break;
                case PortalType.HIDDEN:
                    animationInstance = new AnimationInstance(_commonData.PhPortalAnimation, _commonData.PhPortalBitmaps);
                    break;
                default:
                    animationInstance = null;
                    break;
            }
            var portalInstance = new PortalInstance(mapPortal, animationInstance) { X = mapPortal.X, Y = mapPortal.Y };
            return portalInstance;
        }

        public PlayerInstance CreatePlayerInstance(IDrawable sprite, MapInstance mapInstance)
        {
            var component = _objectFactory.Create<PlayerInstance>();
            component.Sprite = sprite;
            component.MapInstance = mapInstance;
            return component;
        }
    }
}
