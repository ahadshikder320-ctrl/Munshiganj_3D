using System;

namespace Munshiganj.Core
{
    /// <summary>
    /// Pure math used by <see cref="FollowCamera"/>. Kept free of UnityEngine types so the
    /// behaviour can be verified with the portable test runner (mcs + mono).
    /// </summary>
    public static class FollowMath
    {
        /// <summary>Clamps a value into the 0..1 range.</summary>
        public static float Clamp01(float value)
        {
            if (value < 0f)
            {
                return 0f;
            }

            if (value > 1f)
            {
                return 1f;
            }

            return value;
        }

        /// <summary>
        /// Interpolation factor for one frame of smooth following.
        /// Unity's Vector3.Lerp clamps the factor to 0..1, so the same clamp is applied here.
        /// </summary>
        public static float SmoothFactor(float followSpeed, float deltaTime)
        {
            if (deltaTime <= 0f)
            {
                return 0f;
            }

            return Clamp01(followSpeed * deltaTime);
        }

        /// <summary>Linear interpolation between two points.</summary>
        public static Float3 Lerp(Float3 from, Float3 to, float t)
        {
            float factor = Clamp01(t);
            return new Float3(
                from.x + ((to.x - from.x) * factor),
                from.y + ((to.y - from.y) * factor),
                from.z + ((to.z - from.z) * factor));
        }

        /// <summary>
        /// Rotates a vector around the Y axis, matching Unity's positive Y rotation
        /// (a +Z vector becomes +X at 90 degrees).
        /// </summary>
        public static Float3 RotateAroundY(Float3 value, float yawDegrees)
        {
            double radians = yawDegrees * Math.PI / 180.0;
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            return new Float3(
                (float)(value.x * cos + value.z * sin),
                value.y,
                (float)(value.z * cos - value.x * sin));
        }

        /// <summary>
        /// Camera position the follower wants to reach: the offset rotated by the target yaw
        /// and added to the target position (same result as Transform.TransformPoint).
        /// </summary>
        public static Float3 DesiredPosition(Float3 targetPosition, float targetYawDegrees, Float3 offset)
        {
            return targetPosition + RotateAroundY(offset, targetYawDegrees);
        }

        /// <summary>Point the camera looks at: the target position raised by lookHeight.</summary>
        public static Float3 LookPoint(Float3 targetPosition, float lookHeight)
        {
            return new Float3(targetPosition.x, targetPosition.y + lookHeight, targetPosition.z);
        }
    }
}
