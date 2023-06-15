using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] ParticleSystem _spawnParticles;
    [SerializeField] MoveBubble _move;

    private void Start()
    {
        _move.SetTouchControlsActive(false);
        transform.DOScale(0, DataManager.Instance.IsLevelLaunchedFromMainMenu ? 4f : 0f).OnComplete(() =>
        {
            Destroy(Instantiate(_spawnParticles, transform.position, Quaternion.identity), 2f);
            transform.DOScale(0, DataManager.Instance.IsLevelLaunchedFromMainMenu ? 1.5f : 1.2f).OnComplete(() =>
            {
                transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
                _move.SetTouchControlsActive(true);
            });
        });
        
    }
}
