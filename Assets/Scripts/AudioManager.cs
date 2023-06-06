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



    private void Awake()
    {
        music.loop = true;
        glbGlb.loop = true;
        PlayMusic();
        PlayGlbGlbSound();

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

}
