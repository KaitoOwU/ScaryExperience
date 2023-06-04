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

    
    int _currentLevel;
    public int CurrentLevel { get => _currentLevel; private set => _currentLevel = value; }
    public GameObject LoseScreen { get => _loseScreen; private set => _loseScreen = value; }
    public GameObject WinScreen { get => _winScreen; private set => _winScreen = value; }
    public GameObject PauseScreen { get => _pauseScreen; private set => _pauseScreen = value; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        //_flameManager.OnFlameValueChange += CheckForLoseCondition;
    }

    private void OnDisable()
    {
        //_flameManager.OnFlameValueChange -= CheckForLoseCondition;
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