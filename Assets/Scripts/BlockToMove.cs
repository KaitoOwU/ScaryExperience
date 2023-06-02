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
    [SerializeField] Sprite waterRock;

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

    private void RecursiveCheckNextWind (Vector3 pos, TileDown.Direction direction, bool isStopped, Sprite spriteReplace)
    {
        TileDown tempTileDown = _bubble.manager.tileMap.FindTileWithPos(pos);
        pos += _bubble.DirectionAddMovePos(direction);

        if (tempTileDown.type == TileDown.TileType.Wind && tempTileDown.direction == direction)
        {
            tempTileDown.isActivated = isStopped;
            tempTileDown.GetComponent<SpriteRenderer>().sprite = spriteReplace;
            RecursiveCheckNextWind(pos, direction, isStopped, spriteReplace);
        }
        else
        {
            return;
        }
    }

    //private void ActivateWindStop (Vector3 startPos ,TileDown.Direction direction, bool isStopped, Sprite spriteReplace)
    //{
    //    TileDown tempTileDown = _bubble.manager.tileMap.FindTileWithPos(startPos);
    //    Vector3 nextTilePos = startPos;

    //    for (int i = 0; i < tempTileDown.pushNumberTiles +1; i++)
    //    {
    //        TileDown tempTileNext = _bubble.manager.tileMap.FindTileWithPos(nextTilePos);

    //        if (tempTileNext.type == TileDown.TileType.Wind)
    //        {
    //            tempTileNext.isActivated = isStopped;
    //            tempTileNext.GetComponent<SpriteRenderer>().sprite = spriteReplace;
    //        }
    //        else
    //        {
    //            return;
    //        }

    //        nextTilePos += _bubble.DirectionAddMovePos(direction);
    //    }
    //}

    private void CheckNextTileEffect(TileDown.Direction direction)
    {
        SwitchOnTileDown(direction);
    }

    private void SwitchOnTileDown(TileDown.Direction direction)
    {
        TileDown tempTileActual = _bubble.manager.tileMap.FindTileWithPos(transform.position);

        TileDown tempTile = _bubble.manager.tileMap.FindTileWithPos(toGoPosBlock);
        TileUp tempTileUp = _bubble.manager.tileUpMap.FindTileWithPos(toGoPosBlock);

        if (tempTileActual.type == TileDown.TileType.Wind)
        {
            RecursiveCheckNextWind(tempTileActual.transform.position , tempTileActual.direction, false, tempTileActual.sprites.spriteWind[0]);
        }

        switch (tempTile.type)
        {
            case TileDown.TileType.Ice:
                //zooppppppp ice
                MoveNextTile(direction, _bubble);
                break;

            case TileDown.TileType.Void:
                GoBack(direction);
                break;

            case TileDown.TileType.Water:
                tempTile.type = TileDown.TileType.WaterRock;
                tempTile.GetComponent<SpriteRenderer>().sprite = waterRock;
                Destroy(gameObject, 0.4f);
                tempTileUp.type = TileUp.TileUpType.None;
                break;

            case TileDown.TileType.Wind:
                RecursiveCheckNextWind(toGoPosBlock ,tempTile.direction, true, tempTileActual.sprites.spriteRock[0]);
                break;

            default:
                break;
        }
    }
}
