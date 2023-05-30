using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Dictionary<int, LevelData> levelData;

    public SaveData(Dictionary<int, LevelData> levelData)
    {
        this.levelData = levelData;
    }
}
