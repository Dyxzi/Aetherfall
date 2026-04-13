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
    [SerializeField] private Vector2 attackOffset;

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
        anim = GetComponent<Animator>();
        startPosition = transform.position;
    }

    protected override void Movement()
    {
        if (attackTarget == null) return;

        float distance = Vector2.Distance(transform.position, attackTarget.position);

        attackTimer -= Time.deltaTime;
        Vector2 offset = attackOffset;
        offset.x *= graphic.flipX ? -1 : 1;

        attackPoint.localPosition = offset;

        if (distance <= attackDistance)
        {
            Attack();
        }
        else if (distance <= detectDistance)
        {
            Chase();
        }
        else
        {
            Patrol();
        }
    }

    // ---------------- PATROL ----------------
    void Patrol()
    {
        rb.linearVelocity = new Vector2(patrolDirection * moveSpeed, rb.linearVelocity.y);

        if (Mathf.Abs(transform.position.x - startPosition.x) > patrolDistance)
        {
            patrolDirection *= -1;
        }

        graphic.flipX = patrolDirection < 0;

        if (anim != null)
            anim.SetBool("Idle", true);
    }

    // ---------------- CHASE ----------------
    void Chase()
    {
        float direction = attackTarget.position.x > transform.position.x ? 1 : -1;

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

        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayer);

        if (hit != null)
        {
            PlayerMovment player = hit.GetComponent<PlayerMovment>();

            if (player != null)
            {
                player.DamagedBy(attack, attackPoint.position);
            }
        }
    }

    // Debug hitbox
    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}