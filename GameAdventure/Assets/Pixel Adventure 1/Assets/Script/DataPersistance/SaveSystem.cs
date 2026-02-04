using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string savePath => Application.persistentDataPath + "/save.json";

    // Lưu game
    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("💾 Game Saved: " + savePath);
    }

    // Load game
    public static GameData Load()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("❌ No save file found");
            return null;
        }

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<GameData>(json);
    }

    // Kiểm tra đã từng chơi chưa
    public static bool HasPlayed()
    {
        return File.Exists(savePath);
    }

    // Xóa save (debug)
    public static void DeleteSave()
    {
        if (File.Exists(savePath))
            File.Delete(savePath);
    }
}