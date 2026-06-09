using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Obstacle : MonoBehaviour
{
    private bool hasHit = false;

    public void Hit(Player player)
    {
        if (hasHit) return;
        hasHit = true;
        OnHitPlayer(player);
    }

    public abstract void OnHitPlayer(Player player);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            Hit(player);
        }
    }
}