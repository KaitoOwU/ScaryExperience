using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{

    [SerializeField] Image _collectable, _collectableMissed, _flameSilver, _flameGold, _flameMissed;

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
                _collectableMissed.DOColor(new(1, 1, 1, 1f), 1f).SetEase(Ease.OutExpo);
            }

            switch (DataManager.Instance.LevelData[DataManager.Instance.CurrentLevel].FlameState)
            {
                case FlameState.None:
                    _flameMissed.DOColor(new(1, 1, 1, 1f), 1f).SetEase(Ease.OutExpo);
                    break;
                case FlameState.Silver:
                    _flameSilver.transform.DOScale(1.5f, 0);
                    _flameSilver.DOColor(new(1, 1, 1), .5f);
                    _flameSilver.transform.DOScale(1, 1f).SetEase(Ease.InOutExpo);
                    break;
                case FlameState.Gold:
                    _flameGold.transform.DOScale(1.5f, 0);
                    _flameGold.DOColor(new(1, 1, 1), .5f);
                    _flameGold.transform.DOScale(1, 1f).SetEase(Ease.InOutExpo);
                    break;
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
