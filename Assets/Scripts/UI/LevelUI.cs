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
    [SerializeField] Color _golded;
    [SerializeField] TextMeshProUGUI _levelName;
    [SerializeField] Button _play;
    [SerializeField] GameObject _collectible;
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
            img.color = new(.2f, .2f, .2f);
            transform.localScale = new(.6f, .6f, .6f);
            _levelName.text = "";
            GetComponent<Button>().interactable = false;

        } else
        {
            img.color = new(1, 1, 1);
            transform.localScale = new(1, 1, 1);
            _levelName.text = "" + (levelNumber + 1);
            GetComponent<Button>().interactable = true;

            _collectible.SetActive(DataManager.Instance.LevelData[levelNumber].CollectibleAcquired);
        }
    }

    public void LaunchLevel()
    {
        levelSelect.LaunchLevel(levelNumber);
    }
}
