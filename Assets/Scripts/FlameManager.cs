using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Rendering.Universal;
using UnityEngine.Rendering.Universal;
using System;
using DG.Tweening;
using static System.TimeZoneInfo;

public class FlameManager : MonoBehaviour
{
    float _value;
    [SerializeField] int _max;
    [SerializeField] Light2D _light;
    [SerializeField] float _maxRadius;
    [SerializeField] Color _maxColor;
    [SerializeField] Color _minColor;

    [SerializeField] float _fadeDuration;

    public Action<float> OnFlameValueChange;
    public float MaxValue { get => _max; }

    void Start()
    {
        _value = _max;
        _maxRadius = _light.pointLightOuterRadius;
    }


    public void ModifyFlame(bool substract, int amount)
    {
        if (substract)
        {
            _value -= amount;
        }
        else 
        {
            _value += amount;
        }
        _value = Mathf.Clamp(_value, 0, _max);
        switch (_value)
        {
            case 10:
                _light.intensity = 1;
                DOTween.To(() => _light.pointLightOuterRadius, x => _light.pointLightOuterRadius = x, 2.5f, 1f).SetEase(Ease.OutExpo);
                break;
            case 7:
                _light.intensity = .9f;
                DOTween.To(() => _light.pointLightOuterRadius, x => _light.pointLightOuterRadius = x, 2f, 1f).SetEase(Ease.OutExpo);
                break;
            case 4:
                _light.intensity = .8f;
                DOTween.To(() => _light.pointLightOuterRadius, x => _light.pointLightOuterRadius = x, 1f, 1f).SetEase(Ease.OutExpo);
                break;
            case 1:
                DOTween.To(() => _light.pointLightOuterRadius, x => _light.pointLightOuterRadius = x, 0f, 1f).SetEase(Ease.OutExpo);
                break;
        }
        _light.color = new Color(Mathf.Lerp(_minColor.r, _maxColor.r, _value / 10), Mathf.Lerp(_minColor.g, _maxColor.g, _value / 10), Mathf.Lerp(_minColor.b, _maxColor.b, _value / 10));

        OnFlameValueChange?.Invoke(_value);
    }
}
