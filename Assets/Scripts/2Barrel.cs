using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Barrel : Obstacle     // <- Sekarang mewarisi Obstacle
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
        // Panggil base jika perlu? Base class juga punya OnCollisionEnter2D.
        // Unity akan memanggil keduanya? Sebenarnya hanya satu yang akan dipanggil,
        // karena method ini bukan virtual. Agar aman, kita pindahkan logika ground
        // ke sini, dan secara eksplisit panggil base jika menyentuh Player.
        // Alternatif: gunakan method terpisah.
        
        // Cek apakah yang disentuh adalah Player
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            Hit(player); // panggil implementasi abstract
            return;
        }

        // Jika ground, tambahkan gaya
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rigidbody.AddForce(collision.transform.right * speed, ForceMode2D.Impulse);
        }
    }

    public override void OnHitPlayer(Player player)
    {
        // Barrel hanya memberitahu GameManager
        GameManager.Instance.LevelFailed();
    }
}