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
public class LevelData
{
    int _levelId;
    bool _isUnlocked;
    int _coins;

    public bool IsUnlocked { get => _isUnlocked; set => _isUnlocked = value; }
    public int Coins { get => _coins; set => _coins = value; }

    public LevelData(int levelId)
    {
        _levelId = levelId;
        _isUnlocked = false;
        _coins = 0;
    }

    public void Complete(int amountOfCoins)
    {
        if (0 > amountOfCoins && amountOfCoins > 3)
        {
            throw new ArgumentException("Nique ta mère la pute on a que 3 coins max gros débile.");
        }

        _coins = amountOfCoins;
        if (GameManager.Instance.LevelData.ContainsKey(_levelId+1))
        {
            GameManager.Instance.LevelData[_levelId + 1].IsUnlocked = true;
        }
    }
}