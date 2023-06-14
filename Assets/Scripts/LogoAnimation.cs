using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoAnimation : MonoBehaviour
{

    [SerializeField] Image _logo; 

    void Start()
    {
        _logo.DOColor(new(1, 1, 1, 1), 1f).OnComplete(() =>
        {
            _logo.DOColor(new(1, 1, 1, 1), 3f).OnComplete(() =>
            {
                _logo.DOColor(new(1, 1, 1, 0), 1f).OnComplete(() =>
                {
                    SceneManager.LoadScene("MainMenu");
                });
            });
        });
    }
}
