using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    [Header("Player Components")]
    public SpriteRenderer playerSprite;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    private Rigidbody2D rb;
    private bool isGrounded;
    public Animator anim;

    [Header("Player Status")]
    public float health = 100f;
    public float healthMax;
    public bool canBeMoved = true;

    [Header("Shooting")]
    public PlayerAttack playerAttack;
    public GameObject bulletPrefab;
    public Vector2 gunOffset = new Vector2(0.5f, 0.2f);
    public float bulletSpeed = 10f;
    public float attack = 5f;
    public float shootCooldown = 0.2f;
    private float shootTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        healthMax = health;

        if (playerSprite == null)
            playerSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (canBeMoved)
        {
            Movement();
            Jump();
            Shoot();
        }

        if (shootTimer > 0f)
            shootTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && playerAttack != null)
            playerAttack.Attack();

        FallDie();
    }

    // ---------------- Movement ----------------
    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
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
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = true;

        // Damage dari enemy
        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyControler enemy = collision.collider.GetComponent<EnemyControler>();
            if (enemy != null)
            {
                float damage = enemy.GetAttackDamage();
                DamagedBy(damage);
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
        if (Input.GetMouseButton(0) && shootTimer <= 0f)
        {
            shootTimer = shootCooldown;

            int direction = playerSprite.flipX ? -1 : 1;

            Vector2 spawnPos = new Vector2(
                transform.position.x + gunOffset.x * direction,
                transform.position.y + gunOffset.y
            );

            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
                bulletRb.linearVelocity = new Vector2(direction * bulletSpeed, 0);

            SpriteRenderer bulletSprite = bullet.GetComponentInChildren<SpriteRenderer>();
            if (bulletSprite != null)
                bulletSprite.flipX = (direction == -1);
        }
    }

    // ---------------- Damage System ----------------
    public void DamagedBy(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    // Mati jika jatuh
    void FallDie()
    {
        if (transform.position.y < -20)
        {
            Die();
        }
    }

    // Player mati
    void Die()
    {
        playerSprite.enabled = false;
        canBeMoved = false;

        if (anim != null)
            anim.SetTrigger("Die");

        GameManager.GameOver();
    }

    // Kena peluru
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();

            if (bullet != null && bullet.targetTag == "Player")
            {
                float damage = bullet.GetDamage();
                DamagedBy(damage);
                Destroy(collision.gameObject);
            }
        }
    }
}