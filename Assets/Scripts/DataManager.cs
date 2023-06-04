using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [SerializeField] List<Level> _levels;
    Dictionary<int, LevelData> _levelData = new();
    public Dictionary<int, LevelData> LevelData { get => _levelData; }
    public IReadOnlyList<Level> LevelList { get => _levels; }

    public bool IsLevelLaunchedFromMainMenu { get; set; } = true;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);

        if(SaveSystem.LoadData() == null)
        {
            for (int i = 0; i < _levels.Count; i++)
            {
                _levelData[i] = new(i);
            }
            _levelData[0].IsUnlocked = true;
            SaveSystem.SaveData(_levelData);
        } else
        {
            _levelData = SaveSystem.LoadData().levelData;
        }
    }
}


[System.Serializable]
public class LevelData
{
    int _levelId;
    bool _isUnlocked;
    bool _collectibleAcquired;

    public bool IsUnlocked { get => _isUnlocked; set => _isUnlocked = value; }
    public bool CollectibleAcquired { get => _collectibleAcquired; set => _collectibleAcquired = value; }

    public LevelData(int levelId)
    {
        _levelId = levelId;
        _isUnlocked = false;
        _collectibleAcquired = false;
    }

    public void Complete(bool withCollectible)
    {

        _collectibleAcquired = withCollectible;
        if (DataManager.Instance.LevelData.ContainsKey(_levelId + 1))
        {
            DataManager.Instance.LevelData[_levelId + 1].IsUnlocked = true;
        }
    }
}
