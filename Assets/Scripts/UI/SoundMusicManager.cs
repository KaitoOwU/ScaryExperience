using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundMusicManager : MonoBehaviour
{
    [SerializeField] Sprite _sfxDisabled, _musicDisabled, _sfxEnabled, _musicEnabled;
    [SerializeField] Image _sfx, _music;
    bool _soundOn = true, _musicOn = true;

    private void Awake()
    {
        _soundOn = !DataManager.Instance.IsSoundMuted;
        _musicOn = !DataManager.Instance.IsMusicMuted;
    }
    public void SwitchSound()
    {
        /*_sfxDisabled.SetActive(_soundOn);*/
        _soundOn = !_soundOn;
        DataManager.Instance.IsSoundMuted = !_soundOn;

        if (_soundOn)
        {
            _sfx.sprite = _sfxEnabled;
        }
        else
        {
            _sfx.sprite = _sfxDisabled;
        }

    }

    public void SwitchMusic()
    {
        /*_musicDisabled.SetActive(_musicOn);*/
        _musicOn = !_musicOn;
        DataManager.Instance.IsMusicMuted = !_musicOn;

        if (_musicOn)
        {
            _music.sprite = _musicEnabled;
        } else
        {
            _music.sprite = _musicDisabled;
        }

    }
}
