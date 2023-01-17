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
        public IMapHelpers MapHelpers { get; }
        public Dictionary<string, SKBitmap> PvPortalBitmaps { get; }
        public IAnimation PvPortalAnimation { get; }

        public Dictionary<string, SKBitmap> PhPortalBitmaps { get; }
        public IAnimation PhPortalAnimation { get; }

        public CommonData(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
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
