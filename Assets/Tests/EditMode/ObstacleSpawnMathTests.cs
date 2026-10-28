using Munshiganj.Core;
using NUnit.Framework;

namespace Munshiganj.EditModeTests
{
    /// <summary>
    /// Tests of the deterministic obstacle layout.
    /// </summary>
    public class ObstacleSpawnMathTests
    {
        [Test]
        public void Plan_ReturnsTheRequestedNumberOfPositions()
        {
            Float3[] positions = ObstacleSpawnMath.Plan(60f, 60f, 12, 1234, 6f);

            Assert.AreEqual(12, positions.Length);
        }

        [Test]
        public void Plan_WithZeroCount_ReturnsAnEmptyArray()
        {
            Float3[] positions = ObstacleSpawnMath.Plan(60f, 60f, 0, 1234, 6f);

            Assert.AreEqual(0, positions.Length);
        }

        [Test]
        public void Plan_WithTheSameSeed_ReturnsTheSamePositions()
        {
            Float3[] first = ObstacleSpawnMath.Plan(60f, 60f, 8, 99, 5f);
            Float3[] second = ObstacleSpawnMath.Plan(60f, 60f, 8, 99, 5f);

            for (int i = 0; i < first.Length; i++)
            {
                Assert.AreEqual(first[i].x, second[i].x, 0.000001);
                Assert.AreEqual(first[i].z, second[i].z, 0.000001);
            }
        }

        [Test]
        public void Plan_WithADifferentSeed_ReturnsDifferentPositions()
        {
            Float3[] first = ObstacleSpawnMath.Plan(60f, 60f, 8, 1, 5f);
            Float3[] second = ObstacleSpawnMath.Plan(60f, 60f, 8, 2, 5f);

            bool anyDifferent = false;

            for (int i = 0; i < first.Length; i++)
            {
                if (Math.Abs(first[i].x - second[i].x) > 0.0001f)
                {
                    anyDifferent = true;
                }
            }

            Assert.IsTrue(anyDifferent, "A different seed must produce a different layout.");
        }

        [Test]
        public void Plan_KeepsThePositionsInsideTheGround()
        {
            Float3[] positions = ObstacleSpawnMath.Plan(60f, 60f, 20, 7, 5f);

            for (int i = 0; i < positions.Length; i++)
            {
                Assert.IsTrue(positions[i].x >= -30f && positions[i].x <= 30f);
                Assert.IsTrue(positions[i].z >= -30f && positions[i].z <= 30f);
            }
        }

        [Test]
        public void Plan_KeepsThePlayerSpawnAreaClear()
        {
            Float3[] positions = ObstacleSpawnMath.Plan(60f, 60f, 20, 7, 6f);

            for (int i = 0; i < positions.Length; i++)
            {
                float distance = (float)Math.Sqrt(
                    (positions[i].x * positions[i].x) + (positions[i].z * positions[i].z));

                Assert.IsTrue(distance >= 6f, "An obstacle blocks the player spawn area.");
            }
        }

        [Test]
        public void Plan_WithAZeroGround_Throws()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(
                delegate { ObstacleSpawnMath.Plan(0f, 60f, 4, 1, 5f); });
        }

        [Test]
        public void Plan_WithANegativeCount_Throws()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(
                delegate { ObstacleSpawnMath.Plan(60f, 60f, -1, 1, 5f); });
        }
    }
}
