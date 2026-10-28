using UnityEngine;

/// <summary>
/// Builds the Munshiganj_3D play scene at runtime: the new PlaneMesh ground, the player,
/// the follow camera and the lighting. Keeping the setup in code means the scene file
/// stays tiny and the layout can be tweaked from the inspector.
/// </summary>
public class GameBootstrap : MonoBehaviour
{
    public float groundWidth = 60f;
    public float groundDepth = 60f;
    public Color skyColor = new Color(0.53f, 0.80f, 0.92f);

    void Start()
    {
        BuildGround();
        BuildPlayer();
        BuildCamera();
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
        player.transform.SetParent(transform, false);
        player.transform.position = new Vector3(0f, 1f, 0f);

        player.AddComponent<PlayerController>();

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
