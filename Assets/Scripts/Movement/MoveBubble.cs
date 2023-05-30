using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Etouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Tilemaps;

public class MoveBubble : MonoBehaviour
{
    [Header ("- References -")]
    [SerializeField] GameManager manager;

    [Header("- Stats -")]
    [SerializeField] float _delayLerpMove;
    [SerializeField] AnimationCurve _curveLerp;
    [SerializeField] AnimationCurve _curveLerpIce;

    [Header("Buddy")]
    [SerializeField] AnimationCurve _curveBuddy;
    [SerializeField] GameObject _buddy;
    [SerializeField] float _delayMovement;

    //runtime
    float _movementAmount;
    bool _isMoving = false;
    float _currentDelayLerpMove;
    float _moveTimer = 0;
    Vector3 _goToPosition;
    Vector3 _distFromPlayer;
    Vector3 _startPos;
    Vector2 _startPositionFinger;
    AnimationCurve _currentAnimCurve;

    //use for current standing
    TileType _tileStanding;
    TileUpType _tileStandingUp;
    private int _keyFragmentNumber;
    private int _keyNumber;

    //use for current movement ex: ice skatting
    TileType _tileMoving;
    TileUpType _tileMovingUp;

    public enum TileType
    {
        Rock,
        Ice,
        Void,
        Water,
        Wind,
        Breakable
    }

    public enum TileUpType
    {
        None,
        Wall,
        Door,
        Lever,
        KeyFragment,
        KeyDoor,
        OneWayWall,
        Brasero,
        PortalDoor
    }

    private void Awake()
    {
        Etouch.EnhancedTouchSupport.Enable();

        Etouch.Touch.onFingerDown += Touch_onFingerDown;
        Etouch.Touch.onFingerUp += Touch_onFingerUp;

        _distFromPlayer = _buddy.transform.position - transform.position;
        _currentDelayLerpMove = _delayLerpMove;
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
        if ((_startPos.y != _goToPosition.y && _startPos.x != _goToPosition.x) && _tileMovingUp != TileUpType.PortalDoor)
        {
            yield return StartCoroutine(MoveToPos(CheckReturnDiagonal(), 0.1f)); 
        }

        _startPos = transform.position;
        _moveTimer = 0;

        while (_moveTimer < _currentDelayLerpMove)
        {
            _moveTimer += Time.fixedDeltaTime;
            CheckStandingDownTile(transform.position);

            transform.position = Vector3.Lerp(_startPos, _goToPosition, _currentAnimCurve.Evaluate(_moveTimer / _currentDelayLerpMove));

            _buddy.transform.position = Vector3.Lerp(_startPos + _distFromPlayer, _goToPosition + _distFromPlayer, _curveBuddy.Evaluate(_moveTimer / _currentDelayLerpMove));

            yield return new WaitForFixedUpdate();
        }

        _isMoving = false;
        _currentDelayLerpMove = _delayLerpMove;
        _tileMoving = TileType.Rock;
    }

    private void CheckNextTileEffect(TileDown.Direction direction)
    {
        TileDown tempTile = manager.tileMap.FindTileWithPos(_goToPosition);
        TileUp tempTileUp = manager.tileUpMap.FindTileWithPos(_goToPosition);

        switch (tempTileUp.type)
        {
            case TileUpType.Wall:
                GoBack(direction);
                return;

            case TileUpType.Door:
                if (!tempTileUp.isActivated)
                {
                    GoBack(direction);
                    return;
                }
                else
                {
                    tempTileUp.GoBackToWhite();
                    break;
                }

            case TileUpType.Lever:
                tempTileUp.isActivated = true;
                tempTileUp.door.GetComponent<TileUp>().isActivated = true;
                break;

            case TileUpType.KeyFragment:
                if (!tempTileUp.isActivated)
                {
                    tempTileUp.isActivated = true;
                    AddKeyFragment(1);
                    tempTileUp.GoBackToWhite();
                }
                break;

            case TileUpType.KeyDoor:
                if (!tempTileUp.isActivated)
                {
                    if (UseKey(tempTileUp.numberKeyRequired))
                    {
                        tempTileUp.isActivated = true;
                        tempTileUp.GoBackToWhite();
                    }
                }
                break;

            case TileUpType.OneWayWall:
                if (direction == tempTileUp.directionToGoThrough)
                {
                    break;
                }
                else
                {
                    GoBack(direction);
                    return;
                }

            case TileUpType.Brasero:
                if (!tempTileUp.isActivated)
                {
                    GetComponent<FlameManager>().ModifyFlame(false, tempTileUp.refillAmount);
                    tempTileUp.isActivated = true;
                    tempTileUp.GoBackToWhite();
                }
                break;

            case TileUpType.PortalDoor:
                _startPos = tempTileUp.otherDoor.transform.position;
                _goToPosition = tempTileUp.otherDoor.transform.position;
                GoBack(direction);
                _tileMovingUp = TileUpType.PortalDoor;
                return;

            default:
                break;
        }

        switch (tempTile.type)
        {
            case TileType.Rock:
                //rock solid !
                GetComponent<FlameManager>().ModifyFlame(true, 1);
                break;

            case TileType.Ice:
                if (_tileMoving != TileType.Wind)
                {
                    //zooppppppp ice
                    MoveNextTile(direction);
                    _currentDelayLerpMove += _delayLerpMove;
                    _tileMoving = TileType.Ice;
                }
                break;

            case TileType.Void:
                //die void
                Die();
                break;

            case TileType.Water:
                //glou glou water
                Die();
                break;

            case TileType.Wind:
                //wind fiouuuuuu
                _tileMoving = TileType.Wind;

                Tile temp = null;
                Tile tempNext = manager.tileMap.FindTileWithPos(_goToPosition);

                for (int i = 0; i < tempTile.pushNumberTiles; i++)
                {
                    if (temp != tempNext)
                    {
                        temp = tempNext;
                        tempNext = MoveNextTile(tempTile.direction);
                        _currentDelayLerpMove += _delayLerpMove;
                        if (i != 0 && i != 1)
                        {
                            GetComponent<FlameManager>().ModifyFlame(true, 1);
                        }
                    }
                };
                break;

            case TileType.Breakable:
                if (!tempTile.isActivated)
                {
                    tempTile.isActivated = true;
                    break;
                }
                else
                {
                    //Tu meurs bozooo
                    Die();
                    break;
                }

            default:
                break;
        }
    }

    private void AddKeyFragment (int number)
    {
        _keyFragmentNumber += number;

        _keyNumber += _keyFragmentNumber / 3;
        Debug.LogWarning(_keyFragmentNumber / 3);
        _keyFragmentNumber %= 3;
    }

    private bool UseKey (int number)
    {
        if (_keyNumber >= number)
        {
            _keyNumber -= number;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void GoBack(TileDown.Direction direction)
    {
        //trouve le vector d'ajout de position selon la direction du slide
        _goToPosition -= DirectionAddMovePos(direction);

        //trouve le millieu de la tile ou l'on atterie
        _goToPosition = manager.tileMap.FindTileWithPos(_goToPosition).transform.position;
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

        if (fingerTouchDelta == Vector2.zero)
        {
            return;
        }

        //bloque le mouvement selon le mouvement et la tile ou l'on est
        switch (_tileMoving)
        {
            case TileType.Rock:
                break;

            case TileType.Ice:
            case TileType.Void:
            case TileType.Water:
            case TileType.Wind:
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
            case TileType.Ice:
            case TileType.Wind:
                _currentAnimCurve = _curveLerpIce;
                break;
            
            //normal movement
            case TileType.Void:
            case TileType.Water:
            case TileType.Rock:
                _currentAnimCurve = _curveLerp;
                break;

            //other
            default:
                _currentAnimCurve = _curveLerp;
                break;
        }
    }

    private Vector3 DirectionAddMovePos (TileDown.Direction direction)
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

    private void Die ()
    {
        GetComponent<FlameManager>().ModifyFlame(true, 10000);
    }

    private void Touch_onFingerDown(Etouch.Finger finger)
    {
        _startPositionFinger = finger.screenPosition;
    }
}
