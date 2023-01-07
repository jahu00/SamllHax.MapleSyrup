using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Data
{
    public class WzObject : WzEntity
    {
        public WzObject(WzDirectory directory) : base(directory)
        {
            foreach (var subsetNode in _directory.Children.Cast<WzDirectory>())
            {
                var subset = new WzObjectSubset(subsetNode);
                Subsets.Add(subset.Id, subset);
            }
        }

        public Dictionary<string, WzObjectSubset> Subsets { get; } = new Dictionary<string, WzObjectSubset>();
    }
}
