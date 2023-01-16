using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IFoothold: IEntity
    {
        List<IVector> Vectors { get; }
    }
}
