using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.PlatformerLogic
{
    public interface IPhysicsObject: IPoint
    {
        Speed SpeedX { get; }
        Speed SpeedY { get; }
    }

    public static class IPhysicsExtensions
    {
        public static void GetNewPosition(this IPhysicsObject physicsObject, float delta, out float newX, out float newY)
        {
            newX = physicsObject.X + physicsObject.SpeedX.GetForDelta(delta);
            newY = physicsObject.Y + physicsObject.SpeedY.GetForDelta(delta);
        }

        public static void Jump(this IPhysicsObject physicsObject, float jumpSpeed, bool retainMomentum = false)
        {
            if (Math.Sign(jumpSpeed) != -1)
            {
                jumpSpeed = -1 * jumpSpeed;
            }
            if (retainMomentum)
            {
                jumpSpeed += physicsObject.SpeedY.Value;
            }
            physicsObject.SpeedY.Set(jumpSpeed);
        }

        public static void ApplyGravity(this IPhysicsObject physicsObject, float delta, float gravityAcceleration, float maxFallSpeed)
        {
            physicsObject.SpeedY.Accelerate(delta: delta, acceleration: gravityAcceleration, maxFallSpeed, normalizeMaxSpeed: false);
        }

        public static void MoveVertically(this IPhysicsObject physicsObject, float delta, float acceleration, float maxSpeed)
        {
            physicsObject.SpeedY.Accelerate(delta: delta, acceleration: acceleration, maxSpeed, normalizeMaxSpeed: true);
        }

        public static void MoveHorizontally(this IPhysicsObject physicsObject, float delta, float acceleration, float maxSpeed)
        {
            physicsObject.SpeedX.Accelerate(delta: delta, acceleration: acceleration, maxSpeed, normalizeMaxSpeed: true);
        }
    }

}
