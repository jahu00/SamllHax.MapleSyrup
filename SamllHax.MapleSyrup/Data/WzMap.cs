using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Data
{
    public class WzMap: WzEntity
    {
        public WzMap(WzDirectory directory): base(directory)
        {
            for (var i = 0; i < 8; i++)
            {
                var mapLayerDirectory = _directory.GetSingleChild<WzDirectory>(i.ToString());
                var mapLayer = new WzMapLayer(mapLayerDirectory);
                Layers.Add(mapLayer);
            }
        }

        public string MapMark => _directory.GetSingleChild<WzDirectory>("info").GetSingleChild<WzStringValue>("mapMark").Value;
        public List<WzMapLayer> Layers { get; } = new List<WzMapLayer>();

    }
}
