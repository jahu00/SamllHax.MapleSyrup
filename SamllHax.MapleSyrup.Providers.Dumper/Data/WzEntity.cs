using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;

namespace SamllHax.MapleSyrup.Data
{
    public class WzEntity<T> : IEntity where T : WzDirectory
    {
        protected readonly T _directory;
        public WzEntity(T directory)
        {
            _directory = directory;
        }

        public string Name => _directory.Name;
    }

    public class WzEntity : WzEntity<WzDirectory>
    {
        public WzEntity(WzDirectory directory) : base(directory)
        {
        }
    }
}
