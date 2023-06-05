using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableUI : MonoBehaviour
{
    [SerializeField] MoveBubble _move;
    [SerializeField] Image _colDotted, _colActivated, _colWhite;

    private void Awake()
    {
        _move.OnCollectableTaken += CollectableTaken;
    }

    private void CollectableTaken()
    {
        _colWhite.DOColor(new(1, 1, 1, 1), 0);
        _colActivated.DOColor(new(1, 1, 1, 1), 0);

        _colActivated.transform.DOScale(1, 1f).SetEase(Ease.OutExpo);
        _colWhite.DOColor(new(1, 1, 1, 0), 1f).SetEase(Ease.OutExpo);
        _colDotted.DOColor(new(1, 1, 1, 0), 1f).SetEase(Ease.OutExpo);
    }
}
