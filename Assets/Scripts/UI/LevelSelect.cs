using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] Transform _parent, _nextLevelSpawnPos, _prevLevelSpawnPos;
    [SerializeField] Image _transition;
    [SerializeField] GameObject _levelPrefab;
    [SerializeField] GameObject _currentLevelUI;

    int currentDisplayedLevel = 0;

    public void DisplayNextLevel()
    {
        if (FindObjectsOfType<LevelUI>().Length > 1)
            return;

        LevelUI _lvl = Instantiate(_levelPrefab, _nextLevelSpawnPos.position, Quaternion.identity, _parent).GetComponent<LevelUI>();

        _lvl.SetupUI((currentDisplayedLevel + 1) % DataManager.Instance.LevelList.Count);

        _currentLevelUI.transform.DOLocalMoveX(-1000, 1f).SetEase(Ease.InOutExpo);
        _lvl.transform.DOLocalMoveX(0, 1f).SetEase(Ease.InOutExpo);

        Destroy(_currentLevelUI, 0.75f);
        _currentLevelUI = _lvl.gameObject;
        currentDisplayedLevel = (currentDisplayedLevel + 1) % DataManager.Instance.LevelList.Count;
    }

    public void DisplayPrevLevel()
    {
        if (FindObjectsOfType<LevelUI>().Length > 1)
            return;

        LevelUI _lvl = Instantiate(_levelPrefab, _prevLevelSpawnPos.position, Quaternion.identity, _parent).GetComponent<LevelUI>();
        _lvl.SetupUI(currentDisplayedLevel - 1 < 0 ? DataManager.Instance.LevelList.Count - 1 : currentDisplayedLevel - 1);

        _currentLevelUI.transform.DOLocalMoveX(1000, 1f).SetEase(Ease.InOutExpo);
        _lvl.transform.DOLocalMoveX(0, 1f).SetEase(Ease.InOutExpo);

        Destroy(_currentLevelUI, 0.75f);
        _currentLevelUI = _lvl.gameObject;
        currentDisplayedLevel = currentDisplayedLevel - 1 < 0 ? DataManager.Instance.LevelList.Count - 1 : currentDisplayedLevel - 1;
    }

    public void LaunchLevel()
    {
        DataManager.Instance.IsLevelLaunchedFromMainMenu = true;

        _transition.gameObject.SetActive(true);
        _transition.DOColor(new(0, 0, 0, 1), 1f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            SceneManager.LoadScene(DataManager.Instance.LevelList[currentDisplayedLevel].levelSceneName);
        });
    }
}
