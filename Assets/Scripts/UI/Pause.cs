using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
    AudioManager _audioManager;
    [SerializeField] TextMeshProUGUI _backToGame, _mainMenu;

    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnEnable()
    {
        GameManager.Instance.SetTouchControlsActive(false);
        DOTween.Kill(transform);
        transform.DOScale(1, 1.5f).SetEase(Ease.OutExpo);

        if (DataManager.Instance.IsGameInFrench)
        {
            _backToGame.text = "Revenir au jeu";
            _mainMenu.text = "Menu Principal";
        } else
        {
            _backToGame.text = "Back to Game";
            _mainMenu.text = "Main Menu";
        }
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
