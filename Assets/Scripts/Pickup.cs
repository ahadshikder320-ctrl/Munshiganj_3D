using UnityEngine;

/// <summary>
/// A spinning pickup. Touching it with the player adds points to the score.
/// </summary>
public class Pickup : MonoBehaviour
{
    public int value = 10;
    public float spinSpeed = 120f;
    public float bobHeight = 0.25f;

    Vector3 origin;

    void Awake()
    {
        origin = transform.position;
    }

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
        transform.position = origin + (Vector3.up * (Mathf.Sin(Time.time * 2f) * bobHeight));
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        GameManager gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.AddPickup(value);
        }

        Destroy(gameObject);
    }
}
