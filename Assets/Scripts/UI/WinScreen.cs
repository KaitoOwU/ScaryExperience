using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{

    [SerializeField] Image _collectable;

    private void OnEnable()
    {
        GameManager.Instance.SetTouchControlsActive(false);
        DOTween.Kill(transform);
        transform.DOScale(1, 1f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            if (DataManager.Instance.LevelData[DataManager.Instance.CurrentLevel].CollectibleAcquired)
            {
                _collectable.transform.DOScale(1.5f, 0);
                _collectable.DOColor(new(1, 1, 1), .5f);
                _collectable.transform.DOScale(1, 1f).SetEase(Ease.InOutExpo);
            } else
            {
                _collectable.DOColor(new(1, 1, 1, 0.2f), 1f).SetEase(Ease.OutExpo);
            }
        });
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
    }

    public void NextLevel()
    {
        DataManager.Instance.IsLevelLaunchedFromMainMenu = true;
        DataManager.Instance.CurrentLevel++;
        SceneManager.LoadScene(DataManager.Instance.LevelList[DataManager.Instance.CurrentLevel]._levelSceneName);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        DataManager.Instance.IsLevelLaunchedFromMainMenu = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
