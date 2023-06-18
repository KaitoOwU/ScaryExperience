using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _title, _restart, _mainMenu;

    private void OnEnable()
    {
        DOTween.Kill(transform);
        GameManager.Instance.SetTouchControlsActive(false);
        transform.DOScale(1, 1.5f).SetEase(Ease.OutExpo);

        if (DataManager.Instance.IsGameInFrench)
        {
            _title.text = "Llum est Mort";
            _restart.text = "Réessayer";
            _mainMenu.text = "Menu Principal";
        } else
        {
            _title.text = "Llum is Dead";
            _restart.text = "Try Again";
            _mainMenu.text = "Main Menu";
        }

        GameManager.Instance.LocalDeathAmount++;
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
    }

    public void RestartLevel()
    {
        DataManager.Instance.IsLevelLaunchedFromMainMenu = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
