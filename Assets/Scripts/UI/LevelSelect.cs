using DG.Tweening;
using NaughtyAttributes;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] DataManager _dataManager;
    [SerializeField] Transform _parent, _dotParent, _moveTransform;
    [SerializeField] RectTransform _firstLevelPosition;
    [SerializeField] Image _transition;
    [SerializeField] GameObject _levelPrefab, _dot;

    private const float LEVEL_HEIGHT_DIFFERENCE = 400f;
    private float RANDOM_X_AXIS { get => Random.Range(-80f, 80f); }

    private Vector2 fingerPos;
    private float minY, maxY;


    private void Awake()
    {
        maxY = _moveTransform.localPosition.y;
        minY = maxY - (_dataManager.LevelList.Count - 5) * LEVEL_HEIGHT_DIFFERENCE;
    }

    [Button]
    public void GenerateLevels()
    { 
        for (int i = 0; i < _dataManager.LevelList.Count; i++)
        {
            Vector3 pos = _firstLevelPosition.position;

            GameObject obj = Instantiate(_levelPrefab, pos, Quaternion.identity, _parent);
            obj.GetComponent<LevelUI>().SetupUI(i);

            if (i > 0)
            {
                RectTransform rect = obj.GetComponent<RectTransform>();
                rect.localPosition += new Vector3(RANDOM_X_AXIS, LEVEL_HEIGHT_DIFFERENCE * i);
            }
        }

        for(int i = 0; i < _dataManager.LevelList.Count - 1; i++)
        {
            Vector3 _level = _parent.transform.GetChild(i).GetComponent<RectTransform>().localPosition;
            Vector3 _diff = _parent.transform.GetChild(i + 1).GetComponent<RectTransform>().localPosition - _level;

            Vector3 _firstDot = (_diff / 5) * 2, _secondDot = (_diff / 5) * 3;
            Instantiate(_dot, _dotParent).GetComponent<RectTransform>().localPosition = _level + _firstDot;
            Instantiate(_dot, _dotParent).GetComponent<RectTransform>().localPosition = _level + _secondDot;
        }
    }

    public void LaunchLevel(int levelNumber)
    {
        DataManager.Instance.IsLevelLaunchedFromMainMenu = true;
        DataManager.Instance.CurrentLevel = levelNumber;

        _transition.gameObject.SetActive(true);
        _transition.DOColor(new(0, 0, 0, 1), 1f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            SceneManager.LoadScene(DataManager.Instance.LevelList[levelNumber]._levelSceneName);
        });

        ETouch.Touch.onFingerDown -= SetupPos;
        ETouch.Touch.onFingerMove -= Scrolling;
    }

    public void SetFingerControlActive(bool state)
    {
        if (state)
        {
            ETouch.Touch.onFingerDown += SetupPos;
            ETouch.Touch.onFingerMove += Scrolling;
        } else
        {
            ETouch.Touch.onFingerDown -= SetupPos;
            ETouch.Touch.onFingerMove -= Scrolling;
        }
    }

    private void SetupPos(Finger obj)
    {
        fingerPos = Camera.main.ScreenToWorldPoint(obj.screenPosition);
        Debug.LogWarning(fingerPos);
    }

    private void Scrolling(Finger obj)
    {
        Vector2 currentPos = Camera.main.ScreenToWorldPoint(obj.screenPosition);
        Vector2 delta = currentPos - fingerPos;
        if((_moveTransform.localPosition + new Vector3(0, delta.y * 250f, 0)).y <= maxY &&
            (_moveTransform.localPosition + new Vector3(0, delta.y * 250f, 0)).y >= minY)
            _moveTransform.localPosition += new Vector3(0, delta.y * 250f, 0);

        fingerPos = currentPos;
    }
}
