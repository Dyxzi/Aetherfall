using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyControler : MonoBehaviour
{
    //basic status yang ada di enemy
    [Header("Status")]
    public float health = 20;
    public float attack = 5;
    public Transform attackTarget;
    [Header("Configuration")]
    [SerializeField] private float moveSpeed = 2.5f;
    // Update is called once per frame
    void Update()
    {
        Movement();
        FallDie();
    }
    protected virtual void Movement()
    {
    }
    //fungsi untuk mengurangi hp dari damage yang telah diterima
    public void DamagedBy(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
    //Fungsi untuk menghancurkan GameObject ketika jatuh ke bawah disaat
    //psosisi Y GameObject kurang dari -20
    void FallDie()
    {
        if (transform.position.y < -20)
        {
            Die();
        }
    }
    //fungsi penghancuran GameObject
    void Die()
    {
        Destroy(gameObject);
    }
    //fungsi mengambil nilai attack dari properti enemy
    public float GetAttackDamage()
    {
        return attack;
    }
    // bekerja saat collider "menyentuh" trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            //fungsi mengambil nilai damage dari bullet terlebih dahulu
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet.targetTag == "Enemy")
            {
                //lalu akan mengurangi darah dari GameObject ini ("enemy")
                //menggunakan fungsi DamagedBy()
                float damage = bullet.GetDamage();
                DamagedBy(damage);
                Destroy(collision.gameObject);
            }
        }
    }
}
