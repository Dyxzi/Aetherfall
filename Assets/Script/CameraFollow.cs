
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed;
    public Vector3 offset;

    public Transform backgound;
    public float bgWidth;

    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null)
        {
            return;
        }
        Vector3 camPos = player.position + offset;
        Vector3 smoothMove = Vector3.Lerp(transform.position, camPos, smoothSpeed);
        transform.position = smoothMove;

        if (backgound == null)
        {
            return;
        }
        float camX = transform.position.x;
        float bgX = backgound.position.x;

        if (Mathf.Abs(camX - bgX) >= bgWidth)
        {
            float offsetX = (camX - bgX) % bgWidth;
            backgound.position = new Vector3(camX + offsetX, backgound.position.y, backgound.position.z);
        }
    } //test
}
