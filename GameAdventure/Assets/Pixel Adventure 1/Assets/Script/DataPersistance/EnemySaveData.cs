using System;
using UnityEngine;

[Serializable]
public class EnemySaveData
{
    public string enemyID;      // ID duy nhất
    public bool isDead;
    public int currentHealth;
    public Vector3 position;
}