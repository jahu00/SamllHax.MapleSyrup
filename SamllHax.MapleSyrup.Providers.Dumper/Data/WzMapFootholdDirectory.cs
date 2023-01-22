using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzMapFootholdDirectory : WzEntity, IEntityDirectory<IMapFoothold>
    {
        public WzMapFootholdDirectory(WzDirectory directory): base(directory)
        {
            directory.Children.ForEach
            (
                node =>
                {
                    var directoryNode = node as WzDirectory;
                    if (directoryNode == null)
                    {
                        throw new Exception($"Node is not a WzDirectory, but {node.GetType().Name}");
                    }
                    if (directoryNode.Children.Any(x => x is WzIntValue))
                    {
                        var mapFoothold = new WzMapFoothold(directoryNode);
                        Entities.Add(mapFoothold.Name, mapFoothold);
                        return;
                    }
                    var mapFootholdDirectory = new WzMapFootholdDirectory(directoryNode);
                    Directories.Add(mapFootholdDirectory.Name, mapFootholdDirectory);
                }
            );
        }

        public IDictionary<string, IMapFoothold> Entities { get; } = new Dictionary<string, IMapFoothold>();

        public IDictionary<string, IEntityDirectory<IMapFoothold>> Directories { get; } = new Dictionary<string, IEntityDirectory<IMapFoothold>>();
    }
}
