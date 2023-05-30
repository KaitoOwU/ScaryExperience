using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUI : MonoBehaviour
{
    int levelNumber;

    public void SetupUI(int levelNumber, Transform toModify)
    {
        this.levelNumber = levelNumber;
        GetComponentInChildren<TextMeshProUGUI>().text = "" + levelNumber;
        if(levelNumber > 3)
        {
            toModify.localPosition += new Vector3(0f, 350f, 0f);
        } else
        {
            toModify.localPosition = Vector3.zero;
        }
    }

    public void LaunchLevel()
    {
        SceneManager.LoadScene("Level" + levelNumber);
    }
}
