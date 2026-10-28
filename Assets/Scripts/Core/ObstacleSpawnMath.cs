using System;

namespace Munshiganj.Core
{
    /// <summary>
    /// Computes deterministic spawn positions for the obstacles.
    /// The layout uses a golden angle spiral so the obstacles are spread evenly over the
    /// ground while staying reproducible for a given seed.
    /// </summary>
    public static class ObstacleSpawnMath
    {
        /// <summary>Angle between two neighbours of the spiral, in radians.</summary>
        public const double GoldenAngle = 2.399963229728653;

        /// <summary>
        /// Plans the obstacle positions.
        /// </summary>
        /// <param name="groundWidth">Size of the ground along X.</param>
        /// <param name="groundDepth">Size of the ground along Z.</param>
        /// <param name="count">Number of obstacles.</param>
        /// <param name="seed">Seed of the random generator.</param>
        /// <param name="clearRadius">Area around the origin that stays free (player spawn).</param>
        public static Float3[] Plan(float groundWidth, float groundDepth, int count, int seed, float clearRadius = 4f)
        {
            if (groundWidth <= 0f)
            {
                throw new ArgumentOutOfRangeException("groundWidth", "Ground width must be greater than zero.");
            }

            if (groundDepth <= 0f)
            {
                throw new ArgumentOutOfRangeException("groundDepth", "Ground depth must be greater than zero.");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "The obstacle count must not be negative.");
            }

            if (clearRadius < 0f)
            {
                throw new ArgumentOutOfRangeException("clearRadius", "The clear radius must not be negative.");
            }

            float halfWidth = groundWidth * 0.5f;
            float halfDepth = groundDepth * 0.5f;
            float maxRadiusX = halfWidth - clearRadius;
            float maxRadiusZ = halfDepth - clearRadius;

            if (maxRadiusX < 0f)
            {
                maxRadiusX = 0f;
            }

            if (maxRadiusZ < 0f)
            {
                maxRadiusZ = 0f;
            }

            float maxRadius = (float)Math.Min(maxRadiusX, maxRadiusZ);
            float minRadius = clearRadius;

            if (maxRadius < minRadius)
            {
                minRadius = maxRadius;
            }

            Float3[] positions = new Float3[count];
            uint state = unchecked((uint)seed) + 0x9E3779B9u;

            for (int i = 0; i < count; i++)
            {
                state = NextState(state);

                float jitter = (NextUnit(state) - 0.5f) * 0.4f;
                state = NextState(state);

                float radiusScale = NextUnit(state);
                state = NextState(state);

                double angle = (i * GoldenAngle) + jitter;
                float radius = minRadius + ((maxRadius - minRadius) * radiusScale);

                positions[i] = new Float3(
                    (float)(Math.Cos(angle) * radius),
                    0f,
                    (float)(Math.Sin(angle) * radius));
            }

            return positions;
        }

        static uint NextState(uint state)
        {
            // Deterministic linear congruential generator (same constants as glibc).
            state = unchecked((state * 1103515245u) + 12345u);
            return state;
        }

        static float NextUnit(uint state)
        {
            return (state >> 8) / 16777216f;
        }
    }
}
