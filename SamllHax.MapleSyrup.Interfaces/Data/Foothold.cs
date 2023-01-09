using SamllHax.MapleSyrup.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Data
{
    public class Foothold : EntityBase, IFoothold
    {
        public List<IVector> Vectors { get; set; }
    }
}
