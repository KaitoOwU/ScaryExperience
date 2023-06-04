using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _levelNumber, _levelName;
    [SerializeField] Transform _leftLine, _rightLine;
    [SerializeField] Image _bg;

    private void Start()
    {
        if (!DataManager.Instance.IsLevelLaunchedFromMainMenu)
        {
            Destroy(gameObject);
        } else
        {
            StartCoroutine(StartAnimation());

        }
    }

    private IEnumerator StartAnimation()
    {
        Level lvl = DataManager.Instance.LevelList[GameManager.Instance.CurrentLevel];
        _levelNumber.text = "Level " + (GameManager.Instance.CurrentLevel + 1);
        _levelName.text = lvl.levelName;

        //START ANIMATION
        _levelNumber.DOColor(Color.white, 1f).SetEase(Ease.InOutExpo);
        yield return new WaitForSecondsRealtime(0.3f);
        _leftLine.DOScaleX(22500, 1f).SetEase(Ease.OutExpo);
        _rightLine.DOScaleX(22500, 1f).SetEase(Ease.OutExpo);
        yield return new WaitForSecondsRealtime(0.7f);
        _levelName.DOColor(Color.white, 5f).SetEase(Ease.InOutExpo);
        yield return new WaitForSecondsRealtime(2.5f);
        _bg.DOColor(new(0, 0, 0, 0), 4f).SetEase(Ease.InOutExpo);

        yield return new WaitForSecondsRealtime(3f);
        _levelName.DOColor(new(1, 1, 1, 0), 8f).SetEase(Ease.InOutExpo);
        _levelNumber.DOColor(new(1, 1, 1, 0), 8f).SetEase(Ease.InOutExpo);
        _rightLine.GetComponent<Image>().DOColor(new(1, 1, 1, 0), 8f).SetEase(Ease.InOutExpo);
        _leftLine.GetComponent<Image>().DOColor(new(1, 1, 1, 0), 8f).SetEase(Ease.InOutExpo);

        yield return new WaitForSecondsRealtime(3f);
        _bg.raycastTarget = false;
        Destroy(gameObject, 5f);
    }
}
