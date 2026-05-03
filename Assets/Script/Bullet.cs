using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string targetTag;
    private float damage;
    private bool hasHit = false; // Mencegah peluru memicu trigger berkali-kali dalam satu frame

    public void Launch(Vector2 direction, string target, float speed, float dmg)
    {
        targetTag = target;
        damage = dmg;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        
        // Unity 6 menggunakan linearVelocity, versi sebelumnya menggunakan velocity.
        // Jika Unity Anda versi 6, kode di bawah sudah benar.
        rb.linearVelocity = direction * speed; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Jika peluru sudah pernah mengenai sesuatu, abaikan (mencegah double hit)
        if (hasHit) return;

        // Cek apakah mengenai tanah atau target (Enemy/Player)
        if (collision.CompareTag("Ground") || collision.CompareTag(targetTag))
        {
            hasHit = true;

            // Jika mengenai target, pastikan script di sisi Enemy/Player yang memproses damage
            // sudah memiliki pengecekan "isDead" agar skor tidak double.
            
            Destroy(gameObject);
        }
    }

    // Auto destroy jika keluar layar agar tidak memenuhi memori
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public float GetDamage()
    {
        return damage;
    }
}