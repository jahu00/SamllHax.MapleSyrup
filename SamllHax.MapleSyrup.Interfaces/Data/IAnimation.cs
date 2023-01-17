using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IAnimation : IEntity
    {
        Dictionary<string, IFrame> Frames { get; }
        List<IVector> Seat { get; }
        List<IVector> Foothold { get; }
        List<IVector> Ladder { get; }
        List<IVector> Rope { get; }
        int? Blend { get; }
        int? ZigZag { get; }
    }
}
