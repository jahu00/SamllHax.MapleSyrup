using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IMapLadder: IEntity
    {
        LadderType Type { get; }
        bool CanExitUp { get; }
        int X { get; }
        int Y1 { get; }
        int Y2 { get; }
        int LayerId { get; }
    }
}
