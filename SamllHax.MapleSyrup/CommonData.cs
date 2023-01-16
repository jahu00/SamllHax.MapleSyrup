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

        public CommonData(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            MapHelpers = _resourceManager.GetMapHelpers(this);
            var pvPortalPath = new string[] { "game", "pv" };
            PvPortalAnimation = MapHelpers.Portals.GetEntityByPath(pvPortalPath);
            var pvPortalAbsolutePath = (new string[] { "MapHelper.img", "portal" }).Concat(pvPortalPath);

            PvPortalBitmaps = _resourceManager.GetImages(this, Interfaces.Interfaces.Providers.DataFiles.Map, pvPortalAbsolutePath, PvPortalAnimation.Frames.Keys);
        }
    }
}
