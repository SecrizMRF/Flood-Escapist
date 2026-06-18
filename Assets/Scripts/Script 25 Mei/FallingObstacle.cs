using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class FallingObstacleOld : MonoBehaviour
{
    public float fallSpeed = 5f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.down * fallSpeed;
    }

    private void Update()
    {
        if (transform.position.y < -20f)
            Destroy(gameObject);
    }
}