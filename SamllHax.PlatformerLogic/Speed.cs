using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.PlatformerLogic
{
    public class Speed : ISpeed
    {
        public float Value { get; private set; } = 0;

        public bool IsStopped => Value == 0;
        public int Direction => Math.Sign(Value);
        public int OppositeDirection => -1 * Math.Sign(Value);

        public void Accelerate(float delta, float acceleration, float maxSpeed, bool normalizeMaxSpeed = false)
        {
            Value = Accelerate(delta: delta, speed: Value, acceleration: acceleration, maxSpeed: maxSpeed, normalizeMaxSpeed: normalizeMaxSpeed);
        }

        public void Stop()
        {
            Value = 0;
        }

        public void Set(float newValue)
        {
            Value = newValue;
        }

        public static float Accelerate(float delta, float speed, float acceleration, float maxSpeed, bool normalizeMaxSpeed = false)
        {
            var accelerationSign = Math.Sign(acceleration);
            if (accelerationSign == 0)
            {
                return speed;
            }
            if (normalizeMaxSpeed && Math.Sign(maxSpeed) != accelerationSign)
            {
                maxSpeed *= -1;
            }

            if (accelerationSign == 1 && speed >= maxSpeed || accelerationSign == -1 && speed <= maxSpeed)
            {
                return speed;
            }
            var newSpeed = speed + delta * acceleration;
            if (accelerationSign == 1 && newSpeed < maxSpeed || accelerationSign == -1 && newSpeed > maxSpeed)
            {
                return newSpeed;
            }
            return maxSpeed;
        }
    }
}
