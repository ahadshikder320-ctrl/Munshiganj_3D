using Munshiganj.Core;
using NUnit.Framework;

namespace Munshiganj.EditModeTests
{
    /// <summary>
    /// Tests of the axis aligned box collision used by the player against the obstacles.
    /// </summary>
    public class CollisionMathTests
    {
        static readonly Float3 BoxMin = new Float3(-1f, 0f, -1f);
        static readonly Float3 BoxMax = new Float3(1f, 2f, 1f);

        [Test]
        public void OverlapsBox_InsideTheBox_ReturnsTrue()
        {
            Assert.IsTrue(CollisionMath.OverlapsBox(new Float3(0f, 1f, 0f), BoxMin, BoxMax));
        }

        [Test]
        public void OverlapsBox_OutsideTheBox_ReturnsFalse()
        {
            Assert.IsFalse(CollisionMath.OverlapsBox(new Float3(5f, 1f, 0f), BoxMin, BoxMax));
        }

        [Test]
        public void OverlapsCircleBox_InsideTheBox_ReturnsTrue()
        {
            Assert.IsTrue(CollisionMath.OverlapsCircleBox(new Float3(0f, 1f, 0f), 0.5f, BoxMin, BoxMax));
        }

        [Test]
        public void OverlapsCircleBox_FarAway_ReturnsFalse()
        {
            Assert.IsFalse(CollisionMath.OverlapsCircleBox(new Float3(10f, 1f, 0f), 0.5f, BoxMin, BoxMax));
        }

        [Test]
        public void OverlapsCircleBox_TouchingTheEdge_ReturnsTrue()
        {
            Assert.IsTrue(CollisionMath.OverlapsCircleBox(new Float3(1.5f, 1f, 0f), 0.5f, BoxMin, BoxMax));
        }

        [Test]
        public void ResolveCircleBox_WithoutOverlap_ReturnsFalse()
        {
            Float3 resolved;
            bool moved = CollisionMath.ResolveCircleBox(
                new Float3(10f, 1f, 0f), 0.5f, BoxMin, BoxMax, out resolved);

            Assert.IsFalse(moved);
            Assert.AreEqual(10.0, resolved.x, 0.000001);
        }

        [Test]
        public void ResolveCircleBox_OutsideTheBox_PushesOutToTheSurface()
        {
            Float3 resolved;
            bool moved = CollisionMath.ResolveCircleBox(
                new Float3(1.5f, 1f, 0f), 0.5f, BoxMin, BoxMax, out resolved);

            Assert.IsTrue(moved);
            Assert.AreEqual(1.5, resolved.x, 0.000001);
            Assert.IsFalse(CollisionMath.OverlapsCircleBox(resolved, 0.5f, BoxMin, BoxMax));
        }

        [Test]
        public void ResolveCircleBox_InsideTheBox_LeavesThroughTheClosestFace()
        {
            Float3 resolved;
            bool moved = CollisionMath.ResolveCircleBox(
                new Float3(0f, 1f, 0f), 0.5f, BoxMin, BoxMax, out resolved);

            Assert.IsTrue(moved);
            Assert.IsFalse(CollisionMath.OverlapsCircleBox(resolved, 0.5f, BoxMin, BoxMax));
        }

        [Test]
        public void ResolveCircleBoxXZ_KeepsTheHeight()
        {
            Float3 resolved;
            bool moved = CollisionMath.ResolveCircleBoxXZ(
                new Float3(1.5f, 1.25f, 0f), 0.5f, BoxMin, BoxMax, out resolved);

            Assert.IsTrue(moved);
            Assert.AreEqual(1.25, resolved.y, 0.000001);
            Assert.AreEqual(1.5, resolved.x, 0.000001);
        }

        [Test]
        public void ResolveCircleBoxXZ_InsideTheBox_PushesOutOnTheXZPlane()
        {
            Float3 resolved;
            bool moved = CollisionMath.ResolveCircleBoxXZ(
                new Float3(0f, 1f, 0f), 0.5f, BoxMin, BoxMax, out resolved);

            Assert.IsTrue(moved);
            Assert.IsFalse(CollisionMath.OverlapsCircleBoxXZ(resolved, 0.5f, BoxMin, BoxMax));
        }

        [Test]
        public void ClampToBoundsXZ_KeepsThePositionInsideTheGround()
        {
            Float3 clamped = CollisionMath.ClampToBoundsXZ(new Float3(100f, 2f, -100f), 30f, 20f);

            Assert.AreEqual(30.0, clamped.x, 0.000001);
            Assert.AreEqual(2.0, clamped.y, 0.000001);
            Assert.AreEqual(-20.0, clamped.z, 0.000001);
        }

        [Test]
        public void OverlapsCircleBox_WithANegativeRadius_Throws()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(
                delegate { CollisionMath.OverlapsCircleBox(Float3.Zero, -1f, BoxMin, BoxMax); });
        }
    }
}
