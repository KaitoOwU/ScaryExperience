using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    
    public static void SaveData(Dictionary<int, LevelData> data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData saveData = new SaveData(data);
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
}
