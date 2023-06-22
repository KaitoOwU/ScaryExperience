using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] FlameManager _flameManager;
    [SerializeField] TextMeshProUGUI _amountText, _shadowText;
    [SerializeField] Transform _fire;
    [SerializeField] Image _fireRenderer;

    [Header("- Shader Stats")]
    [SerializeField] float _minShaderValue;
    [SerializeField] float _maxShaderValue;
    [SerializeField] float _shaderTimeMoving;
    float _valueMat;

    [SerializeField] Vector3 _fullPos, _emptyPos;
    float _cooldown;

    private Action OnAlert;

    private void Awake()
    {
        _flameManager.OnFlameValueChange += UpdateUI;
        _fireRenderer.material.SetFloat("_value", 0.13f);
        _valueMat = 0.13f;

        //version kevin
        //_fire.transform.localPosition = _fullPos;
    }

    [Button]
    private void SetFullPos()
    {
        _fullPos = _fire.transform.localPosition;
    }

    [Button]
    private void SetEmptyPos()
    {
        _emptyPos = _fire.transform.localPosition;
    }

    private void Update()
    {
        _cooldown += Time.deltaTime;
        if(_cooldown > 2f)
        {
            _cooldown = 0f;
            if(int.Parse(_amountText.text) <= 3)
            {
                OnAlert?.Invoke();
            }
        }
    }

    private void UpdateUI(float value)
    {
        DOTween.Kill(_shadowText);
        _shadowText.DOScale(1f, 0f);
        _shadowText.DOColor(new(1, 1, 1, 1), 0f);

        _amountText.text = "" + value;
        _shadowText.text = "" + value;

        _shadowText.DOScale(2f, 1.5f);
        _shadowText.DOColor(new(1, 1, 1, 0), 1.5f);

        if(value <= 3)
        {
            OnAlert += Alert;
        } else
        {
            OnAlert -= Alert;
        }

        //version kevin
        //_fire.DOLocalMove(Vector3.Lerp(_emptyPos, _fullPos, value / 10f), 1.5f).SetEase(Ease.OutExpo);

        float actualValue = _fireRenderer.material.GetFloat("_value");

        _valueMat = _maxShaderValue - _minShaderValue;
        _valueMat /= 10;
        _valueMat = (_maxShaderValue - (_valueMat * value));

        DOTween.To(() => actualValue, x => actualValue = x, _valueMat, _shaderTimeMoving)
            .OnUpdate(() => {
                _fireRenderer.material.SetFloat("_value", actualValue);
            });
    }

    private void Alert()
    {
        _amountText.DOScale(1.5f, 0);
        _amountText.DOScale(1f, 1.5f).SetEase(Ease.OutExpo);
        _amountText.DOColor(new(1, 0.3f, 0.3f, 1), 0);
        _amountText.DOColor(new(1, 1, 1, 1), 1.5f);
    }
}
