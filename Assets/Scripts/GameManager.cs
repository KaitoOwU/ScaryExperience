using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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

    private IReadOnlyList<Light2D> TorchLights { get
        {
            return FindObjectsOfType<Light2D>().ToList().FindAll(
            delegate (Light2D light)
            {
                return light.GetComponentInParent<TileUp>() != null && light.GetComponentInParent<TileUp>().type != TileUp.TileUpType.WinTrappe;
            }
            );
        } }

    public int StepAccount { get => _stepAccountNeeded; }
    public bool LevelWin { get; internal set; } = false;

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

        if(TorchLights.Count > 0)
        {
            foreach (Light2D _torchLight in TorchLights)
            {
                StartCoroutine(FlickerLight(_torchLight));
            }
        }

    }

    private IEnumerator FlickerLight(Light2D torchLight)
    {
        float baseIntensity = torchLight.intensity;
        while (torchLight.gameObject != null)
        {
            yield return DOTween.To(() => torchLight.intensity, x => torchLight.intensity = x, Random.Range(baseIntensity - 0.3f, baseIntensity + 0.3f), Random.Range(0.2f, 0.6f)).WaitForCompletion();
        }
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
        yield return new WaitForSecondsRealtime(0.2f);
        if (flameValue <= 0 && _loseScreen != null && !_winScreen.activeInHierarchy)
        {
            _loseScreen.SetActive(true);
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