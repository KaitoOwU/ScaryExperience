using UnityEngine;
using System;
using Cinemachine;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine.Rendering.Universal;
using UnityEditor;

public class FlameManager : MonoBehaviour
{
    [SerializeField, ReadOnly] float _value;
    [SerializeField] int _max;

    [SerializeField] Light2D _light;
    [SerializeField] Color _maxColor;
    [SerializeField] Color _minColor;

    [SerializeField] CinemachineVirtualCamera _vCam;
    CinemachineBasicMultiChannelPerlin _noise;

    float _maxSizeLight;

    [SerializeField] MonsterSpawn monsterSpawn;

    [SerializeField] float _fadeDuration;

    public Action<float> OnFlameValueChange;
    public float MaxValue { get => _max; }
    public float Value { get => _value; }

    void Start()
    {
        _noise = _vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _value = _max;
        _maxSizeLight = _light.pointLightOuterRadius;
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
        if(_value == 0)
        {
            DOTween.To(() => _light.pointLightOuterRadius, x => _light.pointLightOuterRadius = x, 0, 0.5f).SetEase(Ease.OutExpo);
            DOTween.To(() => _light.intensity, x => _light.intensity = x, 0, 0.5f).SetEase(Ease.OutExpo);
        }
        else
        {
            DOTween.To(() => _light.pointLightOuterRadius, x => _light.pointLightOuterRadius = x, _maxSizeLight - (_maxSizeLight / 14) * (10 - _value), 0.5f).SetEase(Ease.OutExpo);
            DOTween.To(() => _light.intensity, x => _light.intensity = x, 1.5f + (0.15f * _value), 0.5f).SetEase(Ease.OutExpo);
        }
        
        _light.color = new Color(Mathf.Lerp(_minColor.r, _maxColor.r, _value / 10), Mathf.Lerp(_minColor.g, _maxColor.g, _value / 10), Mathf.Lerp(_minColor.b, _maxColor.b, _value / 10));
        monsterSpawn._radius = _light.pointLightOuterRadius;
        OnFlameValueChange?.Invoke(_value);
        if(_value <= 5)
        {
            _noise.m_AmplitudeGain = 0.2f * (5 - _value);
            _noise.m_FrequencyGain = 0.2f * (5 - _value);
        }
        else
        {
            _noise.m_AmplitudeGain = 0;
            _noise.m_FrequencyGain = 0;
        }
    }

    
}
