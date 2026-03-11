using UnityEngine;
using UnityEngine.Tilemaps;

public class BackGroundTilemapController : MonoBehaviour
{
    private float startPos, length;

    public GameObject cam;
    public float parallaxEffect;

    private Tilemap tilemap;

    void Start()
    {
        startPos = transform.position.x;

        tilemap = GetComponent<Tilemap>();

        length = tilemap.cellBounds.size.x * tilemap.layoutGrid.cellSize.x;
    }

    void Update()
    {
        float distance = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        float movement = cam.transform.position.x * (1 - parallaxEffect);

        // if (movement > startPos + length / 2f)
        //     startPos += length;
        // else if (movement < startPos - length / 2f)
        //     startPos -= length;
    }
}