using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Transform _levelSelect;

    public void SwitchToLevelSelect()
    {
        transform.DOLocalMoveX(-1200, 1f).SetEase(Ease.InOutExpo);
        _levelSelect.DOLocalMoveX(0, 1f).SetEase(Ease.InOutExpo);
    }

    public void SwitchToMainMenu()
    {
        transform.DOLocalMoveX(0, 1f).SetEase(Ease.InOutExpo);
        _levelSelect.DOLocalMoveX(1200, 1f).SetEase(Ease.InOutExpo);
    }
}
