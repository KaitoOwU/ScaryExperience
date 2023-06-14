using System.Collections;
using UnityEngine;
using Etouch = UnityEngine.InputSystem.EnhancedTouch;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] public TileMap tileMap;
    [SerializeField] public TileUpMap tileUpMap;

    [SerializeField] GameObject _loseScreen, _winScreen, _pauseScreen;
    [SerializeField] FlameManager _flameManager;
    [SerializeField] MoveBubble _moveBubble;
    [SerializeField] Animator _playerAnim;
    [SerializeField] AudioManager _audioManager;

    [Header("-- Level Data --")]
    [SerializeField] bool _levelWithoutKey;
    [SerializeField] GameObject _grid;
    [SerializeField] int _stepAccountNeeded;


    public GameObject LoseScreen { get => _loseScreen; private set => _loseScreen = value; }
    public GameObject WinScreen { get => _winScreen; private set => _winScreen = value; }
    public GameObject PauseScreen { get => _pauseScreen; private set => _pauseScreen = value; }
    public bool HaveKey { get => !_levelWithoutKey; }
    public GameObject Grid { get => _grid; }
    public AudioManager AudioManager { get => _audioManager; }
    public int LocalDeathAmount { get; set; } = 0;

    public int StepAccount { get => _stepAccountNeeded; }

    private void Awake()
    {
        Instance = this;
        if (_levelWithoutKey)
        {
            _grid.transform.parent.gameObject.GetComponent<TileUp>().isActivated = true;
            _grid.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _flameManager.OnFlameValueChange += CheckForLoseCondition;
        PauseScreen.SetActive(true);
        PauseScreen.SetActive(false);
    }

    private void OnDisable()
    {
        _flameManager.OnFlameValueChange -= CheckForLoseCondition;
    }

    private void CheckForLoseCondition(float flameValue)
    {
        StartCoroutine(LostCondition(flameValue));
    }

    private IEnumerator LostCondition(float flameValue)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        if (flameValue <= 0 && _loseScreen != null && !_winScreen.activeInHierarchy)
        {
            _loseScreen.SetActive(true);
            DataManager.Instance.AchievementToNextStep(GPGSIds.achievement_welcome_among_us, 100f);
        }
    }

    private void Start()
    {
        Etouch.EnhancedTouchSupport.Enable();
    }

    internal void SetTouchControlsActive(bool active)
    {
        _moveBubble.SetTouchControlsActive(active);
    }

    internal void AnimatePlayer(TileDown.Direction down)
    {
        switch (down)
        {
            case TileDown.Direction.Left:
                _playerAnim.Play("PlayerDashSide");
                _playerAnim.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                break;
            case TileDown.Direction.Right:
                _playerAnim.Play("PlayerDashSide");
                _playerAnim.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                break;
            case TileDown.Direction.Up:
                _playerAnim.Play("PlayerDashBack");
                _playerAnim.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                break;
            case TileDown.Direction.Down:
                _playerAnim.Play("PlayerDashFront");
                Debug.LogWarning("ZOBIZOB");
                _playerAnim.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                break;
        }
    }
}