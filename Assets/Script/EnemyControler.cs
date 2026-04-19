using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    [Header("Status")]
    public float health = 1;
    public float attack = 5;
    public Transform attackTarget;

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
        Movement();
        FallDie();
    }

    protected virtual void Movement()
    {
    }

    public void DamagedBy(float damage)
    {
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
        if (transform.position.y < -20)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (anim != null)
            anim.SetTrigger("Die");

        Destroy(gameObject, 0.3f);
        ScoreManager.DefeatEnemy();
    }

    public float GetAttackDamage()
    {
        return attack;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null && bullet.targetTag == "Enemy")
            {
                float damage = bullet.GetDamage();
                DamagedBy(damage);
                Destroy(collision.gameObject);
            }
        }
    }
}