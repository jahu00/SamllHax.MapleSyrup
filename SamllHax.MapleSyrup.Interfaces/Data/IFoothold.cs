using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    // TODO: Appears to be the same as IVectorCollection, prime suspect for refactoring.
    public interface IFoothold: IEntity
    {
        List<IVector> Vectors { get; }
    }
}
