using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    [SerializeField] Transform _texts;
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
        LevelLoadData lvl = DataManager.Instance.LevelList[DataManager.Instance.CurrentLevel];
        _levelNumber.text = "Level " + (DataManager.Instance.CurrentLevel + 1);
        _levelName.text = lvl._levelName;

        //START ANIMATION
        yield return _levelNumber.DOColor(new(1, 1, 1), .5f).SetEase(Ease.OutExpo).WaitForCompletion();
        _rightLine.DOScaleX(25000, 1.5f).SetEase(Ease.OutExpo);
        yield return _leftLine.DOScaleX(25000, 1.5f).SetEase(Ease.OutExpo).WaitForCompletion();
        _levelName.DOScale(1, 10f);
        yield return _levelName.DOColor(new(1, 1, 1), 1f).SetEase(Ease.OutExpo).WaitForCompletion();
        yield return new WaitForSecondsRealtime(2f);
        _bg.DOColor(new(0, 0, 0, 0), 2f);
        yield return new WaitForSecondsRealtime(3.5f);
        _rightLine.GetComponent<Image>().DOColor(new(1, 1, 1, 0), 1f);
        _leftLine.GetComponent<Image>().DOColor(new(1, 1, 1, 0), 1f);
        _levelName.DOColor(new(1, 1, 1, 0), 1f);
        _levelNumber.DOColor(new(1, 1, 1, 0), 1f);

    }
}
