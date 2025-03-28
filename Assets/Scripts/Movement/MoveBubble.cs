using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static TileUp;
using static UnityEngine.Rendering.DebugUI;
using Etouch = UnityEngine.InputSystem.EnhancedTouch;

public class MoveBubble : MonoBehaviour
{
    [Header("- References -")]
    [SerializeField] Light2D _generalLight;
    [SerializeField] MonsterSpawn _monsterSpawn;

    [Header("- Stats -")]
    [SerializeField] float _slideSensitivity;
    [SerializeField] float _delayLerpMove;
    [SerializeField] AnimationCurve _curveLerp;
    [SerializeField] AnimationCurve _curveLerpIce;
    [SerializeField] AnimationCurve _curveLerpWind;
    [SerializeField] ParticleSystem _particleSlide;
    [SerializeField] float _noPassTime;
    [SerializeField] float _noPassTimeAfter;

    [SerializeField] ParticleSystem _particleMove;
    [Header("- EDITOR -")]
    [SerializeField] TileMap tileMapEDITORforButton;

    //runtime public
    [HideInInspector] public float currentDelayLerpMove;
    [HideInInspector] public Vector3 goToPosition;

    //runtime private
    //bool
    bool _firstMove = true;
    bool _isMoving = false;
    bool _canMove = true;
    bool _isSliding = false;
    bool _shouldStopCheckingTile;
    bool _collectibleAcquired = false;

    //vector
    List<Vector3> _prePosList;
    Vector3 _distFromPlayer;
    Vector3 _startPos;
    Vector2 _startPositionFinger;

    //number
    float _movementAmount;
    float _moveTimer = 0;
    int _keyFragmentNumber;

    //other
    AnimationCurve _currentAnimCurve;
    AudioManager _audioManager;
    TileDown.Direction _direction;
    FlameManager _flameManager;
    TileDown.TileType _oldType;

    //use for current standing
    TileDown.TileType _tileStanding;
    TileUp.TileUpType _tileStandingUp;

    //use for current movement ex: ice skatting
    TileDown.TileType _tileMoving;
    TileUp.TileUpType _tileMovingUp;

    public Action OnDie;
    public Action OnWin;
    public Action OnFirstMove;
    public Action OnKeyTaken;
    public Action OnCollectableTaken;

    public int _numberOfSteps;
    
    private void Awake()
    {
        Etouch.EnhancedTouchSupport.Enable();

        Etouch.Touch.onFingerDown += Touch_onFingerDown;
        Etouch.Touch.onFingerUp += Touch_onFingerUp;

        OnWin += Win;
        OnDie += Die;

        currentDelayLerpMove = _delayLerpMove;
    }

    private void OnDisable()
    {
        Etouch.Touch.onFingerDown -= Touch_onFingerDown;
        Etouch.Touch.onFingerUp -= Touch_onFingerUp;
    }

    [Button("ActualisePOS")]
    private void ActualisePos()
    {
        TileDown tempTile = tileMapEDITORforButton.FindTileWithPosEditor(transform.position);
        transform.position = tempTile.transform.position;
    }


    private void Start()
    {
        goToPosition = transform.position;

        _movementAmount = GameManager.Instance.tileMap.tileSize;
        _prePosList = new List<Vector3>();
        _flameManager = GetComponent<FlameManager>();
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        _currentAnimCurve = _curveLerp;
        //Debug.LogWarning(_movementAmount);
    }

    //coroutine qui deplace le joueur vers la position (simple)
    private IEnumerator MoveToPos (Vector3 prePos, float delay)
    {
        _startPos = transform.position;
        _moveTimer = 0;

        while (_moveTimer < delay)
        {
            _moveTimer += Time.fixedDeltaTime;

            transform.position = Vector3.Lerp(_startPos, prePos, _currentAnimCurve.Evaluate(_moveTimer / delay));

            yield return new WaitForFixedUpdate();
        }
    }


    private void Win()
    {
        if (GameManager.Instance.LevelWin)
        {
            return;
        }

        GameManager.Instance.LevelWin = true;
        Etouch.Touch.onFingerDown -= Touch_onFingerDown;
        Etouch.Touch.onFingerUp -= Touch_onFingerUp;

        if(DataManager.Instance != null)
        {
            FlameState state;

            if(_collectibleAcquired)
            {
                if (_numberOfSteps <= GameManager.Instance.StepAccountWithCollectible)
                {
                    state = FlameState.Gold;
                }
                else
                {
                    state = FlameState.None;
                }
            } else
            {
                if(_numberOfSteps <= GameManager.Instance.StepAccountNoCollectible)
                {
                    state = FlameState.Silver;

                }
                else
                {
                    state = FlameState.None;
                }
                
            }

            DataManager.Instance.LevelData[DataManager.Instance.CurrentLevel].Complete(_collectibleAcquired, state);
            SaveSystem.SaveData(DataManager.Instance.LevelData, DataManager.Instance.DeathAmount, DataManager.Instance.SkullObtained, DataManager.Instance.LevelCompletedAmount, DataManager.Instance.GoldenFlameObtained, DataManager.Instance.SilverFlameObtained, DataManager.Instance.CurrentVersion);
        }

        DataManager.Instance.CheckForGenericAchievement();
        GameManager.Instance.WinScreen.SetActive(true);
        long[] pattern = { 100, 100, 500 };
        Vibration.Vibrate(pattern, -1);
    }
    private void Die ()
    {
        DataManager.Instance.DeathAmount++;
        _flameManager.isDeadFromMonsters = false;
        GetComponent<FlameManager>().ModifyFlame(true, 10000);
        Etouch.Touch.onFingerDown -= Touch_onFingerDown;
        Etouch.Touch.onFingerUp -= Touch_onFingerUp;
        _monsterSpawn.playerIsDead = true;
        GameManager.Instance.LoseScreen.SetActive(true);
        DataManager.Instance.CheckForGenericAchievement();

        Vibration.Vibrate(100);
    }

    private void ChangeRendering (bool isVisible)
    {
        GetComponent<SpriteRenderer>().enabled = isVisible;
    }

    private IEnumerator MoveToPosition ()
    {
        _startPos = transform.position;

        ////particules movement
        //_particleMove.Play();
        //_particleMove.transform.rotation = Quaternion.LookRotation(_startPos - goToPosition);

        //movement intermediaire
        while (_prePosList.Count > 0 && _prePosList[0] != goToPosition)
        {
            yield return StartCoroutine(MoveToPos(_prePosList[0], _delayLerpMove));
            _prePosList.RemoveAt(0);
        }
        _prePosList.Clear();

        //particules
        if (_tileMoving == TileDown.TileType.Ice)
        {
            _particleSlide.Play();
            _particleSlide.transform.rotation = Quaternion.LookRotation(_startPos - goToPosition);
        }

        else if (_tileMoving == TileDown.TileType.Rock || _tileMoving == TileDown.TileType.Breakable)
        {
            _particleMove.Play();
            _particleMove.transform.rotation = Quaternion.LookRotation(_startPos - goToPosition);
        }

        _startPos = transform.position;
        _moveTimer = 0;

        while (_moveTimer < currentDelayLerpMove)
        {
            _moveTimer += Time.fixedDeltaTime;
            CheckStandingDownTile(transform.position);
            CheckStandingUpTile(transform.position);

            transform.position = Vector3.Lerp(_startPos, goToPosition, _currentAnimCurve.Evaluate(_moveTimer / currentDelayLerpMove));

            if (_moveTimer/currentDelayLerpMove > 0.8f && _particleSlide.isPlaying)
            {
                _particleSlide.Stop();
            }

            if (_particleMove.isPlaying && _moveTimer / currentDelayLerpMove > 0.8f)
            {
                _particleMove.Stop();
            }

            yield return new WaitForFixedUpdate();
        }

        // reset variables apres mouvement
        _isMoving = false;
        currentDelayLerpMove = _delayLerpMove;
        _tileMoving = TileDown.TileType.Rock;
        _tileMovingUp = TileUp.TileUpType.None;
        _isSliding = false;

        if (_particleSlide.isPlaying)
        {
            _particleSlide.Stop();
        }

        if (_particleMove.isPlaying)
        {
            _particleMove.Stop();
        }
    }

    private void SwitchOnTileUp (TileUp tempTileUp, TileDown.Direction direction)
    {
        _shouldStopCheckingTile = false;
        GameManager.Instance.AnimatePlayer(direction);

        switch (tempTileUp.type)
        {
            case TileUp.TileUpType.Wall:

                _isSliding = false;
                GoBack(direction);

                // animation de blockage et pas dans une animation
                //if (tempTileUp.moveTimer == 0)
                //{
                //    tempTileUp.StartCoroutine(tempTileUp.BlockedByWall(_noPassTime, _noPassTimeAfter));
                //    Vibration.Vibrate(200);
                //}

                _shouldStopCheckingTile = true;
                return;

            case TileUp.TileUpType.Ventilateur:

                GoBack(direction);
                _shouldStopCheckingTile = true;
                return;

            case TileUp.TileUpType.Key:
                if (!tempTileUp.isActivated)
                {
                    tempTileUp.isActivated = true;

                    GrilleCadenas _refs = GameManager.Instance.Grid.GetComponent<GrilleCadenas>();
                    _refs.Locker.transform.DOScale(4, 1f);
                    _refs.Locker.DOColor(new(1, 1, 1, 0), 1f).OnComplete(() =>
                    {
                        _refs.Grid.transform.DOLocalMoveY(1, 3f);
                        _refs.Grid.DOColor(new(1, 1, 1, 0), 3f);
                    });
                    Destroy(_refs.gameObject, 6f);
                    OnKeyTaken?.Invoke();
                }
                break;
            case TileUp.TileUpType.Brasero:
                GetComponent<FlameManager>().ModifyFlame(false, tempTileUp.refillAmountBrasero);
                _numberOfSteps += 1;
                _shouldStopCheckingTile = true;
                return;

            case TileUp.TileUpType.Torch:
                if (!tempTileUp.isActivated)
                {
                    GetComponent<FlameManager>().ModifyFlame(false, tempTileUp.refillAmountTorch);
                    _numberOfSteps += 1;
                    tempTileUp.isActivated = true;
                    tempTileUp.SwitchOffTorch();
                    _shouldStopCheckingTile = true;
                    tempTileUp.type = TileUpType.None;
                    tempTileUp.GoBackToWhite();
                }
                break;

            case TileUp.TileUpType.Block:
                if (_tileMovingUp != TileUp.TileUpType.Wind && !_isSliding)
                {
                    //push le block
                    bool isBlocked = PushBlock(direction, tempTileUp);

                    //si pas blocker
                    if (!isBlocked)
                    {
                        Vibration.Vibrate(100);

                        //re-check car on vient de modif la tileup sur laquelle on va marcher (block -> wind)
                        if (tempTileUp.type == TileUpType.Wind)
                        {
                            SwitchOnTileUp(tempTileUp, direction);
                        }
                    }
                    else
                    {
                        GoBack(direction);
                        _shouldStopCheckingTile = true;
                    }
                    break;
                }
                else
                {
                    // si le rocher a été bloquer par mur
                    GoBack(direction);
                    _shouldStopCheckingTile = true;
                    break;
                }

            case TileUp.TileUpType.WinTrappe:
                if (!tempTileUp.isActivated)
                {
                    if (UseKey(tempTileUp.numberPartKeyRequired))
                    {
                        tempTileUp.isActivated = true;
                    }
                    else
                    {
                        GoBack(direction);
                        _shouldStopCheckingTile = true;
                        break;
                    }
                }
                break;

            case TileUp.TileUpType.Collectible:
                if (!tempTileUp.isActivated)
                {
                    tempTileUp.isActivated = true;
                    _collectibleAcquired = true;
                    Vibration.Vibrate(200);
                }
                break;

            case TileUp.TileUpType.Wind:
                //wind fiouuuuuu

                if (!tempTileUp.isActivated)
                {
                    TileDown tempTile = GameManager.Instance.tileMap.FindTileWithPos(goToPosition);
                    Vector3 posTileBefore = tempTileUp.transform.position - DirectionAddMovePos(direction);
                    TileUp tempBeforeUp = GameManager.Instance.tileUpMap.FindTileWithPos(posTileBefore);

                    // si l'on change de direction, on ajoute une position a la liste de position a se deplacer
                    if (tempBeforeUp.direction != tempTileUp.direction)
                    {
                        _prePosList.Add(tempTileUp.transform.position);
                    }
                    
                    //enleve une flamme sur le premier deplacement et ajoute l'emplacement dans la liste a deplacer
                    if (_tileMovingUp != TileUp.TileUpType.Wind)
                    {
                        _numberOfSteps += 1;
                        _flameManager.ModifyFlame(true, 1);
                        _prePosList.Add(tempTileUp.transform.position);
                    }

                    _tileMovingUp = TileUp.TileUpType.Wind;

                    //vérifie sur quelle tile je marche DOWN
                    SwitchOnTileDown(tempTile, direction);

                    //si le prochain bloque n'est plus du vent alors on ne check plus la suite
                    if (_tileMovingUp != TileUp.TileUpType.Wind)
                    {
                        return;
                    }

                    MoveNextTile(tempTileUp.direction);
                    //currentDelayLerpMove += _delayLerpMove;
                }
                break;

            default:
                break;
        }
    }

    private void SwitchOnTileDown (TileDown tempTile, TileDown.Direction direction)
    {
        GameManager.Instance.AnimatePlayer(direction);
        TileDown tempTileDown = GameManager.Instance.tileMap.FindTileWithPos(goToPosition);
        Vector3 test = tempTile.transform.position - DirectionAddMovePos(direction);
        TileDown tempBeforeDown = GameManager.Instance.tileMap.FindTileWithPos(test);

        switch (tempTile.type)
        {
            case TileDown.TileType.Rock:
            case TileDown.TileType.WaterRock:
                _isSliding = false;
                if (_tileMovingUp != TileUp.TileUpType.Wind && tempBeforeDown.type != TileDown.TileType.Ice)
                {
                    _numberOfSteps += 1;
                    _flameManager.ModifyFlame(true, 1);
                }
                //rock solid !
                break;

            case TileDown.TileType.Ice:
                
                // premier movement sur glace
                if (!_isSliding)
                {
                    _numberOfSteps += 1;
                    _isSliding = true;
                }

                if (_tileMovingUp != TileUp.TileUpType.Wind)
                {
                    _tileMoving = TileDown.TileType.Ice;
                    currentDelayLerpMove += _delayLerpMove;
                    MoveNextTile(direction);
                }

                // fin de movement
                if (_tileMoving == TileDown.TileType.Ice)
                {
                    _tileMoving = TileDown.TileType.Rock;
                    _flameManager.ModifyFlame(true, 1);
                }

                break;

            case TileDown.TileType.Void:
                _isSliding = false;
                _audioManager.PlaySFX(_audioManager.fallSound);
                transform.DOScale(0, 1f);
                Social.ReportProgress(GPGSIds.achievement_lack_of_balance, 100f, null);
                OnDie?.Invoke();
                break;

            case TileDown.TileType.Water:
                _isSliding = false;
                _audioManager.PlaySFX(_audioManager.waterSound);
                OnDie?.Invoke();
                transform.DOScale(0.7f, 1f);
                GetComponent<SpriteRenderer>().DOColor(new(0, 0, 1, 0), 2f);
                Social.ReportProgress(GPGSIds.achievement_bloup_bloup_bloup, 100f, null);

                if (_tileMovingUp == TileUp.TileUpType.Wind)
                {
                    _tileMovingUp = TileUp.TileUpType.None;
                    goToPosition = tempTile.transform.position;
                }
                break;

            case TileDown.TileType.Breakable:
                _isSliding = false;
                if (_tileMovingUp != TileUp.TileUpType.Wind)
                {
                    _numberOfSteps += 1;
                    _flameManager.ModifyFlame(true, 1);
                }

                if (!tempTile.isActivated)
                {
                    tempTile.isActivated = true;
                    break;
                }
                else
                {
                    _audioManager.PlaySFX(_audioManager.fallSound);
                    transform.DOScale(0, 1f);
                    OnDie?.Invoke();
                    Social.ReportProgress(GPGSIds.achievement_lack_of_balance, 100f, null);
                    break;
                }

            default:
                break;
        }
    }

    private void CheckNextTileEffect(TileDown.Direction direction)
    {
        TileDown tempTile = GameManager.Instance.tileMap.FindTileWithPos(goToPosition);
        TileUp tempTileUp = GameManager.Instance.tileUpMap.FindTileWithPos(goToPosition);

        SwitchOnTileUp(tempTileUp, direction);

        Vector3 beforePos = tempTile.transform.position - DirectionAddMovePos(direction);
        TileDown tempBeforeDown = GameManager.Instance.tileMap.FindTileWithPos(beforePos);

        if (tempBeforeDown.type == TileDown.TileType.Breakable && tempBeforeDown.isActivated)
        {
            tempBeforeDown.GetComponent<SpriteRenderer>().sprite = tempTile.spritesDown.spriteBreakable[1];
        }

        //si l'on veut arreter de check les tiles down
        if (!_shouldStopCheckingTile)
        {
            SwitchOnTileDown(tempTile, direction);
        }
    }

    private void AddKeyFragment (int number)
    {
        _keyFragmentNumber += number;
    }

    private bool UseKey (int number)
    {
        if (_keyFragmentNumber >= number)
        {
            _keyFragmentNumber -= number;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void GoBack(TileDown.Direction direction)
    {
        //trouve le vector d'ajout de position selon la direction du slide
        goToPosition -= DirectionAddMovePos(direction);

        //trouve le millieu de la tile ou l'on atterie
        goToPosition = GameManager.Instance.tileMap.FindTileWithPos(goToPosition).transform.position;
    }

    private bool PushBlock(TileDown.Direction direction, TileUp TileBlock)
    {
        //commence le mouvement du block et check si le block est bloquer
        bool isBlocked = TileBlock.block.GetComponent<BlockToMove>().StartMoving(direction, this);
        return isBlocked;
    }

    private TileDown CheckStandingDownTile(Vector3 pos)
    {

        TileDown temp = GameManager.Instance.tileMap.FindTileWithPos(pos);

        if (temp != null)
        {
            _tileStanding = temp.type;
            return temp;
        }

        return null;
    }

    private TileUp CheckStandingUpTile(Vector3 pos)
    {
        TileUp temp = GameManager.Instance.tileUpMap.FindTileWithPos(pos);

        if (temp != null)
        {
            _tileStandingUp = temp.type;

            // si l'on passe sur un collectible actuellement (physiquement)
            if (_tileStandingUp == TileUpType.Collectible && temp.isActivated)
            {
                _audioManager.PlaySFX(_audioManager.cranePickUpSound);
                temp.GoBackToWhite();
                OnCollectableTaken?.Invoke();
                temp.type = TileUpType.None;
            }

            // si l'on passe sur une key actuellement (physiquement)
            if (_tileStandingUp == TileUpType.Key && temp.isActivated)
            {
                _audioManager.PlaySFX(_audioManager.keyPickUp);
                temp.type = TileUpType.None;
                Destroy(temp.lightKey);
                AddKeyFragment(1);
                Vibration.Vibrate(25);
                temp.GoBackToWhite();
            }

            if (_tileStandingUp == TileUpType.WinTrappe && temp.isActivated)
            {
                Win();
            }

            return temp;
        }

        return null;
    }


    private Tile MoveNextTile(TileDown.Direction direction)
    {
        //trouve le vector d'ajout de position selon la direction du slide
        goToPosition += DirectionAddMovePos(direction);
        Tile tileToMove = GameManager.Instance.tileUpMap.FindTileWithPos(goToPosition);

        //trouve le millieu de la tile ou l'on atterie
        goToPosition = tileToMove.transform.position;

        //hors du level
        if (tileToMove == null) {
            OnDie?.Invoke();
            return null;
        }

        //verifie si la tile ou l'on va bouger contiens un effet, si oui applique l'effet
        CheckNextTileEffect(direction);

        return tileToMove;
    }


    private void Touch_onFingerUp(Etouch.Finger finger) 
    {
        Vector2 fingerTouchDelta = finger.screenPosition - _startPositionFinger;

        // check if swipe < slide horizontal
        if (Mathf.Abs(fingerTouchDelta.x) >= Mathf.Abs(fingerTouchDelta.y) && Mathf.Abs(fingerTouchDelta.x) <= _slideSensitivity)
        {
            
            return;
        }
        // check if swipe < slide vertical
        else if (Mathf.Abs(fingerTouchDelta.y) >= Mathf.Abs(fingerTouchDelta.x) && Mathf.Abs(fingerTouchDelta.y) <= _slideSensitivity)
        {
            
            return;
        }

        if (_isMoving)
        {
            return;
        }

        //bloque le mouvement selon le mouvement et la tile ou l'on est
        switch (_tileMoving)
        {
            case TileDown.TileType.Rock:
                break;

            case TileDown.TileType.Ice:
            case TileDown.TileType.Void:
            case TileDown.TileType.Water:
                return;
        }

        switch (_tileMovingUp)
        {
            case TileUp.TileUpType.Wind:
                return;
        }

        
        //trouve le vector d'ajout de position selon la direction du slide
        goToPosition += VectorAddMovePos(fingerTouchDelta);

        //trouve le millieu de la tile ou l'on atterie
        goToPosition = GameManager.Instance.tileMap.FindTileWithPos(goToPosition).transform.position;

        //verifie si la tile ou l'on va bouger contiens un effet, si oui applique l'effet
        CheckNextTileEffect(FingerToDirection(finger));

        //change anim curve based on tile type current moving
        ChangeAnimCurve();

        if (_firstMove)
        {
            DOTween.To(() => _flameManager._light.pointLightOuterRadius, x => _flameManager._light.pointLightOuterRadius = x, _flameManager._maxSizeLight, 1f).SetEase(Ease.OutExpo);

            _firstMove = false;
            _monsterSpawn.StartSpawn();
            DOTween.To(() => _generalLight.intensity, x => _generalLight.intensity = x, 0f, 1f).SetEase(Ease.OutExpo);
        }

        // si l'on bouge pas encore, lance l'animation
        if (!_isMoving)
        {
            Debug.LogWarning("go to pos : " + goToPosition + " / Position : " + transform.position);
            if (goToPosition == transform.position)
            {
                _prePosList.Add(goToPosition + VectorAddMovePos(fingerTouchDelta) / 3);
                _prePosList.Add(goToPosition);
            }

            StartCoroutine(MoveToPosition());
            _isMoving = true;
        }
        // si l'on bouge deja, reset l'animation pour commencer a partir de la pos actuel
        else
        {
            _startPos = transform.position;
            _moveTimer = 0;
        }

        _audioManager.PlaySFX(_audioManager.movementSounds[UnityEngine.Random.Range(0, _audioManager.movementSounds.Count)]);
    }

    private void ChangeAnimCurve ()
    {
        switch (_tileMoving)
        {
            // ice movement
            case TileDown.TileType.Ice:
            
            //normal movement
            case TileDown.TileType.Void:
            case TileDown.TileType.Water:
            case TileDown.TileType.Rock:
                _currentAnimCurve = _curveLerp;
                break;

            //other
            default:
                _currentAnimCurve = _curveLerp;
                break;
        }

        switch (_tileMovingUp)
        {
            case TileUp.TileUpType.Wind:
                _currentAnimCurve = _curveLerpWind;
                break;
        }
    }

    public Vector3 DirectionAddMovePos (TileDown.Direction direction)
    {
        switch (direction)
        {
            case TileDown.Direction.Left:
                return new Vector3(-_movementAmount, 0, 0);
            case TileDown.Direction.Right:
                return new Vector3(_movementAmount, 0, 0);
            case TileDown.Direction.Up:
                return new Vector3(0, _movementAmount, 0);
            case TileDown.Direction.Down:
                return new Vector3(0, -_movementAmount, 0);
        }

        return Vector3.zero;
    }

    private Vector3 VectorAddMovePos(Vector2 fingerTouchDelta)
    {
        //x
        if (Mathf.Abs(fingerTouchDelta.x) > Mathf.Abs(fingerTouchDelta.y))
        {
            //move right
            if (fingerTouchDelta.x > 0) { return new Vector3(_movementAmount, 0, 0); }
            //move left
            else { return new Vector3(-_movementAmount, 0, 0); }
        }

        //y
        else if (Mathf.Abs(fingerTouchDelta.y) > Mathf.Abs(fingerTouchDelta.x))
        {
            //move up
            if (fingerTouchDelta.y > 0) { return new Vector3(0, _movementAmount, 0); }
            //move down
            else { return new Vector3(0, -_movementAmount, 0); }
        }

        return Vector3.zero;
    }

    private TileDown.Direction FingerToDirection (Etouch.Finger finger)
    {
        Vector2 fingerTouchDelta = finger.screenPosition - _startPositionFinger;

        //x
        if (Mathf.Abs(fingerTouchDelta.x) > Mathf.Abs(fingerTouchDelta.y))
        {
            //move right
            if (fingerTouchDelta.x > 0) { return TileDown.Direction.Right; }

            //move left
            else { return TileDown.Direction.Left; }
        }

        //y
        else if (Mathf.Abs(fingerTouchDelta.y) > Mathf.Abs(fingerTouchDelta.x))
        {
            //move up
            if (fingerTouchDelta.y > 0) { return TileDown.Direction.Up; }

            //move down
            else { return TileDown.Direction.Down; }
        }

        return TileDown.Direction.Down;
    }

    private void Touch_onFingerDown(Etouch.Finger finger)
    {
        _startPositionFinger = finger.screenPosition;
    }

    internal void SetTouchControlsActive(bool active)
    {
        if (active)
        {
            Etouch.Touch.onFingerDown += Touch_onFingerDown;
            Etouch.Touch.onFingerUp += Touch_onFingerUp;
        } else
        {
            Etouch.Touch.onFingerDown -= Touch_onFingerDown;
            Etouch.Touch.onFingerUp -= Touch_onFingerUp;
        }
    }

}