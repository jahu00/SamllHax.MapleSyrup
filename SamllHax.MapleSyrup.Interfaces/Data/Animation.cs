using SamllHax.MapleSyrup.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Data
{
    public class Animation : EntityBase, IAnimation
    {
        public Dictionary<string, IFrame> Frames { get; set; }

        public List<IVector> Seat { get; set; }

        public List<IVector> Foothold { get; set; }

        public int? Blend { get; set; }

        public int? ZigZag { get; set; }
    }
}
