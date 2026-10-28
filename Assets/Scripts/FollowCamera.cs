using Munshiganj.Core;
using UnityEngine;

/// <summary>
/// Third person camera that smoothly follows a target.
/// The interpolation factor is clamped to 0..1 (same rule Unity's Vector3.Lerp uses),
/// so the camera never overshoots even on a long frame.
/// </summary>
public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 4f, -6f);
    public float followSpeed = 8f;
    public float lookHeight = 1.5f;

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 desiredPosition =
            target.TransformPoint(offset);

        float factor = FollowMath.SmoothFactor(followSpeed, Time.deltaTime);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            factor
        );

        Vector3 lookPoint =
            target.position + Vector3.up * lookHeight;

        transform.LookAt(lookPoint);
    }

    /// <summary>Places the camera directly behind the target without any interpolation.</summary>
    public void SnapToTarget()
    {
        if (target == null)
        {
            return;
        }

        transform.position = target.TransformPoint(offset);
        transform.LookAt(target.position + Vector3.up * lookHeight);
    }
}
