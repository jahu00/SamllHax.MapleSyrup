using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzVectorCollection : WzEntity, IVectorCollection
    {
        public WzVectorCollection(WzDirectory directory, IEnumerable<IVector> vectors) : base(directory)
        {
            Vectors = vectors.ToList();
        }

        public string CollectionName { get; }
        public List<IVector> Vectors { get; }
    }
}
