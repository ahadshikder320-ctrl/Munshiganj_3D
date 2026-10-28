using Munshiganj.Core;
using UnityEngine;

/// <summary>
/// Moves the player around the ground plane. The movement direction is calculated
/// relative to the player yaw so the controls stay consistent while the camera follows.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float turnSpeed = 720f;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal == 0f && vertical == 0f)
        {
            return;
        }

        float yaw = transform.eulerAngles.y;
        Float3 direction = PlayerInputMath.MoveDirection(horizontal, vertical, yaw);

        Vector3 movement = new Vector3(direction.x, 0f, direction.z) * (moveSpeed * Time.deltaTime);
        transform.position += movement;

        Vector3 lookDirection = new Vector3(direction.x, 0f, direction.z);
        if (lookDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime);
        }
    }
}
