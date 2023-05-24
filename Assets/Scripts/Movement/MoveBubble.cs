using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Etouch = UnityEngine.InputSystem.EnhancedTouch;

public class MoveBubble : MonoBehaviour
{
    [SerializeField] float _movementAmount;
    Vector3 _movementDirection = new Vector3();
    bool _isMoving = false;

    Vector3 _goToPosition;
    Vector2 _startPositionFinger;


    [SerializeField] float _delayLerpMove;
    [SerializeField] AnimationCurve _curveLerp;

    private void OnEnable()
    {
        Etouch.EnhancedTouchSupport.Enable();
    }
    private void OnDisable()
    {
        Etouch.EnhancedTouchSupport.Disable();
    }

    private void Awake()
    {
        Etouch.EnhancedTouchSupport.Enable();

        Etouch.Touch.onFingerMove += Touch_onFingerMove;
        Etouch.Touch.onFingerDown += Touch_onFingerDown;
        Etouch.Touch.onFingerUp += Touch_onFingerUp;

        _goToPosition = transform.position;
    }


    private IEnumerator MoveToPosition ()
    {
        float moveTimer = 0;
        Vector3 startPos = transform.position;

        while (moveTimer < _delayLerpMove)
        {
            moveTimer += Time.fixedDeltaTime;

            //Debug.Log("prout : " + moveTimer / _delayLerpMove);
            //Debug.Log(transform.position);
            //Debug.Log(_goToPosition);

            transform.position = Vector3.Lerp(startPos, _goToPosition, _curveLerp.Evaluate(moveTimer / _delayLerpMove));

            yield return new WaitForFixedUpdate();
        }

        _isMoving = false;
    }

    private void Touch_onFingerUp(Etouch.Finger finger) 
    {
        Vector2 fingerTouchDelta = finger.screenPosition - _startPositionFinger;
        //Debug.Log("delta : " + fingerTouchDelta);


        //x
        if (Mathf.Abs(fingerTouchDelta.x) > Mathf.Abs(fingerTouchDelta.y))
        {
            //move right
            if (fingerTouchDelta.x > 0)
            {
                _goToPosition += new Vector3(_movementAmount, 0, 0);
            }

            //move left
            else
            {
                _goToPosition += new Vector3(-_movementAmount, 0, 0);
            }
        }

        //y
        else if (Mathf.Abs(fingerTouchDelta.y) > Mathf.Abs(fingerTouchDelta.x))
        {
            //move up
            if (fingerTouchDelta.y > 0)
            {
                _goToPosition += new Vector3(0, _movementAmount, 0);
            }
            //move down
            else
            {
                _goToPosition += new Vector3(0, -_movementAmount, 0);
            }
        }


        StartCoroutine(MoveToPosition());
        _isMoving = true;

        Debug.Log("up");
    }

    private void Touch_onFingerDown(Etouch.Finger finger)
    {
        _startPositionFinger = finger.screenPosition;
    }

    private void Touch_onFingerMove(Etouch.Finger finger)
    {
        Debug.Log("move");
    }
}
