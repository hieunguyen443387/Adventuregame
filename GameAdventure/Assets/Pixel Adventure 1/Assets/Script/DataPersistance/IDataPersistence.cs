using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public interface IDataPersistance
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
    
    // Additional methods for data persistence can be added here
}