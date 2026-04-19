using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    // 🔥 batas camera
    public Vector2 minBounds;
    public Vector2 maxBounds;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPos = player.position + offset;

        // 🔥 clamp posisi camera
        float clampedX = Mathf.Clamp(targetPos.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(targetPos.y, minBounds.y, maxBounds.y);

        Vector3 smoothPos = Vector3.Lerp(transform.position, new Vector3(clampedX, clampedY, transform.position.z), smoothSpeed * Time.deltaTime);

        transform.position = smoothPos;
    }

    // 🔥 dipanggil dari trigger
    public void SetBounds(Vector2 min, Vector2 max)
    {
        minBounds = min;
        maxBounds = max;
    }
}