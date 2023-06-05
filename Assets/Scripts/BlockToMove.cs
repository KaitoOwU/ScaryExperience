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
        TileDown tileDownToMove = _bubble.manager.tileMap.FindTileWithPos(toGoPosBlock);

        Vector3 currentGoPos = toGoPosBlock - bubble.DirectionAddMovePos(direction);
        TileDown currentTileDown = _bubble.manager.tileMap.FindTileWithPos(currentGoPos);

        // si la case n'est pas libre on ne push pas le block
        if (tileUpToMove.type != TileUp.TileUpType.None && tileUpToMove.type != TileUp.TileUpType.Wind)
        {
            GoBack(direction);
            TileUp tileUpEnd = _bubble.manager.tileUpMap.FindTileWithPos(toGoPosBlock);

            //si la case d'arriver c'est du vent alors on applique l'effet
            if (tileUpEnd.type == TileUp.TileUpType.Wind)
            {
                RecursiveCheckNextWind(tileUpEnd.transform.position, tileUpEnd.direction, true, tileUpEnd.spritesUp.spriteNone[0]);
            }

            tileUpEnd.block = gameObject;
            tileUpEnd.type = TileUp.TileUpType.Block;
            return;
        }

        else if (lastTileUp != null && lastTileUp.wasWind) 
        {
            lastTileUp.block = null;
            lastTileUp.type = TileUp.TileUpType.Wind;
            lastTileUp.isActivated = false;
            RecursiveCheckNextWind(lastTileUp.transform.position, lastTileUp.direction, false, lastTileUp.spritesUp.spriteWind[0]);
        }

        else if (lastTileUp != null && currentTileDown.type != TileDown.TileType.Ice)
        {
            lastTileUp.block = null;
            lastTileUp.type = TileUp.TileUpType.None;
        }

        //trouve le millieu de la tile ou l'on atterie
        toGoPosBlock = tileUpToMove.transform.position;

        lastTileUp = tileUpToMove;
        //verifie si la tile ou l'on va bouger contiens un effet, si oui applique l'effet
        CheckNextTileEffect(direction);

        // si tomber dans l'eau ou le void on ne met pas le block sur la prochaine case
        if (tileDownToMove.type != TileDown.TileType.WaterRock && tileDownToMove.type != TileDown.TileType.Void && tileDownToMove.type != TileDown.TileType.Ice)
        {
            tileUpToMove.block = gameObject;
            tileUpToMove.type = TileUp.TileUpType.Block;
        }

        return;
    }

    private void RecursiveCheckNextWind (Vector3 pos, TileDown.Direction direction, bool isStopped, Sprite spriteReplace)
    {
        TileUp tempTileUp = _bubble.manager.tileUpMap.FindTileWithPos(pos);
        pos += _bubble.DirectionAddMovePos(direction);

        if (tempTileUp.type == TileUp.TileUpType.Wind && tempTileUp.direction == direction)
        {
            tempTileUp.isActivated = isStopped;
            tempTileUp.GetComponent<SpriteRenderer>().sprite = spriteReplace;
            RecursiveCheckNextWind(pos, direction, isStopped, spriteReplace);
        }
        else
        {
            return;
        }
    }

    private void CheckNextTileEffect(TileDown.Direction direction)
    {
        SwitchOnTiles(direction);
    }

    private void SwitchOnTiles(TileDown.Direction direction)
    {
        TileDown tempTile = _bubble.manager.tileMap.FindTileWithPos(toGoPosBlock);
        TileUp tempTileUp = _bubble.manager.tileUpMap.FindTileWithPos(toGoPosBlock);

        switch (tempTile.type)
        {
            case TileDown.TileType.Ice:
                //zooppppppp ice
                MoveNextTile(direction, _bubble);
                break;

            case TileDown.TileType.Void:
                Destroy(gameObject, 0.4f);
                tempTileUp.type = TileUp.TileUpType.None;
                break;

            case TileDown.TileType.Water:
                tempTile.type = TileDown.TileType.WaterRock;
                tempTile.GetComponent<SpriteRenderer>().sprite = waterRock;
                Destroy(gameObject, 0.4f);
                tempTileUp.type = TileUp.TileUpType.None;
                break;

            default:
                break;
        }

        switch (tempTileUp.type)
        {
            case TileUp.TileUpType.Wind:
                RecursiveCheckNextWind(toGoPosBlock, tempTileUp.direction, true, tempTileUp.spritesUp.spriteNone[0]);
                tempTileUp.wasWind = true;
                break;

            default:
                tempTileUp.wasWind = false;
                break;
        }
    }
}
