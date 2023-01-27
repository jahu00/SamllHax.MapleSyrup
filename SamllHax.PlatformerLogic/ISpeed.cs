using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.PlatformerLogic
{
    public interface ISpeed
    {
        float Value { get; }
    }

    public static class ISpeedExtensions
    {
        public static float GetForDelta(this ISpeed speed, float delta)
        {
            return speed.Value * delta;
        }
    }
}
