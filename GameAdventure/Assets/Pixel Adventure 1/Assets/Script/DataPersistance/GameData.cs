using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    // ===== PLAYER =====
    public int playerHealth;
    public Vector3 playerPosition;
    public bool isNewGame;

    // ===== SCENE / MAP =====
    public string lastSceneName;

    // ===== ENEMIES =====
    public List<EnemySaveData> enemies;

    public GameData()
    {
        playerHealth = 3;
        playerPosition = Vector3.zero;
        lastSceneName = "CastleBossMap";
        enemies = new List<EnemySaveData>();
        isNewGame = true; // 🔥 mặc định là game mới
    }
}