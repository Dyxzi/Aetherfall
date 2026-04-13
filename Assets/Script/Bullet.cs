using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string targetTag;
    private float damage;

    public void Launch(Vector2 direction, string target, float speed, float dmg)
    {
        targetTag = target;
        damage = dmg;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed; // harus velocity, bukan linearVelocity
    }

    // Untuk deteksi collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Bisa kena ground atau enemy/player
        if (collision.CompareTag("Ground") || collision.CompareTag(targetTag))
        {
            Destroy(gameObject);
        }
    }

    // Optional: auto destroy jika keluar layar
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public float GetDamage()
    {
        return damage;
    }
}