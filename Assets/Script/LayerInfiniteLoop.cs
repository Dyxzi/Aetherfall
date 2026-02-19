using UnityEngine;

public class LayerInfiniteLoop : MonoBehaviour
{
    public Transform cam;

    private Transform[] tiles;
    private float spriteWidth;

    void Start()
    {
        tiles = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            tiles[i] = transform.GetChild(i);

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    void LateUpdate()
    {
        float camX = cam.position.x;

        foreach (Transform tile in tiles)
        {
            float distance = camX - tile.position.x;

            if (distance > spriteWidth)
            {
                tile.position += Vector3.right * spriteWidth * tiles.Length;
            }
            else if (distance < -spriteWidth)
            {
                tile.position -= Vector3.right * spriteWidth * tiles.Length;
            }
        }
    }
}
