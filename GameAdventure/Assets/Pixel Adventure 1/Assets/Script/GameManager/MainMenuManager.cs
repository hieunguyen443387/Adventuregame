using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string startSceneName = "CastleBossMap";

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
        // set scene bắt đầu
        DataPersistanceManager.instance.gameData.lastSceneName = startSceneName;
        SceneManager.LoadScene(startSceneName);
    }

    public void ContinueGame()
    {
        var data = DataPersistanceManager.instance.gameData;
        data.isNewGame = false;   // 🔥 đánh dấu là continue

        if (data == null)
        {
            Debug.LogError("❌ No save data to continue");
            return;
        }

        Debug.Log("▶ Continue game → load scene: " + data.lastSceneName);
        SceneManager.LoadScene(data.lastSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Thoát game");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Dừng Play Mode
        #else
            Application.Quit(); // Thoát game khi build
        #endif
    }
}