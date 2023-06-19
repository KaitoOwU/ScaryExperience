using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _title, _nextLevel, _restart, _mainMenu;
    [SerializeField] Image _collectable, _collectableMissed, _flameSilver, _flameGold, _flameMissed, _transition;

    private void OnEnable()
    {
        GameManager.Instance.SetTouchControlsActive(false);
        DOTween.Kill(transform);

        if (DataManager.Instance.IsGameInFrench)
        {
            _title.text = "Vous avez réussi";
            _title.fontSize = 72f;

            if(!DataManager.Instance.LevelData.ContainsKey(DataManager.Instance.CurrentLevel + 1))
            {
                _nextLevel.GetComponentInParent<Button>().interactable = false;
                _nextLevel.text = "Terminé !";
            } else
            {
                _nextLevel.GetComponentInParent<Button>().interactable = true;
                _nextLevel.text = "Niveau Suivant";
            }

            
            _restart.text = "Réessayer";
            _mainMenu.text = "Menu Principal";
        } else
        {
            _title.text = "You Escaped";
            _title.fontSize = 100f;

            if (!DataManager.Instance.LevelData.ContainsKey(DataManager.Instance.CurrentLevel + 1))
            {
                _nextLevel.GetComponentInParent<Button>().interactable = false;
                _nextLevel.text = "Finish !";
            }
            else
            {
                _nextLevel.GetComponentInParent<Button>().interactable = true;
                _nextLevel.text = "Next Level";
            }

            _restart.text = "Try Again";
            _mainMenu.text = "Main Menu";
        }

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
        _transition.transform.DOLocalMoveY(0, 2f).SetEase(Ease.OutExpo).OnComplete(() => SceneManager.LoadScene(DataManager.Instance.LevelList[DataManager.Instance.CurrentLevel]._levelSceneName));
    }

    public void MainMenu()
    {
        _transition.transform.DOLocalMoveY(0, 2f).SetEase(Ease.OutExpo).OnComplete(() => SceneManager.LoadScene("MainMenu"));
    }

    public void Restart()
    {
        DataManager.Instance.IsLevelLaunchedFromMainMenu = false;
        _transition.transform.DOLocalMoveY(0, 2f).SetEase(Ease.OutExpo).OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
    }
}
