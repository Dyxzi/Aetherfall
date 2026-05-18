using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Range(0f, 1f)]
    public float parallaxFactor = 0.5f;

    private Transform cam;
    private Vector3 startPosition;

    void Start()
    {
        cam = Camera.main.transform;

        startPosition = transform.position - new Vector3(cam.position.x * parallaxFactor, 0, 0);
    }

    void LateUpdate()
    {
        float newX = startPosition.x + cam.position.x * parallaxFactor;

        transform.position = new Vector3(
            newX,
            transform.position.y,
            transform.position.z
        );
    }
}