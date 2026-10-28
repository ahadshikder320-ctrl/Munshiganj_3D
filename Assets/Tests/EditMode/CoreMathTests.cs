using System;
using Munshiganj.Core;
using NUnit.Framework;

namespace Munshiganj.Tests
{
    /// <summary>
    /// Tests for the engine independent gameplay math. The same file is executed by the
    /// Unity Test Runner (EditMode) and by the portable runner in Tests/Portable.
    /// </summary>
    public class CoreMathTests
    {
        const float Tolerance = 0.0001f;

        [Test]
        public void SmoothFactor_IsSpeedTimesDeltaTime()
        {
            Assert.AreEqual(0.4f, FollowMath.SmoothFactor(8f, 0.05f), Tolerance);
        }

        [Test]
        public void SmoothFactor_IsClampedToOne()
        {
            Assert.AreEqual(1f, FollowMath.SmoothFactor(8f, 1f), Tolerance);
        }

        [Test]
        public void SmoothFactor_IsZeroForZeroDeltaTime()
        {
            Assert.AreEqual(0f, FollowMath.SmoothFactor(8f, 0f), Tolerance);
        }

        [Test]
        public void SmoothFactor_IsZeroForNegativeDeltaTime()
        {
            Assert.AreEqual(0f, FollowMath.SmoothFactor(8f, -0.5f), Tolerance);
        }

        [Test]
        public void Lerp_InterpolatesBetweenTwoPoints()
        {
            Float3 result = FollowMath.Lerp(new Float3(0f, 0f, 0f), new Float3(10f, 4f, -6f), 0.5f);

            Assert.AreEqual(5f, result.x, Tolerance);
            Assert.AreEqual(2f, result.y, Tolerance);
            Assert.AreEqual(-3f, result.z, Tolerance);
        }

        [Test]
        public void Lerp_ClampsTheFactor()
        {
            Float3 from = new Float3(1f, 1f, 1f);
            Float3 to = new Float3(3f, 3f, 3f);

            Assert.AreEqual(to.x, FollowMath.Lerp(from, to, 5f).x, Tolerance);
            Assert.AreEqual(from.x, FollowMath.Lerp(from, to, -5f).x, Tolerance);
        }

        [Test]
        public void DesiredPosition_WithoutRotation_KeepsTheOffset()
        {
            Float3 result = FollowMath.DesiredPosition(
                new Float3(0f, 0f, 0f),
                0f,
                new Float3(0f, 4f, -6f));

            Assert.AreEqual(0f, result.x, Tolerance);
            Assert.AreEqual(4f, result.y, Tolerance);
            Assert.AreEqual(-6f, result.z, Tolerance);
        }

        [Test]
        public void DesiredPosition_RotatesTheOffsetWithTheTargetYaw()
        {
            // A target rotated by 90 degrees turns its local -Z offset into -X.
            Float3 result = FollowMath.DesiredPosition(
                new Float3(5f, 1f, 2f),
                90f,
                new Float3(0f, 4f, -6f));

            Assert.AreEqual(-1f, result.x, Tolerance);
            Assert.AreEqual(5f, result.y, Tolerance);
            Assert.AreEqual(2f, result.z, Tolerance);
        }

        [Test]
        public void LookPoint_RaisesTheTargetPosition()
        {
            Float3 result = FollowMath.LookPoint(new Float3(3f, 2f, -1f), 1.5f);

            Assert.AreEqual(3f, result.x, Tolerance);
            Assert.AreEqual(3.5f, result.y, Tolerance);
            Assert.AreEqual(-1f, result.z, Tolerance);
        }

        [Test]
        public void MoveDirection_ForwardWithNoYawPointsAlongPositiveZ()
        {
            Float3 direction = PlayerInputMath.MoveDirection(0f, 1f, 0f);

            Assert.AreEqual(0f, direction.x, Tolerance);
            Assert.AreEqual(0f, direction.y, Tolerance);
            Assert.AreEqual(1f, direction.z, Tolerance);
        }

        [Test]
        public void MoveDirection_IsRelativeToTheYaw()
        {
            Float3 direction = PlayerInputMath.MoveDirection(0f, 1f, 90f);

            Assert.AreEqual(1f, direction.x, Tolerance);
            Assert.AreEqual(0f, direction.y, Tolerance);
            Assert.AreEqual(0f, direction.z, Tolerance);
        }

        [Test]
        public void MoveDirection_IsNormalizedForDiagonalInput()
        {
            Float3 direction = PlayerInputMath.MoveDirection(1f, 1f, 0f);

            Assert.AreEqual(1f, direction.Magnitude, Tolerance);
        }

        [Test]
        public void MoveDirection_WithoutInputReturnsZero()
        {
            Float3 direction = PlayerInputMath.MoveDirection(0f, 0f, 45f);

            Assert.AreEqual(0f, direction.Magnitude, Tolerance);
        }
    }
}
