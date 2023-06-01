using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockToMove : MonoBehaviour
{
    Vector3 _startPos;
    float _moveTimer;
    [SerializeField] AnimationCurve _currentAnimCurve;
    [HideInInspector] public Vector3 toGoPosBlock;
    [HideInInspector] public TileUp lastTileUp;
    private MoveBubble _bubble;

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

    private void Start()
    {
        toGoPosBlock = transform.position;
    }

    public bool StartMoving (TileDown.Direction direction, MoveBubble bubble)
    {
        Vector3 oldToGo = toGoPosBlock;
        MoveNextTile(direction, bubble);
        StartCoroutine(PushBlockCorout(bubble));

        //check if blocked
        return oldToGo == toGoPosBlock;
    }

    private void GoBack(TileDown.Direction direction)
    {
        //trouve le vector d'ajout de position selon la direction du slide
        toGoPosBlock -= _bubble.DirectionAddMovePos(direction);

        //trouve le millieu de la tile ou l'on atterie
        toGoPosBlock = _bubble.manager.tileMap.FindTileWithPos(toGoPosBlock).transform.position;
    }

    public void MoveNextTile(TileDown.Direction direction, MoveBubble bubble)
    {
        if (_bubble == null)
        {
            _bubble = bubble;
            lastTileUp = _bubble.manager.tileUpMap.FindTileWithPos(toGoPosBlock);
        }

        //trouve le vector d'ajout de position selon la direction du slide
        toGoPosBlock += bubble.DirectionAddMovePos(direction);
        TileUp tileUpToMove = bubble.manager.tileUpMap.FindTileWithPos(toGoPosBlock);

        // si la case n'est pas libre on ne push pas le block
        if (tileUpToMove.type != TileUp.TileUpType.None)
        {
            GoBack(direction);
            _bubble.GoBack(direction);
            return;
        }

        if (lastTileUp != null)
        {
            lastTileUp.block = null;
            lastTileUp.type = TileUp.TileUpType.None;
        }

        tileUpToMove.block = gameObject;
        tileUpToMove.type = TileUp.TileUpType.Block;

        //trouve le millieu de la tile ou l'on atterie
        toGoPosBlock = tileUpToMove.transform.position;

        lastTileUp = tileUpToMove;
        //verifie si la tile ou l'on va bouger contiens un effet, si oui applique l'effet
        CheckNextTileEffect(direction);

        return;
    }

    private void CheckNextTileEffect(TileDown.Direction direction)
    {
        SwitchOnTileDown(direction);
    }

    private void SwitchOnTileDown(TileDown.Direction direction)
    {
        TileDown tempTile = _bubble.manager.tileMap.FindTileWithPos(toGoPosBlock);

        switch (tempTile.type)
        {
            case TileDown.TileType.Ice:
                //zooppppppp ice
                MoveNextTile(direction, _bubble);
                break;

            case TileDown.TileType.Void:
            case TileDown.TileType.Water:
                //glou glou water
                GoBack(direction);
                break;

            default:
                break;
        }
    }
}
