using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Etouch = UnityEngine.InputSystem.EnhancedTouch;

public class GameManager : MonoBehaviour
{
    [SerializeField] public TileMap tileMap;
    [SerializeField] public TileUpMap tileUpMap;

    [SerializeField] GameObject _loseScreen, _winScreen;
    [SerializeField] FlameManager _flameManager;

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
