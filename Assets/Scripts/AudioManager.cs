using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [Header("- Audio Source -")]
    [SerializeField] AudioSource music;
    [SerializeField] AudioSource SFX;
    public AudioSource glbGlb;

    [Header("- Audio Clip (Music) -")]
    public AudioClip menuMusic;
    public AudioClip levelMusic;

    [Header("- Audio Clip (SFX) -")]
    public List<AudioClip> movementSounds;
    public AudioClip waterSound;
    public AudioClip cranePickUpSound;
    public AudioClip deathByMonsterSound;
    public AudioClip glbglbSound;
    public AudioClip keyPickUp;
    public AudioClip fallSound;
    public List<AudioClip> rockSound;

    [Header("- Audio Clip (UI) -")]
    public AudioClip pause;
    public AudioClip button;
    public AudioClip levelWin;

    public float volumeGlbGlb;
    public float volumeMusic;
    public float volumeSFX;


    private void Awake()
    {
        music.loop = true;
        glbGlb.loop = true;
        PlayMusic();
        PlayGlbGlbSound();
        volumeSFX = SFX.volume;
        volumeMusic = music.volume;
    }
    public void PlaySFX(AudioClip clip)
    {
        SFX.PlayOneShot(clip);
    }

    public void PlayMusic()
    {
        music.Play();
    }

    public void PlayGlbGlbSound()
    {
        glbGlb.Play();
    }

    private void Update()
    {
        if (DataManager.Instance != null && DataManager.Instance.IsMusicMuted)
        {
            music.volume = 0;
        }
        else
        {
            music.volume = volumeMusic;
        }

        if (DataManager.Instance != null && DataManager.Instance.IsSoundMuted)
        {
            SFX.volume = 0;
            glbGlb.volume = 0;
        }
        else
        {
            SFX.volume = volumeSFX;
            glbGlb.volume = volumeGlbGlb;
        }
    }

}
