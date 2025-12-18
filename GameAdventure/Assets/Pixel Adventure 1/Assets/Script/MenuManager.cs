using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Hàm này sẽ được gọi khi nhấn nút New Game
    public void NewGame()
    {
        // Load sang scene chơi game (ví dụ AdventureGame)
        SceneManager.LoadScene("StartMap");
    }

    // Nếu sau này bạn muốn có nút Quit
    public void QuitGame()
    {
        Debug.Log("Thoát game!");
        Application.Quit();
    }
}