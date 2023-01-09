using SamllHax.MapleSyrup.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IFrame : IEntity
    {
        int Width { get; }
        int Height { get; }
        IVector Origin { get; }
        int? Z { get; }
        int? Delay { get; }
        int? Alpha0 { get; }
        int? Alpha1 { get; }

        /// <summary>
        /// Foothold as used by tiles
        /// </summary>
        List<IFoothold> Footholds { get; }
    }
}
