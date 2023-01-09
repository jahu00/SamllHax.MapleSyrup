using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SamllHax.MapleSyrup.Interfaces.Data;

namespace SamllHax.MapleSyrup.Data
{
    public class EntityDirectory<T> : EntityBase, IEntityDirectory<T> where T : IEntity
    {
        public IDictionary<string, T> Entities { get; set; }

        public IDictionary<string, IEntityDirectory<T>> Directories { get; set; }
    }
}
