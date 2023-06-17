using DG.Tweening;
using GooglePlayGames.OurUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Transform _levelSelect;
    [SerializeField] TextMeshProUGUI _version, _logged;
    AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        _version.text = "";
        _version.DOText("V. " + Application.version, 1f);
        _logged.text = "";

        transform.DOScale(1, 5f).OnComplete(() =>
        {
            if (DataManager.Instance.IsConnectedToGooglePlayServices)
            {
                _logged.color = Color.green;
                _logged.DOText("Connected to Google Play services", 1f);
            }
            else
            {
                _logged.color = Color.red;
                _logged.DOText("Not connected to Google Play services", 1f);
            }
        });
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
