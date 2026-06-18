using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Obstacle : MonoBehaviour
{
    private bool hasHit = false;

    // Method aman yang hanya boleh dipanggil sekali
    public void Hit(Player player)
    {
        if (hasHit) return;       // cegah panggilan kedua & seterusnya
        hasHit = true;
        OnHitPlayer(player);      // panggil implementasi spesifik
    }

    // Implementasi spesifik di class turunan
    public abstract void OnHitPlayer(Player player);

    // Deteksi Player – bisa dipicu beberapa kali jika banyak collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            Hit(player); // otomatis aman dari double‑hit
        }
    }
}