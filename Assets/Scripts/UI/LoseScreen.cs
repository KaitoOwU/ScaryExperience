using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{
    private void OnEnable()
    {
        DOTween.Kill(transform);
        GameManager.Instance.SetTouchControlsActive(false);
        transform.DOScale(1, 1.5f).SetEase(Ease.OutExpo);

        GameManager.Instance.LocalDeathAmount++;
        if(GameManager.Instance.LocalDeathAmount >= 5f)
        {
            DataManager.Instance.AchievementToNextStep(GPGSIds.achievement_hardstuck, 100f);
        }

        DataManager.Instance.AchievementToNextStep(GPGSIds.achievement_ouch, (5f / DataManager.Instance.DeathAmount) * 100f);
        DataManager.Instance.AchievementToNextStep(GPGSIds.achievement_die_25_times, (25f / DataManager.Instance.DeathAmount) * 100f);
        DataManager.Instance.AchievementToNextStep(GPGSIds.achievement_die_50_times, (50f / DataManager.Instance.DeathAmount) * 100f);
        DataManager.Instance.AchievementToNextStep(GPGSIds.achievement_die_100_times, DataManager.Instance.DeathAmount);
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
    }

    public void RestartLevel()
    {
        DataManager.Instance.IsLevelLaunchedFromMainMenu = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
