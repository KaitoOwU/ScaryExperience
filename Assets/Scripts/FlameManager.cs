using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class FlameManager : MonoBehaviour
{
    float _value;
    [SerializeField] int _max;
    [SerializeField] Light2D _light;
    [SerializeField] float _maxRadius;
    [SerializeField] Color _maxColor;
    [SerializeField] Color _minColor;

    [SerializeField] float _fadeDuration;
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
        Debug.LogWarning(_value);
        switch (_value)
        {
            case <= 10 and > 7:
                StartCoroutine(Fade(substract, _light, _light.pointLightOuterRadius, _maxRadius));
                _light.intensity = 1;

                break;
            case <= 7 and > 4:
                StartCoroutine(Fade(substract, _light, _light.pointLightOuterRadius, (int)(2 * (_maxRadius / 3))));
                _light.intensity = 1;
                break;
            case <= 4 and > 1:
                StartCoroutine(Fade(substract, _light, _light.pointLightOuterRadius, (int)(_maxRadius / 3)));
                _light.intensity = 1;
                break;
            case <= 1:
                StartCoroutine(Fade(substract, _light, _light.pointLightOuterRadius, 0));
                _light.intensity = 0;
                break;
        }
        _light.color = new Color(Mathf.Lerp(_minColor.r, _maxColor.r, _value / 10), Mathf.Lerp(_minColor.g, _maxColor.g, _value / 10), Mathf.Lerp(_minColor.b, _maxColor.b, _value / 10));
        

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
        
        
        
        
    }
}
