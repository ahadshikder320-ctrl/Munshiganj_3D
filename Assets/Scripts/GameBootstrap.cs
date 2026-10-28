using Munshiganj.Core;
using UnityEngine;

/// <summary>
/// Builds the Munshiganj_3D play scene at runtime: the new PlaneMesh ground, the player,
/// the follow camera, the obstacles, the pickups and the lighting. Keeping the setup in
/// code means the scene file stays tiny and the layout can be tweaked from the inspector.
/// </summary>
public class GameBootstrap : MonoBehaviour
{
    public float groundWidth = 60f;
    public float groundDepth = 60f;
    public Color skyColor = new Color(0.53f, 0.80f, 0.92f);

    [Header("Obstacles")]
    public int obstacleCount = 12;
    public int obstacleSeed = 20220;
    public float obstacleSize = 2f;
    public float clearRadius = 6f;

    [Header("Pickups")]
    public int pickupCount = 8;
    public int pickupSeed = 4242;

    void Start()
    {
        BuildGround();
        BuildPlayer();
        BuildCamera();
        BuildObstacles();
        BuildPickups();
        BuildLight();
    }

    void BuildGround()
    {
        GameObject ground = new GameObject("Ground");
        ground.transform.SetParent(transform, false);
        ground.AddComponent<MeshFilter>();
        ground.AddComponent<MeshRenderer>();

        GroundPlane plane = ground.AddComponent<GroundPlane>();
        plane.width = groundWidth;
        plane.depth = groundDepth;
        plane.Rebuild();
    }

    void BuildPlayer()
    {
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.tag = "Player";
        player.transform.SetParent(transform, false);
        player.transform.position = new Vector3(0f, 1f, 0f);

        player.AddComponent<PlayerController>();

        PlayerCollision collision = player.AddComponent<PlayerCollision>();
        collision.groundWidth = groundWidth;
        collision.groundDepth = groundDepth;

        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        marker.name = "Marker";
        marker.transform.SetParent(player.transform, false);
        marker.transform.localPosition = new Vector3(0f, 1.1f, 0f);
        marker.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        Object.Destroy(marker.GetComponent<Collider>());
    }

    void BuildCamera()
    {
        GameObject cameraObject = new GameObject("Main Camera");
        cameraObject.transform.SetParent(transform, false);
        cameraObject.tag = "MainCamera";

        Camera camera = cameraObject.AddComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = skyColor;
        camera.fieldOfView = 60f;

        FollowCamera followCamera = cameraObject.AddComponent<FollowCamera>();
        followCamera.target = GameObject.Find("Player").transform;
        followCamera.SnapToTarget();
    }

    void BuildObstacles()
    {
        GameObject spawnerObject = new GameObject("ObstacleSpawner");
        spawnerObject.transform.SetParent(transform, false);

        ObstacleSpawner spawner = spawnerObject.AddComponent<ObstacleSpawner>();
        spawner.obstacleCount = obstacleCount;
        spawner.seed = obstacleSeed;
        spawner.clearRadius = clearRadius;
        spawner.obstacleSize = obstacleSize;
        spawner.groundWidth = groundWidth;
        spawner.groundDepth = groundDepth;
        spawner.Spawn();
    }

    void BuildPickups()
    {
        GameObject managerObject = new GameObject("GameManager");
        managerObject.transform.SetParent(transform, false);
        managerObject.AddComponent<GameManager>();

        Float3[] positions = ObstacleSpawnMath.Plan(
            groundWidth,
            groundDepth,
            pickupCount,
            pickupSeed,
            clearRadius);

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject pickupObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pickupObject.name = "Pickup" + i;
            pickupObject.transform.SetParent(transform, false);
            pickupObject.transform.position = new Vector3(positions[i].x, 1f, positions[i].z);
            pickupObject.transform.localScale = new Vector3(0.6f, 0.2f, 0.6f);

            Collider collider = pickupObject.GetComponent<Collider>();
            collider.isTrigger = true;

            Pickup pickup = pickupObject.AddComponent<Pickup>();
            pickup.value = ScoreCounter.PickupValue;
        }
    }

    void BuildLight()
    {
        GameObject lightObject = new GameObject("Directional Light");
        lightObject.transform.SetParent(transform, false);
        lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.1f;
    }
}
