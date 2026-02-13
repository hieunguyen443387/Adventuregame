using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PauseGameManager : MonoBehaviour
{
    public static PauseGameManager instance;
    private bool isPaused = false;
    public bool IsPaused => isPaused;
    [Header("Quit Game UI")]
    public GameObject quitGameUI;
    private void Awake()
    {
        if (instance == null){
            instance = this;
        }
    }

    void Start()
    {
        if (quitGameUI != null)
        {
            quitGameUI.SetActive(false);
        }
    }
    
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Debug.Log("GAME PAUSED");
    }

    public void UnpauseGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        Debug.Log("GAME RESUMED");
    }

    public void TogglePause()
    {
        if (isPaused)
            UnpauseGame();
        else
            PauseGame();
            quitGameUI.SetActive(isPaused);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("Game Quit!"); // chỉ hiện khi test trong Editor
    }

}
