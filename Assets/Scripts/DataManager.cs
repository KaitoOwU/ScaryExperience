using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [SerializeField] List<LevelLoadData> _levels;
    Dictionary<int, LevelData> _levelData = new();
    [SerializeField, ReadOnly] int _currentLevel;

    public Dictionary<int, LevelData> LevelData { get => _levelData; }
    public IReadOnlyList<LevelLoadData> LevelList { get => _levels; }
    public int CurrentLevel { get => _currentLevel; set => _currentLevel = value; }
    public bool IsLevelLaunchedFromMainMenu { get; set; } = true;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();

        Instance = this;
        DontDestroyOnLoad(this);

        for (int i = 0; i < _levels.Count; i++)
        {
            _levelData[i] = new(i);
        }
        _levelData[0].IsUnlocked = true;
        SaveSystem.SaveData(_levelData);
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

        if(!_collectibleAcquired)
            _collectibleAcquired = withCollectible;

        if (DataManager.Instance.LevelData.ContainsKey(_levelId + 1))
        {
            DataManager.Instance.LevelData[_levelId + 1].IsUnlocked = true;
        }
    }
}

[System.Serializable]
public struct LevelLoadData
{
    public string _levelSceneName;
    public string _levelName;
}
