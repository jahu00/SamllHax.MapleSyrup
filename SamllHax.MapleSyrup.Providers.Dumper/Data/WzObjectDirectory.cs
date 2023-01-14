using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzAnimationDirectory : WzEntity, IEntityDirectory<IAnimation>
    {
        public WzAnimationDirectory(WzDirectory directory, int watchdog = 255) : base(directory)
        {
            watchdog--;
            if (watchdog <= 0)
            {
                throw new Exception("Watchdog triggered");
            }
            foreach (var objectNode in _directory.Children.Cast<WzDirectory>())
            {
                if (objectNode.Children.Any(x => x is WzCanvas))
                {
                    var obj = new WzAnimation(objectNode);
                    Entities.Add(obj.Name, obj);
                    continue;
                }

                var entityDirectory = new WzAnimationDirectory(objectNode);
                Directories.Add(entityDirectory.Name, entityDirectory);
            }
        }

        public IDictionary<string, IAnimation> Entities { get; } = new Dictionary<string, IAnimation>();

        public IDictionary<string, IEntityDirectory<IAnimation>> Directories { get; } = new Dictionary<string, IEntityDirectory<IAnimation>>();
    }
}
