using UnityEngine;

public class CameraZone : MonoBehaviour
{
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraFollow cam = Camera.main.GetComponent<CameraFollow>();

            if (cam != null)
            {
                cam.SetBounds(minBounds, maxBounds);
            }
        }
    }
}