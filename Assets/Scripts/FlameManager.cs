using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Rendering.Universal;
using UnityEngine.Rendering.Universal;
using System;
using DG.Tweening;
using static System.TimeZoneInfo;
using static Tile;
using NaughtyAttributes;

public class FlameManager : MonoBehaviour
{
    [SerializeField, ReadOnly] float _value;
    [SerializeField] int _max;
    [SerializeField] Light2D _lightBig;
    [SerializeField] Light2D _lightMedium;
    [SerializeField] Light2D _lightLittle;
    [SerializeField] Color _maxColor;
    [SerializeField] Color _minColor;

    [SerializeField] float _fadeDuration;

    public Action<float> OnFlameValueChange;
    public float MaxValue { get => _max; }
    public float Value { get => _value; }

    void Start()
    {
        _value = _max;
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
        if(amount == 1)
        {
            if (substract)
            {
                switch (_value)
                {
                    case 7:
                        /*_light.intensity = .9f;
                        DOTween.To(() => _light.pointLightOuterRadius, x => _light.pointLightOuterRadius = x, 2f, 1f).SetEase(Ease.OutExpo);*/
                        DOTween.To(() => _lightBig.intensity, x => _lightBig.intensity = x, 0, _fadeDuration).SetEase(Ease.OutExpo);
                        DOTween.To(() => _lightMedium.intensity, x => _lightMedium.intensity = x, 1, _fadeDuration).SetEase(Ease.OutExpo);
                        break;
                    case 4:
                        /*_light.intensity = .8f;
                        DOTween.To(() => _light.pointLightOuterRadius, x => _light.pointLightOuterRadius = x, 1f, 1f).SetEase(Ease.OutExpo);*/
                        DOTween.To(() => _lightMedium.intensity, x => _lightMedium.intensity = x, 0, _fadeDuration).SetEase(Ease.OutExpo);
                        DOTween.To(() => _lightLittle.intensity, x => _lightLittle.intensity = x, 1, _fadeDuration).SetEase(Ease.OutExpo);
                        break;
                    case 1:
                        /*DOTween.To(() => _light.pointLightOuterRadius, x => _light.pointLightOuterRadius = x, 0f, 1f).SetEase(Ease.OutExpo);*/
                        DOTween.To(() => _lightLittle.intensity, x => _lightLittle.intensity = x, 0, _fadeDuration).SetEase(Ease.OutExpo);
                        break;
                }
            }
            else
            {
                switch (_value)
                {
                    case 8:
                        /*_light.intensity = .9f;
                        DOTween.To(() => _light.pointLightOuterRadius, x => _light.pointLightOuterRadius = x, 2f, 1f).SetEase(Ease.OutExpo);*/
                        DOTween.To(() => _lightMedium.intensity, x => _lightMedium.intensity = x, 0, _fadeDuration).SetEase(Ease.OutExpo);
                        DOTween.To(() => _lightBig.intensity, x => _lightBig.intensity = x, 1, _fadeDuration).SetEase(Ease.OutExpo);
                        break;
                    case 5:
                        /*_light.intensity = .8f;
                        DOTween.To(() => _light.pointLightOuterRadius, x => _light.pointLightOuterRadius = x, 1f, 1f).SetEase(Ease.OutExpo);*/
                        DOTween.To(() => _lightLittle.intensity, x => _lightLittle.intensity = x, 0, _fadeDuration).SetEase(Ease.OutExpo);
                        DOTween.To(() => _lightMedium.intensity, x => _lightMedium.intensity = x, 1, _fadeDuration).SetEase(Ease.OutExpo);
                        break;
                    case 2:
                        /*DOTween.To(() => _light.pointLightOuterRadius, x => _light.pointLightOuterRadius = x, 0f, 1f).SetEase(Ease.OutExpo);*/
                        DOTween.To(() => _lightLittle.intensity, x => _lightLittle.intensity = x, 1, _fadeDuration).SetEase(Ease.OutExpo);
                        break;
                }
            }
            
        }
        else if(amount > 1)
        {
            switch (_value)
            {
                case <= 10 and > 7:
                    /*_light.intensity = .9f;
                    DOTween.To(() => _light.pointLightOuterRadius, x => _light.pointLightOuterRadius = x, 2f, 1f).SetEase(Ease.OutExpo);*/
                    DOTween.To(() => _lightLittle.intensity, x => _lightLittle.intensity = x, 0, _fadeDuration).SetEase(Ease.OutExpo);
                    DOTween.To(() => _lightMedium.intensity, x => _lightMedium.intensity = x, 0, _fadeDuration).SetEase(Ease.OutExpo);
                    DOTween.To(() => _lightBig.intensity, x => _lightBig.intensity = x, 1, _fadeDuration).SetEase(Ease.OutExpo);
                    break;
                case <= 7 and > 4:
                    /*_light.intensity = .8f;
                    DOTween.To(() => _light.pointLightOuterRadius, x => _light.pointLightOuterRadius = x, 1f, 1f).SetEase(Ease.OutExpo);*/
                    DOTween.To(() => _lightBig.intensity, x => _lightBig.intensity = x, 0, _fadeDuration).SetEase(Ease.OutExpo);
                    DOTween.To(() => _lightLittle.intensity, x => _lightLittle.intensity = x, 0, _fadeDuration).SetEase(Ease.OutExpo);
                    DOTween.To(() => _lightMedium.intensity, x => _lightMedium.intensity = x, 1, _fadeDuration).SetEase(Ease.OutExpo);
                    break;
                case <= 4 and > 1:
                    /*_light.intensity = .8f;
                    DOTween.To(() => _light.pointLightOuterRadius, x => _light.pointLightOuterRadius = x, 1f, 1f).SetEase(Ease.OutExpo);*/
                    DOTween.To(() => _lightBig.intensity, x => _lightBig.intensity = x, 0, _fadeDuration).SetEase(Ease.OutExpo);
                    DOTween.To(() => _lightMedium.intensity, x => _lightMedium.intensity = x, 0, _fadeDuration).SetEase(Ease.OutExpo);
                    DOTween.To(() => _lightLittle.intensity, x => _lightLittle.intensity = x, 1, _fadeDuration).SetEase(Ease.OutExpo);
                    break;
                case <= 1:
                    /*DOTween.To(() => _light.pointLightOuterRadius, x => _light.pointLightOuterRadius = x, 0f, 1f).SetEase(Ease.OutExpo);*/
                    DOTween.To(() => _lightLittle.intensity, x => _lightLittle.intensity = x, 0, _fadeDuration).SetEase(Ease.OutExpo);
                    DOTween.To(() => _lightBig.intensity, x => _lightBig.intensity = x, 0, _fadeDuration).SetEase(Ease.OutExpo);
                    DOTween.To(() => _lightMedium.intensity, x => _lightMedium.intensity = x, 0, _fadeDuration).SetEase(Ease.OutExpo);
                    break;
            }

        }
        
        _lightBig.color = new Color(Mathf.Lerp(_minColor.r, _maxColor.r, _value / 10), Mathf.Lerp(_minColor.g, _maxColor.g, _value / 10), Mathf.Lerp(_minColor.b, _maxColor.b, _value / 10));
        _lightMedium.color = new Color(Mathf.Lerp(_minColor.r, _maxColor.r, _value / 10), Mathf.Lerp(_minColor.g, _maxColor.g, _value / 10), Mathf.Lerp(_minColor.b, _maxColor.b, _value / 10));
        _lightLittle.color = new Color(Mathf.Lerp(_minColor.r, _maxColor.r, _value / 10), Mathf.Lerp(_minColor.g, _maxColor.g, _value / 10), Mathf.Lerp(_minColor.b, _maxColor.b, _value / 10));

        OnFlameValueChange?.Invoke(_value);
    }

    IEnumerator Fade(bool substract, Light2D light, float startValue, float endValue)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _fadeDuration)
        {
            float t = elapsedTime / _fadeDuration;

            light.pointLightOuterRadius = Mathf.Lerp(startValue, endValue, t);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        OnFlameValueChange?.Invoke(_value);
    }
}
