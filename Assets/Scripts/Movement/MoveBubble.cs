using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Etouch = UnityEngine.InputSystem.EnhancedTouch;
using System;

public class MoveBubble : MonoBehaviour
{
    [Header("- References -")]
    [SerializeField] public GameManager manager;

    [Header("- Stats -")]
    [SerializeField] float _slideSensitivity;
    [SerializeField] float _delayLerpMove;
    [SerializeField] AnimationCurve _curveLerp;
    [SerializeField] AnimationCurve _curveLerpIce;

    [Header("Buddy")]
    [SerializeField] AnimationCurve _curveBuddy;
    [SerializeField] GameObject _buddy;
    [SerializeField] float _delayMovement;

    //runtime public
    [HideInInspector] public float currentDelayLerpMove;

    //runtime private
    float _movementAmount;
    bool _isMoving = false;
    bool _canMove = true;
    float _moveTimer = 0;
    Vector3 _goToPosition;
    Vector3 _distFromPlayer;
    Vector3 _startPos;
    Vector2 _startPositionFinger;
    AnimationCurve _currentAnimCurve;
    bool _shouldStopCheckingTile;
    private int _keyFragmentNumber;
    bool _collectibleAcquired = false;

    //use for current standing
    TileDown.TileType _tileStanding;
    TileUp.TileUpType _tileStandingUp;

    //use for current movement ex: ice skatting
    TileDown.TileType _tileMoving;
    TileUp.TileUpType _tileMovingUp;

    public Action OnDie;
    public Action OnWin;
    
    private void Awake()
    {
        Etouch.EnhancedTouchSupport.Enable();

        Etouch.Touch.onFingerDown += Touch_onFingerDown;
        Etouch.Touch.onFingerUp += Touch_onFingerUp;

        OnWin += Win;
        OnDie += Die;

        _distFromPlayer = _buddy.transform.position - transform.position;
        currentDelayLerpMove = _delayLerpMove;
        _goToPosition = transform.position;
    }

    private void Start()
    {
        _movementAmount = manager.tileMap.tileSize;
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
        Debug.LogWarning("WINNN !!!!");

        Etouch.Touch.onFingerDown -= Touch_onFingerDown;
        Etouch.Touch.onFingerUp -= Touch_onFingerUp;

        //GameManager.Instance.LevelData[GameManager.Instance.CurrentLevel].Complete(_collectibleAcquired);
        //SaveSystem.SaveData(GameManager.Instance.LevelData);

        GameManager.Instance.WinScreen.SetActive(true);
    }
    private void Die ()
    {
        GetComponent<FlameManager>().ModifyFlame(true, 10000);
        Etouch.Touch.onFingerDown -= Touch_onFingerDown;
        Etouch.Touch.onFingerUp -= Touch_onFingerUp;

        GameManager.Instance.LoseScreen.SetActive(true);
    }

    private void ChangeRendering (bool isVisible)
    {
        GetComponent<SpriteRenderer>().enabled = isVisible;
    }

    // check if going in diagonal and return
    private Vector3 CheckReturnDiagonal ()
    {
        Vector3 preGoToPosition;

        //Debug.LogWarning("start x : " + _startPos.x + " / y : " + _startPos.y);
        //Debug.LogWarning("end x : " + _goToPosition.x + " / y : " + _goToPosition.y);

        if (_startPos.y != _goToPosition.y && _startPos.x != _goToPosition.x)
        {
            if (Mathf.Abs(_goToPosition.y - _startPos.y) < Mathf.Abs(_goToPosition.x - _startPos.x))
            {
                preGoToPosition = new Vector3(_startPos.x, _goToPosition.y, 0);
            }
            else
            {
                preGoToPosition = new Vector3(_goToPosition.x, _startPos.y, 0);
            }

            return preGoToPosition;
        }

        return Vector3.zero;
    }

    private IEnumerator MoveToPosition ()
    {
        _startPos = transform.position;

        // si l'on va en diagonal
        if ((_startPos.y != _goToPosition.y && _startPos.x != _goToPosition.x))
        {
            yield return StartCoroutine(MoveToPos(CheckReturnDiagonal(), _delayLerpMove)); 
        }

        _startPos = transform.position;
        _moveTimer = 0;

        while (_moveTimer < currentDelayLerpMove)
        {
            _moveTimer += Time.fixedDeltaTime;
            CheckStandingDownTile(transform.position);

            transform.position = Vector3.Lerp(_startPos, _goToPosition, _currentAnimCurve.Evaluate(_moveTimer / currentDelayLerpMove));

            _buddy.transform.position = Vector3.Lerp(_startPos + _distFromPlayer, _goToPosition + _distFromPlayer, _curveBuddy.Evaluate(_moveTimer / currentDelayLerpMove));

            yield return new WaitForFixedUpdate();
        }

        // reset variables apr�s mouvement
        _isMoving = false;
        currentDelayLerpMove = _delayLerpMove;
        _tileMoving = TileDown.TileType.Rock;
        _tileMovingUp = TileUp.TileUpType.None;
    }

    private void SwitchOnTileUp (TileUp tempTileUp, TileDown.Direction direction)
    {
        _shouldStopCheckingTile = false;

        switch (tempTileUp.type)
        {
            case TileUp.TileUpType.Wall:
                GoBack(direction);
                return;

            case TileUp.TileUpType.KeyFragment:
                if (!tempTileUp.isActivated)
                {
                    tempTileUp.isActivated = true;
                    AddKeyFragment(1);
                    tempTileUp.GoBackToWhite();
                }
                break;

            case TileUp.TileUpType.OneWayWall:
                if (direction == tempTileUp.directionToGoThrough)
                {
                    break;
                }
                else
                {
                    GoBack(direction);
                    _shouldStopCheckingTile = true;
                    return;
                }

            case TileUp.TileUpType.Brasero:
                GetComponent<FlameManager>().ModifyFlame(false, tempTileUp.refillAmount);
                break;

            case TileUp.TileUpType.Torch:
                if (!tempTileUp.isActivated)
                {
                    GetComponent<FlameManager>().ModifyFlame(false, tempTileUp.refillAmount);
                    tempTileUp.isActivated = true;
                    tempTileUp.GoBackToWhite();
                }
                break;

            case TileUp.TileUpType.Block:
                if (PushBlock(direction, tempTileUp))
                {
                    _shouldStopCheckingTile = true;
                    return;
                }
                else
                {
                    break;
                }

            case TileUp.TileUpType.WinTrappe:
                if (!tempTileUp.isActivated)
                {
                    if (UseKey(tempTileUp.numberPartKeyRequired))
                    {
                        tempTileUp.isActivated = true;
                        Win();
                    }
                    else
                    {
                        break;
                    }
                }
                break;

            case TileUp.TileUpType.Collectible:
                if (!tempTileUp.isActivated)
                {
                    tempTileUp.isActivated = true;
                    _collectibleAcquired = true;
                    tempTileUp.GoBackToWhite();
                }
                break;

            default:
                break;
        }
    }

    private void SwitchOnTileDown (TileDown tempTile, TileDown.Direction direction)
    {
        switch (tempTile.type)
        {
            case TileDown.TileType.Rock:
            case TileDown.TileType.WaterRock:
                //rock solid !
                GetComponent<FlameManager>().ModifyFlame(true, 1);
                break;

            case TileDown.TileType.Ice:
                if (_tileMoving != TileDown.TileType.Wind)
                {
                    //zooppppppp ice
                    MoveNextTile(direction);
                    currentDelayLerpMove += _delayLerpMove;
                    _tileMoving = TileDown.TileType.Ice;
                }
                break;

            case TileDown.TileType.Void:
            // die
            case TileDown.TileType.Water:
            //glou glou water
                OnDie?.Invoke();
                break;

            case TileDown.TileType.Wind:
                //wind fiouuuuuu

                if (_tileMoving != TileDown.TileType.Wind && !tempTile.isActivated)
                {
                    _tileMoving = TileDown.TileType.Wind;
                
                    Tile temp = null;
                    Tile tempNext = manager.tileMap.FindTileWithPos(_goToPosition);

                    for (int i = 0; i < tempTile.pushNumberTiles; i++)
                    {
                        if (temp != tempNext)
                        {
                            temp = tempNext;
                            tempNext = MoveNextTile(tempTile.direction);
                            currentDelayLerpMove += _delayLerpMove;
                            if (i != 0 && i != 1)
                            {
                                GetComponent<FlameManager>().ModifyFlame(true, 1);
                            }
                        }
                    };
                }
                break;

            case TileDown.TileType.Breakable:
                if (!tempTile.isActivated)
                {
                    tempTile.isActivated = true;
                    tempTile.GetComponent<SpriteRenderer>().sprite = tempTile.sprites.spriteBreakable[1];
                    break;
                }
                else
                {
                    //Tu meurs bozooo
                    OnDie?.Invoke();
                    break;
                }

            default:
                break;
        }
    }

    private void CheckNextTileEffect(TileDown.Direction direction)
    {
        TileDown tempTile = manager.tileMap.FindTileWithPos(_goToPosition);
        TileUp tempTileUp = manager.tileUpMap.FindTileWithPos(_goToPosition);

        SwitchOnTileUp(tempTileUp, direction);

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
        _goToPosition -= DirectionAddMovePos(direction);

        //trouve le millieu de la tile ou l'on atterie
        _goToPosition = manager.tileMap.FindTileWithPos(_goToPosition).transform.position;
    }

    private bool PushBlock(TileDown.Direction direction, TileUp TileBlock)
    {
        //commence le mouvement du block et check si le block est bloquer
        bool isBlocked = TileBlock.block.GetComponent<BlockToMove>().StartMoving(direction, this);
        return isBlocked;
    }

    private void CheckStandingDownTile(Vector3 pos)
    {
        TileDown temp = manager.tileMap.FindTileWithPos(pos);

        if (temp != null)
        {
            _tileStanding = temp.type;
        }
    }

    private void CheckStandingUpTile(Vector3 pos)
    {
        TileUp temp = manager.tileUpMap.FindTileWithPos(pos);

        if (temp != null)
        {
            _tileStandingUp = temp.type;
        }
    }


    private Tile MoveNextTile(TileDown.Direction direction)
    {
        //trouve le vector d'ajout de position selon la direction du slide
        _goToPosition += DirectionAddMovePos(direction);
        Tile tileToMove = manager.tileMap.FindTileWithPos(_goToPosition);

        //trouve le millieu de la tile ou l'on atterie
        _goToPosition = tileToMove.transform.position;

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

        //bloque le mouvement selon le mouvement et la tile ou l'on est
        switch (_tileMoving)
        {
            case TileDown.TileType.Rock:
                break;

            case TileDown.TileType.Ice:
            case TileDown.TileType.Void:
            case TileDown.TileType.Water:
            case TileDown.TileType.Wind:
                return;
        }


        //trouve le vector d'ajout de position selon la direction du slide
        _goToPosition += VectorAddMovePos(fingerTouchDelta);

        //trouve le millieu de la tile ou l'on atterie
        _goToPosition = manager.tileMap.FindTileWithPos(_goToPosition).transform.position;


        //verifie si la tile ou l'on va bouger contiens un effet, si oui applique l'effet
        CheckNextTileEffect(FingerToDirection(finger));

        //change anim curve based on tile type current moving
        ChangeAnimCurve();

        // si l'on bouge pas encore, lance l'animation
        if (!_isMoving)
        {
            StartCoroutine(MoveToPosition());
            _isMoving = true;
        }
        // si l'on bouge deja, reset l'animation pour commencer a partir de la pos actuel
        else
        {
            _startPos = transform.position;
            _moveTimer = 0;
        }
    }

    private void ChangeAnimCurve ()
    {
        switch (_tileMoving)
        {
            // ice movement
            case TileDown.TileType.Ice:
            case TileDown.TileType.Wind:
                _currentAnimCurve = _curveLerpIce;
                break;
            
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

        return TileDown.Direction.Left;
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