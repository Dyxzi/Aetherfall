using UnityEngine;

public class EnemyBoss : EnemyControler
{
    [Header("Boss Settings")]
    public float detectDistance = 8f;   // Jarak radar bos untuk menyadari keberadaan player
    public float attackDistance = 2f;   // Jarak minimal bos untuk mulai memukul player
    public float attackCooldown = 1.5f; // Waktu jeda (detik) antar serangan bos

    private float attackTimer;
    private SpriteRenderer sprite;

    protected override void Start()
    {
        // Mencari komponen gambar dan animasi di dalam objek atau anak objeknya (child)
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Movement();

        // Sistem hitung mundur jeda serangan (cooldown)
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    // Mengatur kecerdasan buatan (AI) pergerakan, pengejaran, dan penyerangan bos
    protected override void Movement()
    {
        if (attackTarget == null) return; // Jika player tidak ditemukan, bos diam

        // Menghitung jarak matematika antara posisi bos dan posisi player
        float distance = Vector2.Distance(transform.position, attackTarget.position);

        // Otomatis membalikkan gambar bos sesuai arah posisi player berada
        if (attackTarget.position.x < transform.position.x)
            sprite.flipX = true; // Menghadap kiri
        else
            sprite.flipX = false; // Menghadap kanan

        // KONDISI 1: Player masuk ke dalam area radar bos
        if (distance <= detectDistance)
        {
            // FASE A: Kejar Player (Jika jarak player masih lebih jauh dari jarak pukul)
            if (distance > attackDistance)
            {
                Vector2 dir = (attackTarget.position - transform.position).normalized;
                transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);

                if (anim != null)
                {
                    anim.SetBool("Run", true); // Aktifkan animasi lari
                }
            }
            // FASE B: Pukul Player (Jika jarak player sudah sangat dekat)
            else
            {
                if (anim != null)
                {
                    anim.SetBool("Run", false); // Berhenti lari saat memukul
                }

                // Eksekusi serangan jika waktu jeda (cooldown) sudah habis
                if (attackTimer <= 0)
                {
                    attackTimer = attackCooldown; // Reset timer jeda

                    if (anim != null)
                    {
                        anim.SetTrigger("Attack"); // Picu animasi menyerang
                    }

                    // Mengirim perintah ke script Player untuk mengurangi darahnya
                    PlayerMovment player = attackTarget.GetComponent<PlayerMovment>();
                    if (player != null)
                    {
                        player.DamagedBy(attack, transform.position);
                    }
                }
            }
        }
        // KONDISI 2: Player berada di luar radar bos
        else
        {
            if (anim != null)
            {
                anim.SetBool("Run", false); // Bos diam di tempat (Idle)
            }
        }
    }

    // Fungsi saat bos menerima serangan/damage dari Player
    public override void DamagedBy(float damage)
    {
        if (isDead) return; // Jika sudah mati, abaikan serangan baru

        health -= damage; // Kurangi darah bos

        if (anim != null)
        {
            anim.SetTrigger("Hit"); // Picu animasi berkedip/kesakitan
        }

        // KONDISI KEMATIAN BOS
        if (health <= 0)
        {
            isDead = true;
            health = 0;
            moveSpeed = 0;

            // Mematikan sistem fisika tubuh bos agar tidak bisa didorong/menabrak player lagi
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

            if (anim != null)
            {
                anim.SetTrigger("Die"); // Picu animasi mati
            }

            // Memunculkan item hadiah berupa kristal di koordinat tempat bos mati
            if (crystalPrefab != null)
            {
                Instantiate(crystalPrefab, transform.position, Quaternion.identity);
            }

            // Melaporkan ke ScoreManager bahwa bos sudah kalah untuk menambah progres level
            ScoreManager.DefeatEnemy();

            // Menghapus jasad bos dari game setelah 1 detik demi menghemat RAM
            Destroy(gameObject, 1f);
        }
    }
}