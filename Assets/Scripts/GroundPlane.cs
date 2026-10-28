using Munshiganj.Core;
using UnityEngine;

/// <summary>
/// The "new PlaneMesh" of the Munshiganj_3D scene: a subdivided ground plane that is
/// generated at runtime by <see cref="PlaneMeshBuilder"/>.
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class GroundPlane : MonoBehaviour
{
    public float width = 60f;
    public float depth = 60f;
    public int widthSegments = 10;
    public int depthSegments = 10;
    public Color groundColor = new Color(0.30f, 0.55f, 0.25f);

    void Awake()
    {
        Rebuild();
    }

    /// <summary>Regenerates the plane mesh and applies the ground colour.</summary>
    public void Rebuild()
    {
        PlaneMeshData data = PlaneMeshBuilder.Build(width, depth, widthSegments, depthSegments);

        Mesh mesh = new Mesh();
        mesh.name = "PlaneMesh";
        mesh.vertices = ToVector3Array(data.Positions);
        mesh.normals = ToVector3Array(data.Normals);
        mesh.uv = ToVector2Array(data.Uvs);
        mesh.triangles = data.Indices;
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = mesh;

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.sharedMaterial = CreateMaterial(groundColor);
    }

    static Vector3[] ToVector3Array(float[] values)
    {
        Vector3[] result = new Vector3[values.Length / 3];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new Vector3(values[i * 3], values[(i * 3) + 1], values[(i * 3) + 2]);
        }

        return result;
    }

    static Vector2[] ToVector2Array(float[] values)
    {
        Vector2[] result = new Vector2[values.Length / 2];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new Vector2(values[i * 2], values[(i * 2) + 1]);
        }

        return result;
    }

    static Material CreateMaterial(Color color)
    {
        Shader shader = Shader.Find("Standard");
        if (shader == null)
        {
            shader = Shader.Find("Unlit/Color");
        }

        Material material = new Material(shader);
        material.name = "GroundMaterial";

        if (material.HasProperty("_Color"))
        {
            material.color = color;
        }

        return material;
    }
}
