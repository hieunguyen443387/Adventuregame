using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameData
{
    public int playerHealth;

    public GameData()
    {
        playerHealth = 3; // máu mặc định
    }
}