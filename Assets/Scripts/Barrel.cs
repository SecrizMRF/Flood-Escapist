using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Barrel : Obstacle
{
    private new Rigidbody2D rigidbody;
    public float speed = 1f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Collision dengan ground (tidak menimpa OnCollisionEnter2D di base class)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            Hit(player);
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rigidbody.AddForce(collision.transform.right * speed, ForceMode2D.Impulse);
        }
    }

    public override void OnHitPlayer(Player player)
    {
        GameManager.Instance.LevelFailed();
    }
}