using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Data
{
    public class WzObjectGroup : WzEntity
    {
        public WzObjectGroup(WzDirectory directory) : base(directory)
        {
            foreach (var objectNode in _directory.Children.Cast<WzDirectory>())
            {
                var obj = new WzObject(objectNode);
                Objects.Add(obj.Id, obj);
            }
        }

        public Dictionary<string, WzObject> Objects { get; } = new Dictionary<string, WzObject>();


    }
}
