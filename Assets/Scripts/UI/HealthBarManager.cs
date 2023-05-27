using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] FlameManager _flameManager;
    [SerializeField] Slider _frontBar, _backBar;

    private void Awake()
    {
        _flameManager.OnFlameValueChange += UpdateUI;

        _frontBar.maxValue = _flameManager.MaxValue;
        _frontBar.value = _frontBar.maxValue;

        _backBar.maxValue = _flameManager.MaxValue;
        _backBar.value = _backBar.maxValue;
    }

    private void UpdateUI(float value)
    {
        DOTween.Kill(this);
        _backBar.value = _frontBar.value;

        _frontBar.value--;
        _backBar.DOValue(_backBar.value - 1, 1.5f).SetEase(Ease.OutExpo);
    }
}
