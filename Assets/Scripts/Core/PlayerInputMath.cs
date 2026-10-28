using System;

namespace Munshiganj.Core
{
    /// <summary>
    /// Turns raw input axes into a movement direction that is relative to the player yaw.
    /// Engine independent so it can be unit tested without the Unity Editor.
    /// </summary>
    public static class PlayerInputMath
    {
        /// <summary>
        /// Converts horizontal (strafe) and vertical (forward/back) axes into a normalized
        /// world space direction.
        /// </summary>
        public static Float3 MoveDirection(float horizontal, float vertical, float yawDegrees)
        {
            if (float.IsNaN(horizontal) || float.IsNaN(vertical))
            {
                throw new ArgumentException("Input axes must be valid numbers.");
            }

            Float3 forward = FollowMath.RotateAroundY(Float3.Forward, yawDegrees);
            Float3 right = FollowMath.RotateAroundY(Float3.Right, yawDegrees);

            Float3 direction = (forward * vertical) + (right * horizontal);
            return Float3.Normalize(direction);
        }
    }
}
