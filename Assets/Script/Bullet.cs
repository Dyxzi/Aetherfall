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
        
    
        rb.linearVelocity = direction * speed; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Jika peluru sudah pernah mengenai sesuatu
        if (hasHit) return;

        // Cek apakah mengenai tanah atau target (Enemy/Player)
        if (collision.CompareTag("Ground") || collision.CompareTag(targetTag))
        {
            hasHit = true;
            
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public float GetDamage()
    {
        return damage;
    }
}