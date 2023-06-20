using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    AudioManager _audioManager;
    [SerializeField] Image _transition;

    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnEnable()
    {
        GameManager.Instance.SetTouchControlsActive(false);
        DOTween.Kill(transform);
        transform.DOScale(1, 1.5f).SetEase(Ease.OutExpo);
    }

    public void PlayPauseSound()
    {
        _audioManager.PlaySFX(_audioManager.pause);
    }

    private void OnDisable()
    {
        GameManager.Instance.SetTouchControlsActive(true);
        DOTween.Kill(transform);
        transform.DOScale(0, 1.5f).SetEase(Ease.OutExpo);
    }

    public void QuitPause()
    {
        gameObject.SetActive(false);
    }

    public void RestartLevel()
    {
        _audioManager.PlaySFX(_audioManager.pause);
        if(DataManager.Instance != null)
            DataManager.Instance.IsLevelLaunchedFromMainMenu = false;
        _transition.transform.DOLocalMoveY(0, 2f).SetEase(Ease.OutExpo).OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void GoToMainMenu()
    {
        _transition.transform.DOLocalMoveY(0, 2f).SetEase(Ease.OutExpo).OnComplete(() => SceneManager.LoadScene("MainMenu"));
    }

}
