using System;
using System.Collections.Generic;
using System.Text;
using SamllHax.MapleSyrup.Interfaces.Data;

namespace SamllHax.MapleSyrup.Data
{
    public class Vector : IVector
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
