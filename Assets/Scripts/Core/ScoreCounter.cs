using System;

namespace Munshiganj.Core
{
    /// <summary>
    /// Score state of the round: the collected pickups and the best score so far.
    /// Engine independent so it can be unit tested without the Unity Editor.
    /// </summary>
    public class ScoreCounter
    {
        /// <summary>Points a single pickup is worth.</summary>
        public const int PickupValue = 10;

        int current;
        int best;

        /// <summary>Points collected in the current round.</summary>
        public int Current
        {
            get { return current; }
        }

        /// <summary>Highest score reached in this session.</summary>
        public int Best
        {
            get { return best; }
        }

        /// <summary>Adds points and returns the new score.</summary>
        public int Add(int points)
        {
            if (points < 0)
            {
                throw new ArgumentOutOfRangeException("points", "Points must not be negative.");
            }

            current += points;

            if (current > best)
            {
                best = current;
            }

            return current;
        }

        /// <summary>Adds one pickup.</summary>
        public int AddPickup()
        {
            return Add(PickupValue);
        }

        /// <summary>Starts a new round.</summary>
        public void Reset()
        {
            current = 0;
        }
    }
}
