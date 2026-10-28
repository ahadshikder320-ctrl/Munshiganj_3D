using System;

namespace Munshiganj.Core
{
    /// <summary>
    /// Engine independent 3D vector.
    /// The core gameplay math lives in this namespace so that it can be unit tested
    /// outside of the Unity Editor (see Tests/Portable/PortableTestRunner.cs).
    /// </summary>
    [Serializable]
    public struct Float3
    {
        public float x;
        public float y;
        public float z;

        public Float3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static readonly Float3 Zero = new Float3(0f, 0f, 0f);
        public static readonly Float3 Up = new Float3(0f, 1f, 0f);
        public static readonly Float3 Forward = new Float3(0f, 0f, 1f);
        public static readonly Float3 Right = new Float3(1f, 0f, 0f);

        public float Magnitude
        {
            get { return (float)Math.Sqrt((x * x) + (y * y) + (z * z)); }
        }

        public static Float3 operator +(Float3 a, Float3 b)
        {
            return new Float3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Float3 operator -(Float3 a, Float3 b)
        {
            return new Float3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Float3 operator -(Float3 a)
        {
            return new Float3(-a.x, -a.y, -a.z);
        }

        public static Float3 operator *(Float3 a, float scalar)
        {
            return new Float3(a.x * scalar, a.y * scalar, a.z * scalar);
        }

        public static Float3 operator *(float scalar, Float3 a)
        {
            return a * scalar;
        }

        public static Float3 operator /(Float3 a, float scalar)
        {
            return new Float3(a.x / scalar, a.y / scalar, a.z / scalar);
        }

        public static Float3 Normalize(Float3 value)
        {
            float magnitude = value.Magnitude;
            if (magnitude <= 1e-6f)
            {
                return Zero;
            }

            return value / magnitude;
        }

        public override string ToString()
        {
            return string.Format("({0:0.###}, {1:0.###}, {2:0.###})", x, y, z);
        }
    }
}
