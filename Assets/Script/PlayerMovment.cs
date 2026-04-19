using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    public static PlayerMovment Instance { get; private set; }

    [Header("Player Components")]
    public SpriteRenderer playerSprite;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    private Rigidbody2D rb;
    private bool isGrounded;
    public Animator anim;

    [Header("Player Status")]
    public float health = 100;
    public float healthMax;
    public bool canBeMoved = true;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float knockbackDuration = 0.2f;
    private float knockbackTimer = 0f;

    [Header("Invincible")]
    [SerializeField] private float invincibleTime = 0.5f;
    private float invincibleTimer = 0f;

    [Header("Shooting")]
    public PlayerAttack playerAttack;
    public Transform gunPoint;
    public GameObject bulletPrefab;
    public Vector2 gunOffset = new Vector2(0.5f, 0.2f);
    public float bulletSpeed = 10f;
    public float attack = 5;
    public float shootCooldown = 0.2f;
    private float shootTimer = 0f;

    [Header("Ground Check")]
    [SerializeField] private float minGroundDistance = 1.5f;

    // 🔥 WALL SYSTEM
    [Header("Wall System")]
    [SerializeField] private float wallCheckDistance = 0.6f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private float wallStickTime = 1.5f;

    private bool isTouchingWall;
    private bool isWallSliding;
    private float wallTimer;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        healthMax = health;

        if (playerSprite == null)
            playerSprite = GetComponent<SpriteRenderer>();

        wallTimer = wallStickTime;
    }

    void Update()
    {
        if (canBeMoved)
        {
            Movement();
            Jump();
            Shoot();
        }

        // 🔥 WALL SYSTEM
        CheckWall();
        WallSlide();

        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
            return;
        }

        if (invincibleTimer > 0)
            invincibleTimer -= Time.deltaTime;

        if (shootTimer > 0f)
            shootTimer -= Time.deltaTime;

        if (Input.GetMouseButton(0) && playerAttack != null)
            playerAttack.Attack();

        FallDie();
    }

    // ---------------- Movement ----------------
    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");

        // 🔥 kalau wall slide, jangan gerak bebas
        if (!isWallSliding)
            rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

        if (x < 0)
            playerSprite.flipX = true;
        else if (x > 0)
            playerSprite.flipX = false;

        if (anim != null)
        {
            anim.SetBool("Run", x != 0);
        }
    }

    // ---------------- Jump ----------------
    void Jump()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, 10, LayerMask.GetMask("Obstacle"));

        if (ray && ray.distance < minGroundDistance)
            isGrounded = true;
        else
            isGrounded = false;

        // 🔥 WALL JUMP
        if (isWallSliding && Input.GetKeyDown(KeyCode.Space))
        {
            float direction = playerSprite.flipX ? 1 : -1;
            rb.linearVelocity = new Vector2(direction * moveSpeed, jumpForce);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // 🔥 CEK TEMBOK
    void CheckWall()
    {
        Vector2 direction = playerSprite.flipX ? Vector2.left : Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, wallLayer);

        isTouchingWall = hit;
    }

    // 🔥 WALL SLIDE
    void WallSlide()
    {
        if (isTouchingWall && !isGrounded && rb.linearVelocity.y < 0)
        {
            isWallSliding = true;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);

            wallTimer -= Time.deltaTime;

            if (wallTimer <= 0)
            {
                isWallSliding = false;
            }
        }
        else
        {
            isWallSliding = false;
            wallTimer = wallStickTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = true;

        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyControler enemy = collision.collider.GetComponent<EnemyControler>();
            if (enemy != null)
            {
                float damage = enemy.GetAttackDamage();
                DamagedBy(damage, collision.transform.position);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = false;
    }

    // ---------------- Shooting ----------------
    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && shootTimer <= 0f)
        {
            shootTimer = shootCooldown;

            int direction = playerSprite.flipX ? -1 : 1;

            Vector2 gunPos = (Vector2)transform.position +
                 new Vector2(Mathf.Abs(gunOffset.x) * direction, gunOffset.y);

            GameObject bullet = Instantiate(bulletPrefab, gunPos, Quaternion.identity);

            Bullet b = bullet.GetComponent<Bullet>();
            if (b != null)
            {
                b.Launch(new Vector2(direction, 0), "Enemy", bulletSpeed, attack);
            }

            SpriteRenderer bulletSprite = bullet.GetComponentInChildren<SpriteRenderer>();
            if (bulletSprite != null)
                bulletSprite.flipX = playerSprite.flipX;

            if (anim != null)
                anim.SetTrigger("isAttacking");

            Destroy(bullet, 5f);
        }
    }

    // ---------------- Damage System ----------------
    public void DamagedBy(float damage, Vector2 hitSource)
    {
        if (invincibleTimer > 0) return;

        if (Vector2.Distance(transform.position, hitSource) > 3f)
            return;

        invincibleTimer = invincibleTime;

        health -= damage;

        if (anim != null)
            anim.SetTrigger("Hit");

        float dir = transform.position.x > hitSource.x ? 1 : -1;
        Vector2 direction = new Vector2(dir, 0.5f);

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        knockbackTimer = knockbackDuration;

        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    void FallDie()
    {
        if (transform.position.y < -25)
        {
            Die();
        }
    }

    void Die()
    {
        playerSprite.enabled = false;
        canBeMoved = false;

        if (anim != null)
            anim.SetTrigger("Die");

        GameManager.GameOver();
    }

    // 🔥 FINAL TRIGGER SYSTEM (BULLET + SPIKE)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            Die();
            return;
        }

        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();

            if (bullet != null && bullet.targetTag == "Player")
            {
                float damage = bullet.GetDamage();
                DamagedBy(damage, collision.transform.position);
                Destroy(collision.gameObject);
            }
        }
    }
}