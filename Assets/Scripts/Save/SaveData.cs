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
    public int goldenFlameObtained;
    public int silverFlameObtained;
    public string currentVersion;

    public SaveData(Dictionary<int, LevelData> levelData, int numberOfDeaths, int amountOfSkulls, int amountOfLevelCompleted, int goldenFlameObtained, int silverFlameObtained, string currentVersion)
    {
        this.levelData = levelData;
        this.numberOfDeaths = numberOfDeaths;
        this.amountOfSkulls = amountOfSkulls;
        this.amountOfLevelCompleted = amountOfLevelCompleted;
        version = Application.version;
        this.goldenFlameObtained = goldenFlameObtained;
        this.silverFlameObtained = silverFlameObtained;
        this.currentVersion = currentVersion;
    }
}
