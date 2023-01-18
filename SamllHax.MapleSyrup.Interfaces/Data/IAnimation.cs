using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IAnimation : IEntity
    {
        Dictionary<string, IFrame> Frames { get; }
        List<IVectorCollection> Seat { get; }
        List<IVectorCollection> Foothold { get; }
        List<IVectorCollection> Ladder { get; }
        List<IVectorCollection> Rope { get; }
        int? Blend { get; }
        int? ZigZag { get; }
        IVector Origin { get; }
    }
}
