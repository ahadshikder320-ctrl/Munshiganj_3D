using Munshiganj.Core;
using NUnit.Framework;

namespace Munshiganj.EditModeTests
{
    /// <summary>
    /// Tests of the round score and the best score.
    /// </summary>
    public class ScoreCounterTests
    {
        [Test]
        public void Add_IncreasesTheCurrentScore()
        {
            ScoreCounter score = new ScoreCounter();

            score.Add(25);

            Assert.AreEqual(25, score.Current);
        }

        [Test]
        public void AddPickup_AddsThePickupValue()
        {
            ScoreCounter score = new ScoreCounter();

            score.AddPickup();

            Assert.AreEqual(ScoreCounter.PickupValue, score.Current);
        }

        [Test]
        public void Best_KeepsTheHighestScore()
        {
            ScoreCounter score = new ScoreCounter();

            score.Add(50);
            score.Reset();

            Assert.AreEqual(0, score.Current);
            Assert.AreEqual(50, score.Best);
        }

        [Test]
        public void Best_IsUpdatedWhenTheScoreIsHigher()
        {
            ScoreCounter score = new ScoreCounter();

            score.Add(10);
            score.Add(20);

            Assert.AreEqual(30, score.Best);
        }

        [Test]
        public void Reset_ClearsTheCurrentScoreOnly()
        {
            ScoreCounter score = new ScoreCounter();

            score.Add(40);
            score.Reset();
            score.Add(5);

            Assert.AreEqual(5, score.Current);
            Assert.AreEqual(40, score.Best);
        }

        [Test]
        public void Add_WithANegativeValue_Throws()
        {
            ScoreCounter score = new ScoreCounter();

            Assert.Throws<System.ArgumentOutOfRangeException>(delegate { score.Add(-5); });
        }
    }
}
