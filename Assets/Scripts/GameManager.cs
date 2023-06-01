using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Etouch = UnityEngine.InputSystem.EnhancedTouch;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] public TileMap tileMap;
    [SerializeField] public TileUpMap tileUpMap;

    [SerializeField] GameObject _loseScreen, _winScreen;
    [SerializeField] FlameManager _flameManager;

    Dictionary<int, LevelData> _levelData = new();
    int _currentLevel;
    public Dictionary<int, LevelData> LevelData { get => _levelData; }
    public int CurrentLevel { get => _currentLevel; private set => _currentLevel = value; }
    public GameObject LoseScreen { get => _loseScreen; set => _loseScreen = value; }
    public GameObject WinScreen { get => _winScreen; set => _winScreen = value; }

    private void Awake()
    {
        Instance = this;

        if(SaveSystem.LoadData() == null)
        {
            _levelData[0] = new(0);
            _levelData[0].IsUnlocked = true;
            SaveSystem.SaveData(_levelData);
        } else
        {
            _levelData = SaveSystem.LoadData().levelData;
        }
    }

    private void OnEnable()
    {
        _flameManager.OnFlameValueChange += CheckForLoseCondition;
    }

    private void OnDisable()
    {
        _flameManager.OnFlameValueChange -= CheckForLoseCondition;
    }

    private void CheckForLoseCondition(float flameValue)
    {
        //Debug.LogWarning(flameValue);
        if(flameValue <= 0 && _loseScreen != null)
        {
            _loseScreen.SetActive(true);
        }
    }

    private void Start()
    {
        Etouch.EnhancedTouchSupport.Enable();
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
        if (GameManager.Instance.LevelData.ContainsKey(_levelId+1))
        {
            GameManager.Instance.LevelData[_levelId + 1].IsUnlocked = true;
        }
    }
}