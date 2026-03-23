using UnityEngine;

public class DataPersistanceManager : MonoBehaviour
{
    public static DataPersistanceManager instance { get; private set; }
    public GameData gameData;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadGame();
    }

    // =====================================================
    // 🎮 NEW GAME / PLAY AGAIN
    public void NewGame()
    {
        Debug.Log("🆕 NEW GAME — RESET SAVE");

        // ❌ Xóa save cũ hoàn toàn
        SaveSystem.DeleteSave();

        // ✅ Tạo data mới sạch
        gameData = new GameData();

        gameData.playerHealth = 3;
        gameData.playerPosition = Vector3.zero;
        gameData.lastSceneName = "CastleBossMap"; // ⭐ scene bắt đầu
        gameData.isNewGame = true;

        // ✅ Ghi đè JSON mới
        SaveGame();
    }

    // =====================================================
    // 📂 LOAD
    public void LoadGame()
    {
        gameData = SaveSystem.Load();

        if (gameData == null)
        {
            Debug.Log("⚠ No save found → create new game");
            NewGame();
        }
    }

    // =====================================================
    // 💾 SAVE
    public void SaveGame()
    {
        if (gameData == null)
            gameData = new GameData();

        SaveSystem.Save(gameData);
    }

    // =====================================================
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}