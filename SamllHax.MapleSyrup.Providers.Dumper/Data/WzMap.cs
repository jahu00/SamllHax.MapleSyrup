using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzMap: WzEntity, IMap
    {
        public WzMap(WzDirectory directory): base(directory)
        {
            for (var i = 0; i < 8; i++)
            {
                var mapLayerDirectory = _directory.GetSingleChild<WzDirectory>(i.ToString());
                var mapLayer = new WzMapLayer(mapLayerDirectory);
                Layers.Add(mapLayer);
            }
            Portals = _directory.GetSingleChild<WzDirectory>("portal").Children.Cast<WzDirectory>().Select(x => (IMapPortal)new WzMapPortal(x)).ToList();
            MapMark = _directory.GetSingleChild<WzDirectory>("info").GetSingleChild<WzStringValue>("mapMark").Value;
        }

        public string MapMark { get; }
        public List<IMapLayer> Layers { get; } = new List<IMapLayer>();
        public List<IMapPortal> Portals { get; } = new List<IMapPortal>();

    }
}
