using DG.Tweening;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    [SerializeField] List<LevelLoadData> _levels;
    Dictionary<int, LevelData> _levelData;
    [SerializeField, ReadOnly] int _currentLevel;
    [SerializeField] TextMeshProUGUI _version, _logged;
    private bool _isConnectedToGooglePlayServices;

    public bool IsConnectedToGooglePlayServices { get => _isConnectedToGooglePlayServices; }
    public Dictionary<int, LevelData> LevelData { get => _levelData; }
    public IReadOnlyList<LevelLoadData> LevelList { get => _levels; }
    public int CurrentLevel { get => _currentLevel; set => _currentLevel = value; }
    public bool IsLevelLaunchedFromMainMenu { get; set; } = false;
    private string CurrentVersion { get => Application.version; }
    public bool IsMusicMuted { get; set; } = false;

    public bool IsSoundMuted { get; set; } = false;
    public int DeathAmount { get; set; }
    public int LevelCompletedAmount { get; set; }
    public int SkullObtained { get; set; }
    public int GoldenFlameObtained { get; set; }


    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        SceneManager.sceneLoaded += OnSceneLoaded;

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
                SaveSystem.SaveData(_levelData, DeathAmount, SkullObtained, LevelCompletedAmount);
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

                    DeathAmount = 0;
                    SkullObtained = 0;
                    LevelCompletedAmount = 0;
                    GoldenFlameObtained = 0;

                    SaveSystem.SaveData(_levelData, DeathAmount, SkullObtained, LevelCompletedAmount);
                }
                else
                {
                    _levelData = data.levelData;
                    DeathAmount = data.numberOfDeaths;
                    SkullObtained = data.amountOfSkulls;
                    LevelCompletedAmount = data.amountOfLevelCompleted;
                    GoldenFlameObtained = data.goldenFlameObtained;
                }
            }

            EnhancedTouchSupport.Enable();

            Instance = this;
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(gameObject);
        }

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == "MainMenu")
        {
            _version.text = "";
            _version.DOText("V." + Application.version, 1f);
            _logged.text = "";

            if (_isConnectedToGooglePlayServices)
            {
                _logged.color = Color.green;
                _logged.DOText("Connected to Google Play services", 1f);
            } else
            {
                _logged.color = Color.red;
                _logged.DOText("Not connected to Google Play services", 1f);
            }
        }
    }

    private void Start()
    {
        PlayGamesPlatform.Instance.Authenticate((result) =>
        {
            switch (result)
            {
                case SignInStatus.Success:
                    _isConnectedToGooglePlayServices = true;
                    break;
                default:
                    _isConnectedToGooglePlayServices = false;
                    break;
            }
        });
    }

    public bool AchievementToNextStep(string achievementId, float percentOfCompletion)
    {
        if (!_isConnectedToGooglePlayServices)
            return false;

        percentOfCompletion = Mathf.Clamp(percentOfCompletion, 0f, 100f);

        Social.ReportProgress(achievementId, percentOfCompletion, null);
        return true;
    }

}


[System.Serializable]
public class LevelData
{
    int _levelId;
    bool _isUnlocked;
    bool _collectibleAcquired;
    FlameState _state;

    public bool IsUnlocked { get => _isUnlocked; set => _isUnlocked = value; }
    public bool CollectibleAcquired { get => _collectibleAcquired; set => _collectibleAcquired = value; }

    public LevelData(int levelId)
    {
        _levelId = levelId;
        _isUnlocked = false;
        _collectibleAcquired = false;
        _state = FlameState.None;
    }

    public void Complete(bool withCollectible, FlameState flameState)
    {
        _state = flameState;

        if (!_collectibleAcquired)
        {
            _collectibleAcquired = withCollectible;
            DataManager.Instance.SkullObtained++;
        }
            

        if (DataManager.Instance.LevelData.ContainsKey(_levelId + 1))
        {
            if(!DataManager.Instance.LevelData[_levelId + 1].IsUnlocked)
            {
                DataManager.Instance.LevelData[_levelId + 1].IsUnlocked = true;
                DataManager.Instance.LevelCompletedAmount++;
            }
        }

        DataManager.Instance.AchievementToNextStep(GPGSIds.achievement_budding_archaeologist, (5f / DataManager.Instance.LevelList.Count) * 100f);
        DataManager.Instance.AchievementToNextStep(GPGSIds.achievement_gold_digger, (10f / DataManager.Instance.LevelList.Count) * 100f);
        DataManager.Instance.AchievementToNextStep(GPGSIds.achievement_the_feeling_of_a_job_well_done, (15f / DataManager.Instance.LevelList.Count) * 100f);
        DataManager.Instance.AchievementToNextStep(GPGSIds.achievement_franko_would_be_proud, (DataManager.Instance.SkullObtained / DataManager.Instance.LevelList.Count) * 100f);
        DataManager.Instance.AchievementToNextStep(GPGSIds.achievement_the_light_at_the_end_of_the_tunnel, (DataManager.Instance.LevelCompletedAmount / DataManager.Instance.LevelList.Count) * 100f);
    }
}

[System.Serializable]
public struct LevelLoadData
{
    public string _levelSceneName;
    public string _levelName;
}

public enum FlameState
{
    None,
    Silver,
    Gold
}
