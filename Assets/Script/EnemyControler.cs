using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    [Header("Status")]
    public float health = 1;
    public float attack = 5;
    public Transform attackTarget;
    protected bool isDead = false; // 🔥 Tambahkan ini untuk mencegah skor double

    [Header("Component")]
    protected Animator anim;
    protected Rigidbody2D rb;
    protected SpriteRenderer graphic;

    [Header("Configuration")]
    [SerializeField] protected float moveSpeed = 2.5f;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        graphic = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // Jika sudah mati, berhenti menjalankan logika Movement/Update
        if (isDead) return; 

        Movement();
        FallDie();
    }

    protected virtual void Movement()
    {
    }

    public void DamagedBy(float damage)
    {
        if (isDead) return; // Mencegah musuh dipukul saat sedang proses mati

        health -= damage;

        if (anim != null)
            anim.SetTrigger("Hit");

        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    void FallDie()
    {
        // Tambahkan cek !isDead agar tidak panggil Die() berkali-kali saat jatuh
        if (transform.position.y < -20 && !isDead)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (isDead) return; // KUNCI UTAMA: Jika sudah mati, abaikan panggilan Die() berikutnya

        isDead = true; // Tandai sudah mati

        // Matikan collider agar peluru lain lewat saja/tidak menabrak lagi
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (anim != null)
            anim.SetTrigger("Die");

        // Skor hanya dipanggil SATU KALI di sini
        ScoreManager.DefeatEnemy();

        Destroy(gameObject, 0.3f);
    }

    public float GetAttackDamage()
    {
        return attack;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return; // Jika sudah mati, peluru tidak bisa mengenai lagi

        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null && bullet.targetTag == "Enemy")
            {
                float damage = bullet.GetDamage();
                DamagedBy(damage);
                
                // Hancurkan peluru segera
                Destroy(collision.gameObject);
            }
        }
    }
}