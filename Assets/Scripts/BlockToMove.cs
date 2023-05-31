using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockToMove : MonoBehaviour
{
    Vector3 _startPos;
    float _moveTimer;
    [SerializeField] AnimationCurve _currentAnimCurve;
    [HideInInspector] public Vector3 toGoPosBlock;

    public IEnumerator PushBlockCorout(MoveBubble playerMovement)
    {
        _startPos = transform.position;
        _moveTimer = 0;

        while (_moveTimer < playerMovement.currentDelayLerpMove)
        {
            _moveTimer += Time.fixedDeltaTime;

            transform.position = Vector3.Lerp(_startPos, toGoPosBlock, _currentAnimCurve.Evaluate(_moveTimer / playerMovement.currentDelayLerpMove));

            yield return new WaitForFixedUpdate();
        }
    }
}
