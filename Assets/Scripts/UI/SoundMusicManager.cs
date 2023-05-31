using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMusicManager : MonoBehaviour
{
    [SerializeField] GameObject _sfxDisabled, _musicDisabled;
    bool _soundOn = true, _musicOn = true;

    public void SwitchSound()
    {
        _sfxDisabled.SetActive(_soundOn);
        _soundOn = !_soundOn;
    }

    public void SwitchMusic()
    {
        _musicDisabled.SetActive(_musicOn);
        _musicOn = !_musicOn;
    }

}
