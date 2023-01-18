using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IVectorCollection : IEntity
    {
        List<IVector> Vectors { get; }
    }
}
