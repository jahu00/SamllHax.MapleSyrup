using System;
using System.Collections.Generic;
using System.Text;
using SamllHax.MapleSyrup.Interfaces.Data;

namespace SamllHax.MapleSyrup.Data
{
    public class Frame : EntityBase, IFrame
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public IVector Origin { get; set; }

        public int Z { get; set; }

        public int? Delay { get; set; }

        public int? Alpha0 { get; set; }

        public int? Alpha1 { get; set; }

        public List<IVector> Foothold { get; set; }

    }
}
