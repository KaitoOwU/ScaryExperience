using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{
    private void OnEnable()
    {
        DOTween.Kill(transform);
        transform.DOScale(1, 1.5f).SetEase(Ease.OutExpo);
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
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
