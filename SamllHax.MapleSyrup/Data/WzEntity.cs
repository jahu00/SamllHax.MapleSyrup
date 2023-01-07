using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Data
{
    public class WzEntity<T> where T : WzDirectory
    {
        protected readonly T _directory;
        public WzEntity(T directory)
        {
            _directory = directory;
        }

        public string Id => _directory.Name;
    }

    public class WzEntity : WzEntity<WzDirectory>
    {
        public WzEntity(WzDirectory directory) : base(directory)
        {
        }
    }
}
