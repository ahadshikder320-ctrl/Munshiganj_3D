using Munshiganj.Core;
using UnityEngine;

/// <summary>
/// Keeps the score of the round and draws it on screen. The score itself lives in the
/// engine independent <see cref="ScoreCounter"/> so it can be unit tested.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>Instance used by the pickups to report a collected item.</summary>
    public static GameManager Instance { get; private set; }

    ScoreCounter score = new ScoreCounter();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    /// <summary>Points collected in the current round.</summary>
    public int Score
    {
        get { return score.Current; }
    }

    /// <summary>Highest score reached in this session.</summary>
    public int Best
    {
        get { return score.Best; }
    }

    /// <summary>Adds the points of a collected pickup.</summary>
    public void AddPickup(int value)
    {
        score.Add(value);
    }

    /// <summary>Starts a new round.</summary>
    public void ResetRound()
    {
        score.Reset();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(16f, 16f, 320f, 24f), "Score: " + score.Current);
        GUI.Label(new Rect(16f, 40f, 320f, 24f), "Best: " + score.Best);
    }
}
