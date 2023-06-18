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
    [SerializeField] TextMeshProUGUI _version, _logged, _language;
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

        if (DataManager.Instance.IsGameInFrench)
        {
            _language.text = "Francais";
        } else
        {
            _language.text = "English";
        }

        transform.DOScale(1, 5f).OnComplete(() =>
        {
            if (DataManager.Instance.IsGameInFrench)
            {
                if (DataManager.Instance.IsConnectedToGooglePlayServices)
                {
                    _logged.DOText("Connecté aux services Google Play", 1f);
                    _logged.color = Color.green;
                }
                else
                {
                    _logged.DOText("Non connecté aux services Google Play", 1f);
                    _logged.color = Color.red;
                }
            }
            else
            {
                if (DataManager.Instance.IsConnectedToGooglePlayServices)
                {
                    _logged.DOText("Connected to Google Play services", 1f);
                    _logged.color = Color.green;
                }
                else
                {
                    _logged.DOText("Not connected to Google Play services", 1f);
                    _logged.color = Color.red;
                }
            }
        });
    }

    public void SwitchToLevelSelect()
    {
        _audioManager.PlaySFX(_audioManager.button);
        transform.DOLocalMoveX(-1200, 1f).SetEase(Ease.InOutExpo);
        _levelSelect.DOLocalMoveX(0, 1f).SetEase(Ease.InOutExpo);
    }

    public void ChangeLanguage()
    {
        DataManager.Instance.IsGameInFrench = !DataManager.Instance.IsGameInFrench;
        if (DataManager.Instance.IsGameInFrench)
        {
            _language.DOText("Francais", 1f);

            if (_logged.text == "")
                return;

            if (DataManager.Instance.IsConnectedToGooglePlayServices)
            {
                _logged.DOText("Connecté aux services Google Play", 1f);
                _logged.color = Color.green;
            } else
            {
                _logged.DOText("Non connecté aux services Google Play", 1f);
                _logged.color = Color.red;
            }
        } else
        {
            _language.DOText("English", 1f);

            if (_logged.text == "")
                return;

            if (DataManager.Instance.IsConnectedToGooglePlayServices)
            {
                _logged.DOText("Connected to Google Play services", 1f);
                _logged.color = Color.green;
            }
            else
            {
                _logged.DOText("Not connected to Google Play services", 1f);
                _logged.color = Color.red;
            }
        }
    }

    public void SwitchToMainMenu()
    {
        _audioManager.PlaySFX(_audioManager.button);
        transform.DOLocalMoveX(0, 1f).SetEase(Ease.InOutExpo);
        _levelSelect.DOLocalMoveX(1200, 1f).SetEase(Ease.InOutExpo);
    }
}
