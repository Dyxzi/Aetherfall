using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor = 0.5f;

    void Start()
    {
        if (Camera.main == null) return;

        ParallaxCamera parallaxCam = Camera.main.GetComponent<ParallaxCamera>();

        if (parallaxCam != null)
        {
            parallaxCam.onCameraTranslate += Move;
        }
    }

    // 🔥 WAJIB PUBLIC
    public void Move(float delta)
    {
        Vector3 newPos = transform.position;
        newPos.x += delta * parallaxFactor;

        transform.position = newPos;
    }
}