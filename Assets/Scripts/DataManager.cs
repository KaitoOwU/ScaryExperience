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
    [SerializeField, ReadOnly] int _currentLevel, _skullObtained, _silverFlameObtained;
    private bool _isConnectedToGooglePlayServices = false;

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
    public int SkullObtained { get => _skullObtained; set => _skullObtained = value; }
    public int GoldenFlameObtained { get; set; }
    public int SilverFlameObtained { get => _silverFlameObtained; set => _silverFlameObtained = value; }


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
                SaveSystem.SaveData(_levelData, DeathAmount, SkullObtained, LevelCompletedAmount, GoldenFlameObtained, SilverFlameObtained);
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
                    SilverFlameObtained = 0;

                    SaveSystem.SaveData(_levelData, DeathAmount, SkullObtained, LevelCompletedAmount, GoldenFlameObtained, SilverFlameObtained);
                }
                else
                {
                    _levelData = data.levelData;
                    DeathAmount = data.numberOfDeaths;
                    SkullObtained = data.amountOfSkulls;
                    LevelCompletedAmount = data.amountOfLevelCompleted;
                    GoldenFlameObtained = data.goldenFlameObtained;
                    SilverFlameObtained = data.silverFlameObtained;
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

    public void CheckForGenericAchievement()
    {


        //CHECK LES ACHIEVEMENTS SUR LA MORT
        if(DeathAmount >= 1)
            Social.ReportProgress(GPGSIds.achievement_beggining_of_a_long_journey, 100f, null);

        Social.ReportProgress(GPGSIds.achievement_ouch, DeathAmount / 25f * 100f, null);
        Social.ReportProgress(GPGSIds.achievement_you_need_to_get_better, DeathAmount / 50f * 100f, null);
        Social.ReportProgress(GPGSIds.achievement_lost_cause, DeathAmount, null);

        //CHECK LES ACHIEVEMENTS SUR LE COLLECTIBLE
        Social.ReportProgress(GPGSIds.achievement_budding_archaeologist, SkullObtained / 10f * 100f, null);
        Social.ReportProgress(GPGSIds.achievement_where_are_they_all, SkullObtained / 20f * 100f, null);
        Social.ReportProgress(GPGSIds.achievement_gold_digger, SkullObtained / LevelData.Count * 100f, null);

        //CHECK LES ACHIEVEMENTS SUR LA FLAMME
        Social.ReportProgress(GPGSIds.achievement_step_saver, SilverFlameObtained / 10f * 100f, null);
        Social.ReportProgress(GPGSIds.achievement_unhealthy, SilverFlameObtained / 20f * 100f, null);
        Social.ReportProgress(GPGSIds.achievement_laziness_or_talent, SilverFlameObtained / LevelData.Count * 100f, null);

        //CHECK LES ACHIEVEMENTS SUR LES NIVEAUX
        if (LevelCompletedAmount >= LevelData.Count)
        {
            Social.ReportProgress(GPGSIds.achievement_beta_tester, 100f, null);

            if(SkullObtained >= LevelData.Count && SilverFlameObtained >= LevelData.Count)
            {
                Social.ReportProgress(GPGSIds.achievement_francko_would_be_proud, 100f, null);
            }

        }
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
    public FlameState FlameState { get => _state; set => _state = value; }

    public LevelData(int levelId)
    {
        _levelId = levelId;
        _isUnlocked = false;
        _collectibleAcquired = false;
        _state = FlameState.None;
    }

    public void Complete(bool withCollectible, FlameState flameState)
    {
        if (withCollectible && !_collectibleAcquired)
        {
            _collectibleAcquired = true;
            DataManager.Instance.SkullObtained++;
            Debug.LogError(DataManager.Instance.SkullObtained);
        }

        if(flameState > _state)
        {
            if(flameState == FlameState.Gold)
            {
                DataManager.Instance.GoldenFlameObtained++;
                if(_state == FlameState.None)
                {
                    DataManager.Instance.SilverFlameObtained++;
                }
            } else if(flameState == FlameState.Silver)
            {
                DataManager.Instance.SilverFlameObtained++;
            }

            _state = flameState;
            Debug.LogWarning(_state);
        }

        if (DataManager.Instance.LevelData.ContainsKey(_levelId + 1))
        {
            if(!DataManager.Instance.LevelData[_levelId + 1].IsUnlocked)
            {
                DataManager.Instance.LevelData[_levelId + 1].IsUnlocked = true;
                DataManager.Instance.LevelCompletedAmount++;
            }
        }

        SaveSystem.SaveData(DataManager.Instance.LevelData, DataManager.Instance.DeathAmount, DataManager.Instance.SkullObtained, DataManager.Instance.LevelCompletedAmount, DataManager.Instance.GoldenFlameObtained, DataManager.Instance.SilverFlameObtained);
        
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
