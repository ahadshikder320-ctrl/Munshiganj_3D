using System;

namespace Munshiganj.Core
{
    /// <summary>
    /// Axis aligned box collision helpers used by the player against the obstacles.
    /// Engine independent so the behaviour can be unit tested without the Unity Editor.
    /// </summary>
    public static class CollisionMath
    {
        /// <summary>True when the point is inside (or on the border of) the box.</summary>
        public static bool OverlapsBox(Float3 point, Float3 boxMin, Float3 boxMax)
        {
            return point.x >= boxMin.x && point.x <= boxMax.x &&
                   point.y >= boxMin.y && point.y <= boxMax.y &&
                   point.z >= boxMin.z && point.z <= boxMax.z;
        }

        /// <summary>Closest point of the box to the given point.</summary>
        public static Float3 ClosestPointOnBox(Float3 point, Float3 boxMin, Float3 boxMax)
        {
            return new Float3(
                Clamp(point.x, boxMin.x, boxMax.x),
                Clamp(point.y, boxMin.y, boxMax.y),
                Clamp(point.z, boxMin.z, boxMax.z));
        }

        /// <summary>
        /// True when a circle (the player) intersects the box (an obstacle).
        /// </summary>
        public static bool OverlapsCircleBox(Float3 center, float radius, Float3 boxMin, Float3 boxMax)
        {
            if (radius < 0f)
            {
                throw new ArgumentOutOfRangeException("radius", "Radius must not be negative.");
            }

            Float3 closest = ClosestPointOnBox(center, boxMin, boxMax);
            Float3 delta = center - closest;

            return (delta.x * delta.x) + (delta.y * delta.y) + (delta.z * delta.z) <= radius * radius;
        }

        /// <summary>
        /// Moves the circle out of the box along the shortest way out.
        /// Returns false (and leaves the position untouched) when there is no overlap.
        /// </summary>
        public static bool ResolveCircleBox(
            Float3 center,
            float radius,
            Float3 boxMin,
            Float3 boxMax,
            out Float3 resolved)
        {
            if (radius < 0f)
            {
                throw new ArgumentOutOfRangeException("radius", "Radius must not be negative.");
            }

            resolved = center;

            if (!OverlapsCircleBox(center, radius, boxMin, boxMax))
            {
                return false;
            }

            Float3 closest = ClosestPointOnBox(center, boxMin, boxMax);
            Float3 delta = center - closest;
            float distance = delta.Magnitude;

            if (distance > 1e-6f)
            {
                // Outside the box but inside the circle: push along the surface normal.
                resolved = closest + (Float3.Normalize(delta) * radius);
                return true;
            }

            // The center is inside the box: leave through the closest face.
            float toMinX = center.x - boxMin.x;
            float toMaxX = boxMax.x - center.x;
            float toMinY = center.y - boxMin.y;
            float toMaxY = boxMax.y - center.y;
            float toMinZ = center.z - boxMin.z;
            float toMaxZ = boxMax.z - center.z;

            float smallest = toMinX;
            resolved = new Float3(boxMin.x - radius, center.y, center.z);

            if (toMaxX < smallest)
            {
                smallest = toMaxX;
                resolved = new Float3(boxMax.x + radius, center.y, center.z);
            }

            if (toMinY < smallest)
            {
                smallest = toMinY;
                resolved = new Float3(center.x, boxMin.y - radius, center.z);
            }

            if (toMaxY < smallest)
            {
                smallest = toMaxY;
                resolved = new Float3(center.x, boxMax.y + radius, center.z);
            }

            if (toMinZ < smallest)
            {
                smallest = toMinZ;
                resolved = new Float3(center.x, center.y, boxMin.z - radius);
            }

            if (toMaxZ < smallest)
            {
                resolved = new Float3(center.x, center.y, boxMax.z + radius);
            }

            return true;
        }

        static float Clamp(float value, float min, float max)
        {
            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }
    }
}
