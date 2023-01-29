using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IPhysics: IEntity
    {
        float WalkForce { get; }
        float WalkSpeed { get; }
        float WalkDrag { get; }
        float SlipForce { get; }
        float SlipSpeed { get; }
        float FloatDrag1 { get; }
        float FloatDrag2 { get; }
        float FloatCoefficient { get; }
        float SwimForce { get; }
        float SwimSpeed { get; }
        float FlyForce { get; }
        float FlySpeed { get; }
        float GravityAcc { get; }
        float FallSpeed { get; }
        float JumpSpeed { get; }
        float MaxFriction { get; }
        float MinFriction { get; }
        float SwimSpeedDec { get; }
        float FlyJumpDec { get; }
    }
}
