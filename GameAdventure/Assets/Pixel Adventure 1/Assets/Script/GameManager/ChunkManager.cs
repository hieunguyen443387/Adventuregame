using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject[] chunks;

    public int tilesPerChunk = 64;
    public int preloadTiles = 10; // load trước 10 tile

    void Update()
    {
        int camTileX = Mathf.FloorToInt( mainCamera.transform.position.x);

        int currentChunkIndex = (camTileX + preloadTiles) / tilesPerChunk;

        for (int i = 0; i < chunks.Length; i++)
        {
            bool shouldBeActive = Mathf.Abs(i - currentChunkIndex) <= 1;

            if (chunks[i].activeSelf != shouldBeActive)
                chunks[i].SetActive(shouldBeActive);
        }
    }
}