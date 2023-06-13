using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Dictionary<int, LevelData> levelData;
    public string version;
    public int numberOfDeaths;
    public int amountOfSkulls;
    public int amountOfLevelCompleted;

    public SaveData(Dictionary<int, LevelData> levelData, int numberOfDeaths, int amountOfSkulls, int amountOfLevelCompleted)
    {
        this.levelData = levelData;
        this.numberOfDeaths = numberOfDeaths;
        this.amountOfSkulls = amountOfSkulls;
        this.amountOfLevelCompleted = amountOfLevelCompleted;
        version = Application.version;
    }
}
