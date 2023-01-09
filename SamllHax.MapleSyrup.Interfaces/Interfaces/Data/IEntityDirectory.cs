using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IEntityDirectory<TEntity> : IEntity where TEntity : IEntity
    {
        IDictionary<string, TEntity> Entities { get; }
        IDictionary<string, IEntityDirectory<TEntity>> Directories { get; }
    }
}
