using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public float FollowSpeed = 2f;
    public float yOffset = 1f;
    public Transform target;

    [Header("Map Settings")]
    public Tilemap tilemap;   // Gán Tilemap vào đây

    private Vector3 minBounds;
    private Vector3 maxBounds;
    private float camHalfHeight;
    private float camHalfWidth;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        // Tính nửa chiều cao / rộng camera
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = cam.aspect * camHalfHeight;

        // Lấy giới hạn từ Tilemap
        Bounds mapBounds = tilemap.localBounds;
        minBounds = mapBounds.min;
        maxBounds = mapBounds.max;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Vị trí muốn follow
            Vector3 targetPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);

            // Smooth follow
            Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, FollowSpeed * Time.deltaTime);

            // Clamp theo tilemap bounds
            float clampX = Mathf.Clamp(smoothPos.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
            float clampY = Mathf.Clamp(smoothPos.y, minBounds.y + camHalfHeight, maxBounds.y - camHalfHeight);

            transform.position = new Vector3(clampX, clampY, smoothPos.z);
        }
    }
}
