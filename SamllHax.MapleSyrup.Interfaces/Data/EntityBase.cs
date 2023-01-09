using System;
using System.Collections.Generic;
using System.Text;
using SamllHax.MapleSyrup.Interfaces.Data;

namespace SamllHax.MapleSyrup.Data
{
    public abstract class EntityBase : IEntity
    {
        public string Name { get; set; }
    }
}
