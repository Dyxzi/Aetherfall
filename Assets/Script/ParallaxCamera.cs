using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;

    private float oldPosition;

    void Start()
    {
        oldPosition = transform.position.x;
    }

    void LateUpdate() // 🔥 PENTING: pakai LateUpdate
    {
        float newPosition = transform.position.x;

        if (newPosition != oldPosition)
        {
            float delta = newPosition - oldPosition; // 🔥 arah benar

            if (onCameraTranslate != null)
            {
                onCameraTranslate(delta);
            }

            oldPosition = newPosition;
        }
    }
}