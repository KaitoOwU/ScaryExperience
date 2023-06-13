using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMusicManager : MonoBehaviour
{
    [SerializeField] GameObject _sfxDisabled, _musicDisabled;
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
    }

    public void SwitchMusic()
    {
        /*_musicDisabled.SetActive(_musicOn);*/
        _musicOn = !_musicOn;
        DataManager.Instance.IsMusicMuted = !_musicOn;
    }

    private void Update()
    {
        _sfxDisabled.SetActive(DataManager.Instance.IsSoundMuted);
        _musicDisabled.SetActive(DataManager.Instance.IsMusicMuted);
    }
}
