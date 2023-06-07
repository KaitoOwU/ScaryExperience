using NaughtyAttributes;
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

    [SerializeField] GameObject _loseScreen, _winScreen, _pauseScreen;
    [SerializeField] FlameManager _flameManager;
    [SerializeField] MoveBubble _moveBubble;

    [Header("-- Level Data --")]
    [SerializeField] bool _levelWithoutKey;
    [SerializeField] GameObject _grid;

    
    int _currentLevel;
    public int CurrentLevel { get => _currentLevel; private set => _currentLevel = value; }
    public GameObject LoseScreen { get => _loseScreen; private set => _loseScreen = value; }
    public GameObject WinScreen { get => _winScreen; private set => _winScreen = value; }
    public GameObject PauseScreen { get => _pauseScreen; private set => _pauseScreen = value; }
    public bool HaveKey { get => !_levelWithoutKey; }
    public GameObject Grid { get => _grid; }

    private void Awake()
    {
        Instance = this;
        if (_levelWithoutKey)
        {
            _grid.SetActive(false);
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

    public Vector3 DirectionAddMovePos(TileDown.Direction direction)
    {
        switch (direction)
        {
            case TileDown.Direction.Left:
                return new Vector3(-1, 0, 0);

            case TileDown.Direction.Right:
                return new Vector3(1, 0, 0);

            case TileDown.Direction.Up:
                return new Vector3(0, 1, 0);

            case TileDown.Direction.Down:
                return new Vector3(0, -1, 0);
        }

        return Vector3.zero;
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

    internal void SetTouchControlsActive(bool active)
    {
        _moveBubble.SetTouchControlsActive(active);
    }
}