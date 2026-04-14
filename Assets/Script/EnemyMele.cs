using System.Collections;
using UnityEngine;

public class EnemyMelee : EnemyControler
{
    [Header("AI Settings")]
    [SerializeField] private float detectDistance = 6f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float patrolDistance = 3f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Attack Hitbox")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Vector2 attackOffset = new Vector2(1f, 0f); // 🔥 default

    [Header("Component")]
    [SerializeField] private SpriteRenderer graphic;
    private Rigidbody2D rb;
    private Animator anim;

    private float attackTimer = 0f;
    private Vector3 startPosition;
    private int patrolDirection = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        startPosition = transform.position;
    }

    protected override void Movement()
    {
        if (attackPoint == null) return;

        // 🔥 kalau tidak ada target tetap patrol
        if (attackTarget == null)
        {
            UpdateAttackPoint(patrolDirection);
            Patrol();
            return;
        }

        float distance = Vector2.Distance(transform.position, attackTarget.position);
        float distanceX = attackTarget.position.x - transform.position.x;

        attackTimer -= Time.deltaTime;

        // 🔥 tentukan arah
        int direction = distanceX > 0 ? 1 : -1;

        // 🔥 update posisi hitbox (FIX UTAMA)
        UpdateAttackPoint(direction);

        if (distance <= attackDistance)
        {
            Attack();
        }
        else if (distance <= detectDistance)
        {
            Chase(direction, distanceX);
        }
        else
        {
            Patrol();
        }
    }

    // ---------------- UPDATE HITBOX ----------------
    void UpdateAttackPoint(int direction)
    {
        attackPoint.localPosition = new Vector2(
            attackOffset.x * direction,
            attackOffset.y
        );
    }

    // ---------------- PATROL ----------------
    void Patrol()
    {
        rb.linearVelocity = new Vector2(patrolDirection * moveSpeed, rb.linearVelocity.y);

        float dist = transform.position.x - startPosition.x;

        // 🔥 hanya balik arah jika sudah melewati batas DAN arah sesuai
        if (dist > patrolDistance && patrolDirection == 1)
        {
            patrolDirection = -1;
        }
        else if (dist < -patrolDistance && patrolDirection == -1)
        {
            patrolDirection = 1;
        }

        graphic.flipX = patrolDirection < 0;

        if (anim != null)
            anim.SetBool("Idle", true);
    }

    // ---------------- CHASE ----------------
    void Chase(int direction, float distanceX)
    {
        // 🔥 DEAD ZONE (anti glitch)
        if (Mathf.Abs(distanceX) < 0.2f)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        graphic.flipX = direction < 0;

        if (anim != null)
            anim.SetBool("Idle", true);
    }

    // ---------------- ATTACK ----------------
    void Attack()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (anim != null)
            anim.SetBool("Idle", false);

        if (attackTimer <= 0)
        {
            attackTimer = attackCooldown;

            if (anim != null)
            {
                anim.ResetTrigger("Attack");
                anim.SetTrigger("Attack");
            }

            StartCoroutine(DoDamage());
        }
    }

    IEnumerator DoDamage()
    {
        yield return new WaitForSeconds(0.2f);

        Collider2D hit = Physics2D.OverlapCircle(
            attackPoint.position,
            attackRadius,
            playerLayer
        );

        if (hit != null)
        {
            PlayerMovment player = hit.GetComponent<PlayerMovment>();

            if (player != null)
            {
                player.DamagedBy(attack, attackPoint.position);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}