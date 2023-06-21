using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    
    public static void SaveData(Dictionary<int, LevelData> data, int numberOfDeaths, int numberOfSkulls, int levelCompletedAmount, int goldenFlameNeeded, int silverFlameNeeded, string currentVersion)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData saveData = new SaveData(data, numberOfDeaths, numberOfSkulls, levelCompletedAmount, goldenFlameNeeded, silverFlameNeeded, currentVersion);
        bf.Serialize(stream, saveData);
        stream.Close();
    }

    public static SaveData LoadData()
    {
        string path = Application.persistentDataPath + "/.data";
        if(File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData saveData = bf.Deserialize(stream) as SaveData;
            stream.Close();

            return saveData;
        } else
        {
            return null;
        }
    }

    public static void RemoveData()
    {
        string path = Application.persistentDataPath + "/.data";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
