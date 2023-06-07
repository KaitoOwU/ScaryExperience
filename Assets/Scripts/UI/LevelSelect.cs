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
    [SerializeField] Transform _parent;
    [SerializeField] RectTransform _firstLevelPosition;
    [SerializeField] Image _transition;
    [SerializeField] GameObject _levelPrefab;

    private const float LEVEL_HEIGHT_DIFFERENCE = 400f;
    private float RANDOM_X_AXIS { get => Random.Range(-200f, 300f); }

    int currentDisplayedLevel = 0;

    private Vector2 fingerPos;
    private float minY, maxY;


    private void Awake()
    {
        maxY = _firstLevelPosition.localPosition.y;
        minY = maxY - (_dataManager.LevelList.Count - 5) * 400;
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
    }

    public void LaunchLevel()
    {
        DataManager.Instance.IsLevelLaunchedFromMainMenu = true;

        _transition.gameObject.SetActive(true);
        _transition.DOColor(new(0, 0, 0, 1), 1f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            SceneManager.LoadScene(DataManager.Instance.LevelList[currentDisplayedLevel]._levelSceneName);
        });
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
        if((_firstLevelPosition.localPosition + new Vector3(0, delta.y * 150f, 0)).y <= maxY &&
            (_firstLevelPosition.localPosition + new Vector3(0, delta.y * 150f, 0)).y >= minY)
            _firstLevelPosition.localPosition += new Vector3(0, delta.y * 150f, 0);

        fingerPos = currentPos;
    }
}
