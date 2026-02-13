using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    // ===== PLAYER =====
    public int playerHealth;
    public Vector3 playerPosition;

    // ===== SCENE / MAP =====
    public string lastSceneName;

    // ===== ENEMIES =====
    public List<EnemySaveData> enemies;

    public GameData()
    {
        playerHealth = 3;
        playerPosition = Vector3.zero;
        lastSceneName = "SwampMap";
        enemies = new List<EnemySaveData>();
    }
}