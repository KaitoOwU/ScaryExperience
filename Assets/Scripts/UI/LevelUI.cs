using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{

    [SerializeField, ReadOnly] int levelNumber;
    [SerializeField] TextMeshProUGUI _levelName, _unlockedText;
    [SerializeField] Image _collectible;
    [SerializeField] Button _play;

    public void SetupUI(int levelNumber)
    {

        this.levelNumber = levelNumber;
        _levelName.text = "Level " + (levelNumber + 1);

        if (DataManager.Instance.LevelData[levelNumber].IsUnlocked)
        {
            _play.interactable = true;
            _unlockedText.text = "Play";
        } else
        {
            _play.interactable = false;
            _unlockedText.text = "Locked";
        }
        
    }

    public void LaunchLevel()
    {
        SceneManager.LoadScene(DataManager.Instance.LevelList[0].name);
    }
}
