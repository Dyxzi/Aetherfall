using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    [Header("Player Components")]
    public SpriteRenderer playerSprite; // SpriteRenderer Player

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    private Rigidbody2D rb;
    private bool isGrounded;
    public Animator anim;

    [Header("Shooting")]
    public PlayerAttack playerAttack;
    public GameObject bulletPrefab; // drag Bullet prefab dari Project
    public Vector2 gunOffset = new Vector2(0.5f, 0.2f);
    public float bulletSpeed = 10f;
    public float attack = 5f;
    public float shootCooldown = 0.2f;
    private float shootTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (playerSprite == null)
            playerSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Movement();
        Jump();
        Shoot();

        if (shootTimer > 0f)
            shootTimer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
            playerAttack.Attack();
    }

    // ---------------- Movement ----------------
    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

        // Flip Player sprite
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

            // Spawn position
            Vector2 spawnPos = new Vector2(
                transform.position.x + gunOffset.x * direction,
                transform.position.y + gunOffset.y
            );

            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

            // Launch bullet
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
                bulletRb.linearVelocity = new Vector2(direction * bulletSpeed, 0);

            // Flip bullet sprite if needed
            SpriteRenderer bulletSprite = bullet.GetComponentInChildren<SpriteRenderer>();
            if (bulletSprite != null)
                bulletSprite.flipX = (direction == -1);
        }
    }
}