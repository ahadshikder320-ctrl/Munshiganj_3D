using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// Play mode tests for the obstacles, the pickups and the score.
/// These need the Unity Editor (Test Runner > PlayMode).
/// </summary>
public class GameplayPlayModeTests
{
    [UnityTest]
    public IEnumerator ObstacleSpawner_SpawnsTheRequestedNumberOfObstacles()
    {
        GameObject spawnerObject = new GameObject("ObstacleSpawner");
        ObstacleSpawner spawner = spawnerObject.AddComponent<ObstacleSpawner>();
        spawner.obstacleCount = 5;
        spawner.seed = 1234;
        spawner.clearRadius = 4f;
        spawner.obstacleSize = 2f;
        spawner.groundWidth = 40f;
        spawner.groundDepth = 40f;
        spawner.spawnOnStart = false;

        int before = Obstacle.All.Count;

        Obstacle[] spawned = spawner.Spawn();

        yield return null;

        Assert.AreEqual(5, spawned.Length);
        Assert.AreEqual(before + 5, Obstacle.All.Count);

        for (int i = 0; i < spawned.Length; i++)
        {
            Assert.IsNotNull(spawned[i]);
            Assert.AreEqual(2f, spawned[i].size.x, 0.001f);
        }

        for (int i = 0; i < spawned.Length; i++)
        {
            Object.Destroy(spawned[i].gameObject);
        }

        Object.Destroy(spawnerObject);
    }

    [UnityTest]
    public IEnumerator PlayerCollision_PushesThePlayerOutOfAnObstacle()
    {
        GameObject obstacleObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obstacleObject.transform.position = new Vector3(0f, 1f, 0f);

        Obstacle obstacle = obstacleObject.AddComponent<Obstacle>();
        obstacle.size = new Vector3(2f, 2f, 2f);
        obstacle.ApplySize();

        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.tag = "Player";
        player.transform.position = new Vector3(1.2f, 1f, 0f);

        PlayerCollision collision = player.AddComponent<PlayerCollision>();
        collision.radius = 0.5f;
        collision.groundWidth = 40f;
        collision.groundDepth = 40f;

        yield return null;

        // The player started inside the obstacle and has to end up outside of it.
        float distance = Vector3.Distance(
            new Vector3(player.transform.position.x, 1f, player.transform.position.z),
            new Vector3(0f, 1f, 0f));

        Assert.IsTrue(distance >= 1.49f, "The player was not pushed out of the obstacle.");

        Object.Destroy(player);
        Object.Destroy(obstacleObject);
    }

    [UnityTest]
    public IEnumerator PlayerCollision_KeepsThePlayerInsideTheGround()
    {
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.tag = "Player";
        player.transform.position = new Vector3(100f, 1f, 0f);

        PlayerCollision collision = player.AddComponent<PlayerCollision>();
        collision.radius = 0.5f;
        collision.groundWidth = 40f;
        collision.groundDepth = 40f;

        yield return null;

        Assert.IsTrue(
            Mathf.Abs(player.transform.position.x) <= 20f,
            "The player left the ground.");

        Object.Destroy(player);
    }

    [UnityTest]
    public IEnumerator GameManager_CountsTheScoreOfTheRound()
    {
        GameObject managerObject = new GameObject("GameManager");
        GameManager manager = managerObject.AddComponent<GameManager>();

        manager.AddPickup(10);
        manager.AddPickup(5);

        Assert.AreEqual(15, manager.Score);
        Assert.AreEqual(15, manager.Best);

        manager.ResetRound();

        Assert.AreEqual(0, manager.Score);
        Assert.AreEqual(15, manager.Best);

        Object.Destroy(managerObject);
    }

    [UnityTest]
    public IEnumerator Pickup_AddsTheScoreWhenThePlayerTouchesIt()
    {
        GameObject managerObject = new GameObject("GameManager");
        GameManager manager = managerObject.AddComponent<GameManager>();

        GameObject pickupObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pickupObject.transform.position = new Vector3(0f, 1f, 0f);

        Collider collider = pickupObject.GetComponent<Collider>();
        collider.isTrigger = true;

        Rigidbody body = pickupObject.AddComponent<Rigidbody>();
        body.isKinematic = true;
        body.useGravity = false;

        Pickup pickup = pickupObject.AddComponent<Pickup>();
        pickup.value = 10;

        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.tag = "Player";
        player.transform.position = new Vector3(0f, 1f, 0f);

        Rigidbody playerBody = player.AddComponent<Rigidbody>();
        playerBody.isKinematic = true;
        playerBody.useGravity = false;

        for (int frame = 0; frame < 10; frame++)
        {
            yield return null;
        }

        Assert.AreEqual(10, manager.Score);

        Object.Destroy(player);
        Object.Destroy(pickupObject);
        Object.Destroy(managerObject);
    }
}
