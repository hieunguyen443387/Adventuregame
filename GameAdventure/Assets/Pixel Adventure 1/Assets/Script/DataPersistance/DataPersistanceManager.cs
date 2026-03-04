using System.Collections.Generic;
using UnityEngine;
using System.Collections;

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

        LoadGame(); // 🔥 BẮT BUỘC
    }

    // New Game
    public void NewGame()
    {
        gameData = new GameData();
        SaveSystem.Save(gameData); // tạo file save ngay
    }

    // Load Game-
    public void LoadGame()
    {
        gameData = SaveSystem.Load();

        if (gameData == null)
        {
            Debug.Log("⚠ No save found → create new game");
            NewGame();
        }
    }

    // Save Game
    public void SaveGame()
    {
        if (gameData == null)
            gameData = new GameData();

        SaveSystem.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}