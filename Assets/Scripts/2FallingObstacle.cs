using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class FallingObstacle : Obstacle
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
            gameObject.SetActive(false);
    }

    // Override abstract method
    public override void OnHitPlayer(Player player)
    {
        GameManager.Instance.LevelFailed();
    }

    // Karena base class Obstacle punya OnCollisionEnter2D, dan kita tidak
    // mendefinisikan lagi di sini, base class akan menangani tabrakan dengan Player.
    // Itu sudah memanggil OnHitPlayer. Jadi FallingObstacle bisa membiarkan
    // base class bekerja. Perfect!
}