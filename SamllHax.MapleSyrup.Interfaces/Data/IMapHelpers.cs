using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IMapHelpers : IEntity
    {
        IEntityDirectory<IAnimation> Portals { get; }
    }
}
