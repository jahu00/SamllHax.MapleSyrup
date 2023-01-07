using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Data
{
    public class WzObjectSubset: WzEntity
    {
        public WzObjectSubset(WzDirectory directory) : base(directory)
        {
            foreach (var partNode in _directory.Children.Cast<WzDirectory>())
            {
                var part = new WzObjectPart(partNode);
                Parts.Add(part.Id, part);
            }
        }

        public Dictionary<string,WzObjectPart> Parts { get; } = new Dictionary<string,WzObjectPart>();
    }
}
