using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Instance.SetTouchControlsActive(false);
        DOTween.Kill(transform);
        transform.DOScale(1, 1.5f).SetEase(Ease.OutExpo);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
