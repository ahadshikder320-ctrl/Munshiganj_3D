using Munshiganj.Core;
using UnityEngine;

/// <summary>
/// A box shaped obstacle standing on the ground. The box is axis aligned, so its world
/// bounds can be handed to <see cref="CollisionMath"/> without any extra math.
/// </summary>
public class Obstacle : MonoBehaviour
{
    /// <summary>Every obstacle currently in the scene.</summary>
    public static readonly System.Collections.Generic.List<Obstacle> All =
        new System.Collections.Generic.List<Obstacle>();

    public Vector3 size = new Vector3(2f, 2f, 2f);
    public Color obstacleColor = new Color(0.55f, 0.25f, 0.20f);

    void OnEnable()
    {
        if (!All.Contains(this))
        {
            All.Add(this);
        }

        ApplySize();
    }

    void OnDisable()
    {
        All.Remove(this);
    }

    /// <summary>Applies the size to the mesh filter and the material colour.</summary>
    public void ApplySize()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            meshFilter.sharedMesh = Instantiate(meshFilter.sharedMesh);
            meshFilter.sharedMesh.name = "ObstacleMesh";
            meshFilter.sharedMesh.RecalculateBounds();
        }

        Transform cached = transform;
        cached.localScale = new Vector3(
            size.x / 2f,
            size.y / 2f,
            size.z / 2f);

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material.color = obstacleColor;
        }
    }

    /// <summary>World space bounds of the obstacle box.</summary>
    public void GetWorldBounds(out Float3 boxMin, out Float3 boxMax)
    {
        Vector3 center = transform.position;
        Vector3 half = size * 0.5f;

        boxMin = new Float3(center.x - half.x, center.y - half.y, center.z - half.z);
        boxMax = new Float3(center.x + half.x, center.y + half.y, center.z + half.z);
    }
}
