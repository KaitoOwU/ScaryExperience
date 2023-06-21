using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _title;
    [SerializeField] Image _transition;

    private void OnEnable()
    {
        DOTween.Kill(transform);
        GameManager.Instance.SetTouchControlsActive(false);
        transform.DOScale(1, 1.5f).SetEase(Ease.OutExpo);

        if (DataManager.Instance.IsGameInFrench)
        {
            _title.text = "Llum est Mort";
        } else
        {
            _title.text = "Llum is Dead";
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

        _transition.transform.DOLocalMoveY(0, 2f).SetEase(Ease.OutExpo).OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        
    }

    public void GoToMainMenu()
    {
        _transition.transform.DOLocalMoveY(0, 2f).SetEase(Ease.OutExpo).OnComplete(() => SceneManager.LoadScene("MainMenu"));
    }

}
