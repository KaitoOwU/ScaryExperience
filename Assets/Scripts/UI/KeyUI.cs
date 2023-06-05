using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    MoveBubble _move;
    [SerializeField] Image _colDotted, _colActivated, _colWhite;

    private void Awake()
    {
        _move = FindObjectOfType<MoveBubble>();
        _move.OnKeyTaken += CollectableTaken;
    }

    private void CollectableTaken()
    {
        _colWhite.DOColor(new(1, 1, 1, 1), 0);
        _colActivated.DOColor(new(1, 1, 1, 1), 0);

        _colActivated.transform.DOScale(1, 2f).SetEase(Ease.OutExpo);
        _colWhite.DOColor(new(1, 1, 1, 0), 2f).SetEase(Ease.OutExpo);
        _colDotted.DOColor(new(1, 1, 1, 0), 2f).SetEase(Ease.OutExpo);
    }
}
