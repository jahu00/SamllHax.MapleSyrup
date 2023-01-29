using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IMapEntityBase : IEntity
    {
        int X { get; }
        int Y { get; }
    }
}
