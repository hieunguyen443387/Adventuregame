using UnityEngine;

public class CameraChunkLoader : MonoBehaviour
{
    public Transform cam;
    public GameObject contentRoot;
    public float loadDistance = 5f;

    void Start()
    {
        UpdateLoadState();
    }

    void Update()
    {
        UpdateLoadState();
    }

    void UpdateLoadState()
    {
        float dist = Mathf.Abs(cam.position.x - transform.position.x);
        bool shouldLoad = dist < loadDistance;

        if (contentRoot.activeSelf != shouldLoad)
            contentRoot.SetActive(shouldLoad);
    }
}