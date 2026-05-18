using System.Collections;
using UnityEngine;

public class BreakPlatform : MonoBehaviour
{
    [Header("Break Settings")]
    public float breakDelay = 0.5f;
    public float shakeAmount = 0.03f;

    [Header("Destroy")]
    public float destroyTime = 2f;

    private Rigidbody2D rb;
    private Collider2D col;

    private bool isTriggered = false;
    private Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;

        startPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTriggered) return;

        if (collision.collider.CompareTag("Player"))
        {
            // Hanya aktif jika diinjak dari atas
            foreach (ContactPoint2D hit in collision.contacts)
            {
                if (hit.normal.y < -0.5f)
                {
                    isTriggered = true;
                    StartCoroutine(Break());
                    break;
                }
            }
        }
    }

    IEnumerator Break()
    {
        float timer = 0f;

        //  SHAKE
        while (timer < breakDelay)
        {
            timer += Time.deltaTime;

            float offsetX = Random.Range(-shakeAmount, shakeAmount);

            transform.position = startPos + new Vector3(offsetX, 0, 0);

            yield return null;
        }

        transform.position = startPos;

        // MATIKAN COLLIDER DULU
        col.enabled = false;

        // JATUH
        rb.bodyType = RigidbodyType2D.Dynamic;

        Destroy(gameObject, destroyTime);
    }
}