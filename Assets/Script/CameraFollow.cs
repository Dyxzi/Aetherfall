using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 camPos = player.position + offset;

        float camHalfHeight = Camera.main.orthographicSize;
        float camHalfWidth = camHalfHeight * Screen.width / Screen.height;

        camPos.x = Mathf.Clamp(camPos.x, minX + camHalfWidth, maxX - camHalfWidth);
        camPos.y = Mathf.Clamp(camPos.y, minY + camHalfHeight, maxY - camHalfHeight);

        transform.position = Vector3.Lerp(
            transform.position,
            camPos,
            smoothSpeed * Time.deltaTime
        );
    }
}