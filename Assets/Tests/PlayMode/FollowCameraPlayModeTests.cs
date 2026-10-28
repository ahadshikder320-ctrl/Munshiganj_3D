using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// Play mode tests for the runtime components: the follow camera and the generated
/// PlaneMesh ground. These need the Unity Editor (Test Runner > PlayMode).
/// </summary>
public class FollowCameraPlayModeTests
{
    [UnityTest]
    public IEnumerator FollowCamera_ConvergesToTheOffsetPosition()
    {
        GameObject target = new GameObject("Target");
        GameObject cameraObject = new GameObject("Camera");
        cameraObject.tag = "MainCamera";

        FollowCamera followCamera = cameraObject.AddComponent<FollowCamera>();
        followCamera.target = target.transform;
        followCamera.offset = new Vector3(0f, 4f, -6f);
        followCamera.followSpeed = 8f;
        followCamera.lookHeight = 1.5f;

        target.transform.position = new Vector3(0f, 0f, 0f);

        for (int frame = 0; frame < 200; frame++)
        {
            yield return null;
        }

        Vector3 expected = target.transform.position + new Vector3(0f, 4f, -6f);
        Assert.IsTrue(
            Vector3.Distance(cameraObject.transform.position, expected) < 0.05f,
            "Camera did not reach the follow position.");

        Object.Destroy(cameraObject);
        Object.Destroy(target);
    }

    [UnityTest]
    public IEnumerator FollowCamera_WithoutTargetDoesNotMove()
    {
        GameObject cameraObject = new GameObject("Camera");
        FollowCamera followCamera = cameraObject.AddComponent<FollowCamera>();

        Vector3 startPosition = cameraObject.transform.position;
        yield return null;

        Assert.AreEqual(startPosition, cameraObject.transform.position);

        Object.Destroy(cameraObject);
    }

    [UnityTest]
    public IEnumerator GroundPlane_BuildsThePlaneMesh()
    {
        GameObject ground = new GameObject("Ground");
        ground.AddComponent<MeshFilter>();
        ground.AddComponent<MeshRenderer>();

        GroundPlane plane = ground.AddComponent<GroundPlane>();
        plane.width = 20f;
        plane.depth = 30f;
        plane.widthSegments = 4;
        plane.depthSegments = 3;
        plane.Rebuild();

        yield return null;

        Mesh mesh = ground.GetComponent<MeshFilter>().sharedMesh;

        Assert.IsNotNull(mesh);
        Assert.AreEqual((4 + 1) * (3 + 1), mesh.vertexCount);
        Assert.AreEqual(4 * 3 * 2 * 3, mesh.triangles.Length);
        Assert.AreEqual(20f, mesh.bounds.size.x, 0.001f);
        Assert.AreEqual(30f, mesh.bounds.size.z, 0.001f);

        Object.Destroy(ground);
    }
}
