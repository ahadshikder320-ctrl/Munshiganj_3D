using Munshiganj.Core;
using UnityEngine;

/// <summary>
/// Keeps the player on the ground and pushes it out of the obstacles. The resolution
/// itself is done by <see cref="CollisionMath"/>, which is unit tested.
/// </summary>
public class PlayerCollision : MonoBehaviour
{
    public float radius = 0.5f;
    public float groundWidth = 60f;
    public float groundDepth = 60f;

    void LateUpdate()
    {
        Vector3 position = transform.position;

        Float3 resolved;
        bool changed = false;

        for (int i = 0; i < Obstacle.All.Count; i++)
        {
            Obstacle obstacle = Obstacle.All[i];
            if (obstacle == null)
            {
                continue;
            }

            Float3 boxMin;
            Float3 boxMax;
            obstacle.GetWorldBounds(out boxMin, out boxMax);

            Float3 current = new Float3(position.x, position.y, position.z);
            if (CollisionMath.ResolveCircleBoxXZ(current, radius, boxMin, boxMax, out resolved))
            {
                position = new Vector3(resolved.x, resolved.y, resolved.z);
                changed = true;
            }
        }

        Float3 clamped = CollisionMath.ClampToBoundsXZ(
            new Float3(position.x, position.y, position.z),
            (groundWidth * 0.5f) - radius,
            (groundDepth * 0.5f) - radius);

        if (!changed || clamped.x != position.x || clamped.z != position.z)
        {
            position = new Vector3(clamped.x, clamped.y, clamped.z);
        }

        transform.position = position;
    }
}
