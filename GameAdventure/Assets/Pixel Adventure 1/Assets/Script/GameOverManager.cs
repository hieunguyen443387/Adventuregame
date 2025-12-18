using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // Hàm gọi khi bấm nút "Play Again"
    public void PlayAgain()
    {
        // Lấy tên scene hiện tại và load lại
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    // Hàm gọi khi bấm "Quit"
    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("Game Quit!"); // chỉ hiện khi test trong Editor
    }
}