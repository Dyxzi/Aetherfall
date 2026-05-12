using UnityEngine;

public class SimpleParallax : MonoBehaviour
{
    public Transform cam;

    [Header("Parallax")]
    public float parallaxEffect = 0.2f;

    [Header("Loop")]
    public float spriteWidth = 20f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void LateUpdate()
    {
        if (cam == null) return;

        // 🔥 PARALLAX
        float movement = cam.position.x * parallaxEffect;

        transform.position = new Vector3(
            startPos.x + movement,
            transform.position.y,
            transform.position.z
        );

        // 🔥 INFINITE LOOP
        float distance = cam.position.x - transform.position.x;

        if (distance > spriteWidth)
        {
            startPos.x += spriteWidth * 3;
        }
        else if (distance < -spriteWidth)
        {
            startPos.x -= spriteWidth * 3;
        }
    }
}