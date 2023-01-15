using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Data;
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

        public CommonData(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            MapHelpers = _resourceManager.GetMapHelpers(this);
        }
    }
}
