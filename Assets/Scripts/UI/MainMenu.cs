using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Transform _levelSelect;
    AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void SwitchToLevelSelect()
    {
        _audioManager.PlaySFX(_audioManager.button);
        transform.DOLocalMoveX(-1200, 1f).SetEase(Ease.InOutExpo);
        _levelSelect.DOLocalMoveX(0, 1f).SetEase(Ease.InOutExpo);
    }

    public void SwitchToMainMenu()
    {
        _audioManager.PlaySFX(_audioManager.button);
        transform.DOLocalMoveX(0, 1f).SetEase(Ease.InOutExpo);
        _levelSelect.DOLocalMoveX(1200, 1f).SetEase(Ease.InOutExpo);
    }
}
