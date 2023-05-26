using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Etouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Tilemaps;

public class MoveBubble : MonoBehaviour
{
    [Header ("- References -")]
    [SerializeField] GameManage manager;

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

    //use for current movement ex: ice skatting
    TileType _tileMoving;
    public enum TileType
    {
        Rock,
        Ice,
        Void,
        Water,
        Wind
    }

    public enum TileUpType
    {
        None,
        Wall,
        Door
    }

    private void Awake()
    {
        Etouch.EnhancedTouchSupport.Enable();

        Etouch.Touch.onFingerMove += Touch_onFingerMove;
        Etouch.Touch.onFingerDown += Touch_onFingerDown;
        Etouch.Touch.onFingerUp += Touch_onFingerUp;

        _goToPosition = transform.position;
       

        _distFromPlayer = _buddy.transform.position - transform.position;
        _currentDelayLerpMove = _delayLerpMove;
    }

    private void Start()
    {
        _movementAmount = manager.tileMap.tileSize;
        Debug.LogWarning(_movementAmount);
    }

    private IEnumerator MoveToPosition ()
    {
        _moveTimer = 0;
        _startPos = transform.position;

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

    private void CheckNextTileEffect (Etouch.Finger finger)
    {
        TileDown tempTile = manager.tileMap.FindTileWithPos(_goToPosition);
        TileUp tempTileUp = manager.tileUpMap.FindTileWithPos(_goToPosition);

        switch (tempTileUp.type)
        {
            case TileUpType.Wall:
                GoBack(finger);
                return;
            case TileUpType.Door:
                return;
            default:
                break;
        }

        switch (tempTile.type)
        {
            case TileType.Rock:
                //rock solid !
                GetComponent<FlameManager>().SubstractFlame(true, 1);
                break;

            case TileType.Ice:
                //zooppppppp ice
                MoveNextTile(finger);
                _currentDelayLerpMove += _delayLerpMove;
                _tileMoving = TileType.Ice;
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
                for (int i = 0; i < tempTile.pushNumberTiles; i++)
                {
                    MoveNextTile(tempTile.direction);
                    _currentDelayLerpMove += _delayLerpMove;
                    GetComponent<FlameManager>().SubstractFlame(true, 1);
                };
                _tileMoving = TileType.Wind;
                break;

            default:
                break;
        }
    }

    private void GoBack (Etouch.Finger finger)
    {
        Vector2 fingerTouchDelta = finger.screenPosition - _startPositionFinger;

        //trouve le vector d'ajout de position selon la direction du slide
        _goToPosition -= VectorAddMovePos(fingerTouchDelta);

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

    private void MoveNextTile (Etouch.Finger finger)
    {
        Vector2 fingerTouchDelta = finger.screenPosition - _startPositionFinger;

        //trouve le vector d'ajout de position selon la direction du slide
        _goToPosition += VectorAddMovePos(fingerTouchDelta);

        //trouve le millieu de la tile ou l'on atterie
        _goToPosition = manager.tileMap.FindTileWithPos(_goToPosition).transform.position;

        //verifie si la tile ou l'on va bouger contiens un effet, si oui applique l'effet
        CheckNextTileEffect(finger);
    }

    private void MoveNextTile(TileDown.Direction direction)
    {
        //trouve le vector d'ajout de position selon la direction du slide
        _goToPosition += DirectionAddMovePos(direction);

        //trouve le millieu de la tile ou l'on atterie
        _goToPosition = manager.tileMap.FindTileWithPos(_goToPosition).transform.position;
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
        CheckNextTileEffect(finger);

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

            default:
                break;
        }

        return Vector3.zero;
    }

    private Vector3 VectorAddMovePos(Vector2 fingerTouchDelta)
    {
        //x
        if (Mathf.Abs(fingerTouchDelta.x) > Mathf.Abs(fingerTouchDelta.y))
        {
            //move right
            if (fingerTouchDelta.x > 0)
            {
                return new Vector3(_movementAmount, 0, 0);
            }

            //move left
            else
            {
                return new Vector3(-_movementAmount, 0, 0);
            }
        }

        //y
        else if (Mathf.Abs(fingerTouchDelta.y) > Mathf.Abs(fingerTouchDelta.x))
        {
            //move up
            if (fingerTouchDelta.y > 0)
            {
                return new Vector3(0, _movementAmount, 0);
            }
            //move down
            else
            {
                return new Vector3(0, -_movementAmount, 0);
            }
        }

        return Vector3.zero;
    }

    private void Die ()
    {
        GetComponent<FlameManager>().SubstractFlame(true, 10000);
    }

    private void Touch_onFingerDown(Etouch.Finger finger)
    {
        _startPositionFinger = finger.screenPosition;
    }

    private void Touch_onFingerMove(Etouch.Finger finger)
    {
    }
}
