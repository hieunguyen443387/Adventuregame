using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour
{
    public GameObject newGameButtonObject; // kéo thả GameObject của Button vào đây trong Inspector

    public void OnStart()
    {
        Debug.Log("🎮 Bắt đầu game!");

        // Ẩn nút Start
        if (newGameButtonObject != null)
        {
            newGameButtonObject.SetActive(false);
        }

        // Load Scene gameplay
        SceneManager.LoadScene("StartMap"); // đổi thành Scene gameplay của bạn
    }
}
