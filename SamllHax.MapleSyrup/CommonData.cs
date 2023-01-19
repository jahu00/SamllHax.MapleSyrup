using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Data;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup
{
    public class CommonData
    {
        private readonly ResourceManager _resourceManager;
        public IMapHelpers MapHelpers { get; private set; }
        public Dictionary<string, SKImage> PvPortalBitmaps { get; private set; }
        public IAnimation PvPortalAnimation { get; private set; }

        public Dictionary<string, SKImage> PhPortalBitmaps { get; private set; }
        public IAnimation PhPortalAnimation { get; private set; }

        public CommonData(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public void Init()
        {
            MapHelpers = _resourceManager.GetMapHelpers(this);
            var portalPathPrefix = new string[] { "MapHelper.img", "portal" };

            var pvPortalPath = new string[] { "game", "pv" };
            PvPortalAnimation = MapHelpers.Portals.GetEntityByPath(pvPortalPath);
            var pvPortalAbsolutePath = portalPathPrefix.Concat(pvPortalPath);
            PvPortalBitmaps = _resourceManager.GetImages(this, Interfaces.Interfaces.Providers.DataFiles.Map, pvPortalAbsolutePath, PvPortalAnimation.Frames.Keys);

            var phPortalPath = new string[] { "game", "ph", "default", "portalContinue" };
            PhPortalAnimation = MapHelpers.Portals.GetEntityByPath(phPortalPath);
            var phPortalAbsolutePath = portalPathPrefix.Concat(phPortalPath);
            PhPortalBitmaps = _resourceManager.GetImages(this, Interfaces.Interfaces.Providers.DataFiles.Map, phPortalAbsolutePath, PhPortalAnimation.Frames.Keys);
        }
    }
}
