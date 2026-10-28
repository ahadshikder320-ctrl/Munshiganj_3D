using System;

namespace NUnit.Framework
{
    /// <summary>
    /// Minimal stand-in for the parts of NUnit that the EditMode tests use.
    /// It lets the very same test files run outside of the Unity Editor through the
    /// portable runner (Tests/Portable/PortableTestRunner.cs).
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TestAttribute : Attribute
    {
    }

    public delegate void TestDelegate();

    public class AssertionException : Exception
    {
        public AssertionException(string message)
            : base(message)
        {
        }
    }

    public static class Assert
    {
        public static void AreEqual(double expected, double actual, double delta)
        {
            if (Math.Abs(expected - actual) > delta)
            {
                throw new AssertionException(
                    "Expected " + expected + " but was " + actual + " (delta " + delta + ").");
            }
        }

        public static void AreEqual(double expected, double actual)
        {
            AreEqual(expected, actual, 0.000001);
        }

        public static void AreEqual(int expected, int actual)
        {
            if (expected != actual)
            {
                throw new AssertionException("Expected " + expected + " but was " + actual + ".");
            }
        }

        public static void AreEqual(object expected, object actual)
        {
            if (!object.Equals(expected, actual))
            {
                throw new AssertionException("Expected " + expected + " but was " + actual + ".");
            }
        }

        public static void IsTrue(bool condition)
        {
            if (!condition)
            {
                throw new AssertionException("Expected the condition to be true.");
            }
        }

        public static void IsTrue(bool condition, string message)
        {
            if (!condition)
            {
                throw new AssertionException(message);
            }
        }

        public static void IsFalse(bool condition)
        {
            if (condition)
            {
                throw new AssertionException("Expected the condition to be false.");
            }
        }

        public static void IsFalse(bool condition, string message)
        {
            if (condition)
            {
                throw new AssertionException(message);
            }
        }

        public static void IsNotNull(object value)
        {
            if (value == null)
            {
                throw new AssertionException("Expected a value but was null.");
            }
        }

        public static T Throws<T>(TestDelegate code) where T : Exception
        {
            try
            {
                code();
            }
            catch (T expected)
            {
                return expected;
            }
            catch (Exception other)
            {
                throw new AssertionException(
                    "Expected " + typeof(T).Name + " but got " + other.GetType().Name + ".");
            }

            throw new AssertionException("Expected " + typeof(T).Name + " but no exception was thrown.");
        }
    }
}
