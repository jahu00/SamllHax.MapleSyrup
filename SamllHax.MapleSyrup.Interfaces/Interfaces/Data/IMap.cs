﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IMap : IEntity
    {
        string MapMark { get; }
        List<IMapLayer> Layers { get; }
    }
}
