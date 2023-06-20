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
    [SerializeField] Sprite _notActivated, _unlocked, _finished, _golded;
    [SerializeField] TextMeshProUGUI _levelName;
    [SerializeField] Button _play;
    [SerializeField] GameObject _collectible, _goldenFlame, _silverFlame;
    [SerializeField] Transform _dotParent;
    LevelSelect levelSelect;

    public Transform DotParent { get => _dotParent; }

    public void SetupUI(int levelNumber)
    {
        this.levelNumber = levelNumber;
    }

    private void Start()
    {
        levelSelect = FindObjectOfType<LevelSelect>();
        Image img = GetComponent<Image>();

        if (!DataManager.Instance.LevelData[levelNumber].IsUnlocked)
        {
            img.sprite = _notActivated;
            transform.localScale = new(.6f, .6f, .6f);
            _levelName.text = "";
            GetComponent<Button>().interactable = false;

        } else
        {
            img.sprite = _unlocked;
            transform.localScale = new(1, 1, 1);
            _levelName.text = "" + (levelNumber + 1);
            GetComponent<Button>().interactable = true;

            _collectible.SetActive(DataManager.Instance.LevelData[levelNumber].CollectibleAcquired);
            switch (DataManager.Instance.LevelData[levelNumber].FlameState)
            {
                case FlameState.Silver:
                    _silverFlame.SetActive(true);
                    break;
                case FlameState.Gold:
                    _goldenFlame.SetActive(true);
                    break;
            }

            if (DataManager.Instance.LevelData[levelNumber].IsCompleted)
            {
                img.sprite = _finished;
            }

            if(DataManager.Instance.LevelData[levelNumber].CollectibleAcquired && DataManager.Instance.LevelData[levelNumber].FlameState == FlameState.Gold)
            {
                img.sprite = _golded;
            }

        }
    }

    public void LaunchLevel()
    {
        levelSelect.LaunchLevel(levelNumber);
    }
}
