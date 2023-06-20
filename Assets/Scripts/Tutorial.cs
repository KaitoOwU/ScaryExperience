using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tutorial;
    [SerializeField] RectTransform _listTuto;
    [SerializeField] TextMeshProUGUI _moveTitle, _moveDesc, _goalTitle, _goalDesc, _torchTitle, _torchDesc, _braseroTitle, _braseroDesc, _fragileGroundTitle, _fragileGroundDesc, _windTitle, _windDesc, _blockTitle, _blockDesc;
    [SerializeField] string _moveTitleFr, _moveTitleEn, _goalTitleFr, _goalTitleEn, _torchTitleFr, _torchTitleEn, _braseroTitleFr, _braseroTitleEn, _fragileGroundTitleFr, _fragileGroundTitleEn, _windTitleFr, _windTitleEn, _blockTitleFr, _blockTitleEn;
    [SerializeField, TextArea] string _moveDescFr, _moveDescEn, _goalDescFr, _goalDescEn, _torchDescFr, _torchDescEn, _braseroDescFr, _braseroDescEn, _fragileGroundDescFr, _fragileGroundDescEn, _windDescFr, _windDescEn, _blockDescFr, _blockDescEn;

    int _currentActive;

    public void Activate()
    {
        _currentActive = 0;
        _listTuto.DOLocalMoveX(3575f, 0f);
        transform.DOLocalMoveY(0, 1f).SetEase(Ease.OutExpo);

        if (DataManager.Instance.IsGameInFrench)
        {
            _tutorial.text = "Tutoriel";

            _moveTitle.text = _moveTitleFr;
            _moveDesc.text = _moveDescFr;

            _goalTitle.text = _goalTitleFr;
            _goalDesc.text = _goalDescFr;

            _torchTitle.text = _torchTitleFr;
            _torchDesc.text = _torchDescFr;

            _braseroTitle.text = _braseroTitleFr;
            _braseroDesc.text = _braseroDescFr;

            _fragileGroundTitle.text = _fragileGroundTitleFr;
            _fragileGroundDesc.text = _fragileGroundDescFr;

            _windTitle.text = _windTitleFr;
            _windDesc.text = _windDescFr;

            _blockTitle.text = _blockTitleFr;
            _blockDesc.text = _blockDescFr;
        } else
        {
            _tutorial.text = "Tutorial";

            _moveTitle.text = _moveTitleEn;
            _moveDesc.text = _moveDescEn;

            _goalTitle.text = _goalTitleEn;
            _goalDesc.text = _goalDescEn;

            _torchTitle.text = _torchTitleEn;
            _torchDesc.text = _torchDescEn;

            _braseroTitle.text = _braseroTitleEn;
            _braseroDesc.text = _braseroDescEn;

            _fragileGroundTitle.text = _fragileGroundTitleEn;
            _fragileGroundDesc.text = _fragileGroundDescEn;

            _windTitle.text = _windTitleEn;
            _windDesc.text = _windDescEn;

            _blockTitle.text = _blockTitleEn;
            _blockDesc.text = _blockDescEn;
        }
    }

    public void Next()
    {
        _currentActive++;
        if(_currentActive <= 6)
        {
            _listTuto.DOLocalMoveX(3575f - 1500 * _currentActive, 1f).SetEase(Ease.OutExpo);
        } else
        {
            Deactivate();
        }
    }

    public void Deactivate()
    {
        transform.DOLocalMoveY(-2700, 1f).SetEase(Ease.OutExpo);
    }
}
