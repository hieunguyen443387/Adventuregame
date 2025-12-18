using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public GameObject startButtonObject; // kéo thả GameObject của Button vào đây trong Inspector

    public void OnStart()
    {
        Debug.Log("🎮 Bắt đầu game!");

        // Ẩn nút Start
        if (startButtonObject != null)
        {
            startButtonObject.SetActive(false);
        }

        // Load Scene gameplay
        SceneManager.LoadScene("StartMap"); // đổi thành Scene gameplay của bạn
    }
}
