using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    [SerializeField] List<LevelLoadData> _levels;
    Dictionary<int, LevelData> _levelData;
    [SerializeField, ReadOnly] int _currentLevel;

    public Dictionary<int, LevelData> LevelData { get => _levelData; }
    public IReadOnlyList<LevelLoadData> LevelList { get => _levels; }
    public int CurrentLevel { get => _currentLevel; set => _currentLevel = value; }
    public bool IsLevelLaunchedFromMainMenu { get; set; } = true;
    private string CurrentVersion { get => Application.version; }
    public bool IsMusicMuted { get; set; } = false;

    public bool IsSoundMuted { get; set; } = false;


    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        if (Instance == null)
        {
            _levelData = new();
            for (int i = 0; i < _levels.Count; i++)
            {
                _levelData[i] = new(i);
                if (i == 0)
                {
                    _levelData[i].IsUnlocked = true;
                }
            }

            var data = SaveSystem.LoadData();
            if (data == null)
            {
                Debug.LogWarning("No level data detected");
                for (int i = 0; i < _levels.Count; i++)
                {
                    _levelData[i] = new(i);
                    if (i == 0)
                    {
                        _levelData[i].IsUnlocked = true;
                    }
                }
                SaveSystem.SaveData(_levelData);
            }
            else
            {
                if (CurrentVersion != data.version)
                {
                    SaveSystem.RemoveData();
                    for (int i = 0; i < _levels.Count; i++)
                    {
                        _levelData[i] = new(i);
                        if (i == 0)
                        {
                            _levelData[i].IsUnlocked = true;
                        }
                    }
                    SaveSystem.SaveData(_levelData);
                }
                else
                {
                    _levelData = data.levelData;
                }
            }

            EnhancedTouchSupport.Enable();

            Instance = this;
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(gameObject);
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
