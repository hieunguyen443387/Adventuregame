using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string startSceneName = "SwampMap";

    [Header("UI")]
    [SerializeField] private GameObject continueButton;

    private void Start()
    {
        // Hiện Continue nếu có save
        continueButton.SetActive(SaveSystem.HasPlayed());
    }

    public void NewGame()
    {
        DataPersistanceManager.instance.NewGame();
        SceneManager.LoadScene(startSceneName);
    }

    public void ContinueGame()
    {
        DataPersistanceManager.instance.LoadGame();
        SceneManager.LoadScene(startSceneName);
    }
}