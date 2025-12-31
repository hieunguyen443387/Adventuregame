using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Vector3 lastCheckpointPos;

    void Awake()
    {
        // Đảm bảo chỉ có 1 GameManager tồn tại
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject); // không bị hủy khi load scene khác
    }

    public void SetCheckpoint(Vector3 pos)
    {
        lastCheckpointPos = pos;
    }

    public Vector3 GetCheckpoint()
    {
        return lastCheckpointPos;
    }
}
