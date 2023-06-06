using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{

    [SerializeField, ReadOnly] int levelNumber;
    [SerializeField] TextMeshProUGUI _levelName;
    [SerializeField] Button _play;
    LevelSelect levelSelect;

    public void SetupUI(int levelNumber)
    {
        this.levelNumber = levelNumber;
        _levelName.text = "" + (levelNumber + 1);
    }

    private void Start()
    {
        levelSelect = FindObjectOfType<LevelSelect>();
        _play.interactable = DataManager.Instance.LevelData[levelNumber].IsUnlocked;
    }

    public void LaunchLevel()
    {
        levelSelect.LaunchLevel();
    }
}
