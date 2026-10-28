using Munshiganj.Core;
using UnityEngine;

/// <summary>
/// Spawns the obstacles on the ground. The layout comes from
/// <see cref="ObstacleSpawnMath"/> so it is reproducible for a given seed.
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    public int obstacleCount = 12;
    public int seed = 20220;
    public float clearRadius = 5f;
    public float obstacleSize = 2f;
    public float groundWidth = 60f;
    public float groundDepth = 60f;

    void Start()
    {
        Spawn();
    }

    /// <summary>Creates the obstacles and returns the spawned objects.</summary>
    public Obstacle[] Spawn()
    {
        Float3[] positions = ObstacleSpawnMath.Plan(
            groundWidth,
            groundDepth,
            obstacleCount,
            seed,
            clearRadius);

        Obstacle[] spawned = new Obstacle[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject obstacleObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obstacleObject.name = "Obstacle" + i;
            obstacleObject.transform.SetParent(transform, false);
            obstacleObject.transform.position = new Vector3(positions[i].x, obstacleSize * 0.5f, positions[i].z);

            Obstacle obstacle = obstacleObject.AddComponent<Obstacle>();
            obstacle.size = new Vector3(obstacleSize, obstacleSize, obstacleSize);
            obstacle.ApplySize();

            spawned[i] = obstacle;
        }

        return spawned;
    }
}
