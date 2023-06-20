using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartTransition : MonoBehaviour
{
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            transform.DOLocalMoveY(0, 0).OnComplete(() => transform.DOLocalMoveY(-3000, 2f).SetEase(Ease.OutExpo));
        } else
        {
            if (!DataManager.Instance.IsLevelLaunchedFromMainMenu)
            {
                transform.DOLocalMoveY(0, 0).OnComplete(() => transform.DOLocalMoveY(-3000, 2f).SetEase(Ease.OutExpo));
            }
            else
            {
                transform.DOLocalMoveY(-3000, 0);
            }
        }
    }
}
