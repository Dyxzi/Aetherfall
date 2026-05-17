using UnityEngine;

public class EnemyBoss : EnemyControler
{
    [Header("Boss")]
    public float detectDistance = 8f;
    public float attackDistance = 2f;

    public float attackCooldown = 1.5f;

    private float attackTimer;

    private SpriteRenderer sprite;

    [Header("Drop")]
    public GameObject crystalPrefab;



    protected override void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();

        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Movement();

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    protected override void Movement()
    {
        if (attackTarget == null) return;

        float distance =
        Vector2.Distance(
            transform.position,
            attackTarget.position
        );

        // 🔥 Flip
        if (attackTarget.position.x < transform.position.x)
            sprite.flipX = true;
        else
            sprite.flipX = false;

        // 🔥 Detect Player
        if (distance <= detectDistance)
        {
            // 🔥 Chase
            if (distance > attackDistance)
            {
                Vector2 dir =
                (attackTarget.position - transform.position).normalized;

                transform.position +=
                (Vector3)(dir * moveSpeed * Time.deltaTime);

                if (anim != null)
                {
                    anim.SetBool("Run", true);
                }
            }
            else
            {
                // 🔥 Stop Run
                if (anim != null)
                {
                    anim.SetBool("Run", false);
                }

                // 🔥 Attack
                if (attackTimer <= 0)
                {
                    attackTimer = attackCooldown;

                    if (anim != null)
                    {
                        anim.SetTrigger("Attack");
                    }

                    PlayerMovment player =
                    attackTarget.GetComponent<PlayerMovment>();

                    if (player != null)
                    {
                        player.DamagedBy(
                            attack,
                            transform.position
                        );
                    }
                }
            }
        }
        else
        {
            // 🔥 Idle
            if (anim != null)
            {
                anim.SetBool("Run", false);
            }
        }
    }

        // 🔥 Kalau sudah mati
    public override void DamagedBy(float damage)
    {
        // Kalau sudah mati
        if (isDead)
            return;

        health -= damage;

        // Hit animation
        if (anim != null)
        {
            anim.SetTrigger("Hit");
        }

        // Mati
        if (health <= 0)
        {
            isDead = true;

            health = 0;

            moveSpeed = 0;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

            if (anim != null)
            {
                anim.SetTrigger("Die");
            }

            Instantiate(
                crystalPrefab,
                transform.position,
                Quaternion.identity
            );

            // Tambah score SEKALI
            ScoreManager.DefeatEnemy();

            Destroy(gameObject, 1f);
        }
    }
}
