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
    public IReadOnlyDictionary<int, LevelData> LevelData { get => _levelData; }

    private void Awake()
    {
        Instance = this;

        if(SaveSystem.LoadData() == null)
        {
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
        Debug.LogWarning(flameValue);
        if(flameValue <= 0)
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
public struct LevelData
{
    public bool isUnlocked;
    public int coins;

    public LevelData(bool isUnlocked = false, int coins = 0)
    {
        this.isUnlocked = isUnlocked;
        this.coins = coins;
    }
}