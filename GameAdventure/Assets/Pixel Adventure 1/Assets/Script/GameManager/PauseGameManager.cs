using UnityEngine;
using System.Collections.Generic;

public class PauseGameManager : MonoBehaviour
{
    public static PauseGameManager instance;
    private bool isPaused = false;
    public bool IsPaused => isPaused;
    private void Awake()
    {
        if (instance == null){
            instance = this;
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
    }

}
