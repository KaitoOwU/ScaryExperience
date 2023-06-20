using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockToMove : MonoBehaviour
{
    Vector3 _startPos;
    float _moveTimer;
    [SerializeField] AnimationCurve _currentAnimCurve;
    [HideInInspector] public Vector3 toGoPosBlock;
    [HideInInspector] public Vector3 oldToGo;
    [HideInInspector] public TileUp lastTileUp;

    private bool isInWater;
    private MoveBubble _bubble;
    public SpriteDown spritesDown;

    AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public IEnumerator PushBlockCorout(MoveBubble playerMovement)
    {
        _startPos = transform.position;
        _moveTimer = 0;

        while (_moveTimer < 0.2f)
        {
            _moveTimer += Time.fixedDeltaTime;

            transform.position = Vector3.Lerp(_startPos, toGoPosBlock, _currentAnimCurve.Evaluate(_moveTimer / 0.2f));

            yield return new WaitForFixedUpdate();
        }
    }

    private void Start()
    {
        toGoPosBlock = transform.position;
    }

    public bool StartMoving (TileDown.Direction direction, MoveBubble bubble)
    {
        oldToGo = toGoPosBlock;
        MoveNextTile(direction, bubble);

        //joue le son que lorsque le block a bouger
        if (oldToGo != toGoPosBlock)
        {
            _audioManager.PlaySFX(_audioManager.rockSound[Random.Range(0, _audioManager.rockSound.Count)]);
        }

        StartCoroutine(PushBlockCorout(bubble));

        //check if blocked
        return oldToGo == toGoPosBlock;
    }

    private void GoBack(TileDown.Direction direction)
    {
        //trouve le vector d'ajout de position selon la direction du slide
        toGoPosBlock -= _bubble.DirectionAddMovePos(direction);

        //trouve le millieu de la tile ou l'on atterie
        toGoPosBlock = GameManager.Instance.tileMap.FindTileWithPos(toGoPosBlock).transform.position;
    }

    public void MoveNextTile(TileDown.Direction direction, MoveBubble bubble)
    {
        if (_bubble == null)
        {
            _bubble = bubble;
            lastTileUp = GameManager.Instance.tileUpMap.FindTileWithPos(toGoPosBlock);
        }

        //trouve le vector d'ajout de position selon la direction du slide
        toGoPosBlock += bubble.DirectionAddMovePos(direction);
        TileUp tileUpToMove = GameManager.Instance.tileUpMap.FindTileWithPos(toGoPosBlock);
        TileUp tileBefore = GameManager.Instance.tileUpMap.FindTileWithPos(lastTileUp.transform.position - bubble.DirectionAddMovePos(lastTileUp.direction));
        TileDown tileDownToMove = GameManager.Instance.tileMap.FindTileWithPos(toGoPosBlock);

        Vector3 currentGoPos = toGoPosBlock - bubble.DirectionAddMovePos(direction);
        TileDown currentTileDown = GameManager.Instance.tileMap.FindTileWithPos(currentGoPos);

        // si la case n'est pas libre on ne push pas le block
        if (tileUpToMove.type != TileUp.TileUpType.None && tileUpToMove.type != TileUp.TileUpType.Wind && tileDownToMove.type != TileDown.TileType.WaterRock)
        {
            GoBack(direction);
            TileUp tileUpEnd = GameManager.Instance.tileUpMap.FindTileWithPos(toGoPosBlock);

            //si la case d'arriver c'est du vent alors on applique l'effet
            if (tileUpEnd.type == TileUp.TileUpType.Wind)
            {
                RecursiveCheckNextWind(tileUpEnd.transform.position, tileUpEnd.direction, true, tileUpEnd.spritesUp.spriteNone[0]);
            }

            tileUpEnd.block = gameObject;
            tileUpEnd.type = TileUp.TileUpType.Block;
            return;
        }

        // si on retire le block d'une case ou il y avait du vent avec le meme block qui la arreter
        else if (lastTileUp != null && lastTileUp.wasWind && ((tileBefore.type == TileUp.TileUpType.Wind && lastTileUp.direction == tileBefore.direction) || (tileBefore.type == TileUp.TileUpType.Ventilateur && lastTileUp.direction == tileBefore.dirWind)) && !tileBefore.isActivated)
        {
            if (direction == lastTileUp.direction)
            {
                tileUpToMove.wasWind = true;
                tileUpToMove.direction = direction;
            }

            lastTileUp.block = null;
            lastTileUp.type = TileUp.TileUpType.Wind;
            RecursiveCheckNextWind(lastTileUp.transform.position, lastTileUp.direction, false, lastTileUp.spritesUp.spriteWind[0]);
        }

        // ce n'est pas le meme block qui a blocker le vent alors on remet vent mais arreter
        else if (lastTileUp != null && lastTileUp.isActivated && tileBefore.isActivated)
        {
            lastTileUp.type = TileUp.TileUpType.Wind;
            lastTileUp.isActivated = true;
            lastTileUp.PutOrWithdrawShaderWind(true);
            lastTileUp.GetComponent<SpriteRenderer>().sprite = lastTileUp.spritesUp.spriteNone[0];
        }

        else if (lastTileUp != null)
        {
            lastTileUp.block = null;
            lastTileUp.type = TileUp.TileUpType.None;
        }

        //si la direction dans laquelle on pousse est la m�me que la direction du vent
        if (lastTileUp != null && direction == lastTileUp.direction && lastTileUp.wasWind && tileBefore.type == TileUp.TileUpType.Wind && tileBefore.isActivated == false)
        {
            lastTileUp.block = null;
            lastTileUp.type = TileUp.TileUpType.Wind;
            tileUpToMove.wasWind = true;
            RecursiveCheckNextWind(lastTileUp.transform.position, lastTileUp.direction, false, lastTileUp.spritesUp.spriteWind[0]);
        }

        //trouve le millieu de la tile ou l'on atterie
        toGoPosBlock = tileUpToMove.transform.position;
        lastTileUp = tileUpToMove;

        //verifie si la tile ou l'on va bouger contiens un effet, si oui applique l'effet
        CheckNextTileEffect(direction);

        // si tomber dans l'eau ou le void on ne met pas le block sur la prochaine case
        // sinon on met le block sur la prochaine case
        if (!isInWater && tileDownToMove.type != TileDown.TileType.Water && tileDownToMove.type != TileDown.TileType.Void && tileDownToMove.type != TileDown.TileType.Ice)
        {
            tileUpToMove.block = gameObject;
            tileUpToMove.type = TileUp.TileUpType.Block;
            tileUpToMove.block.transform.parent = tileUpToMove.transform;
            tileUpToMove.block.transform.rotation = Quaternion.identity;
        }

        return;
    }

    private void RecursiveCheckNextWind (Vector3 pos, TileDown.Direction direction, bool isStopped, Sprite spriteReplace)
    {
        TileUp tempTileUp = GameManager.Instance.tileUpMap.FindTileWithPos(pos);
        pos += _bubble.DirectionAddMovePos(direction);

        if (tempTileUp != null && (tempTileUp.type == TileUp.TileUpType.Wind || tempTileUp.type == TileUp.TileUpType.None))
        {
            tempTileUp.isActivated = isStopped;

            if (!isStopped)
            {
                lastTileUp.RotateDirectionWind(lastTileUp, direction);
            }

            tempTileUp.PutOrWithdrawShaderWind(isStopped);
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
        TileDown tempTile = GameManager.Instance.tileMap.FindTileWithPos(toGoPosBlock);
        TileUp tempTileUp = GameManager.Instance.tileUpMap.FindTileWithPos(toGoPosBlock);

        switch (tempTile.type)
        {
            case TileDown.TileType.Ice:
                //zooppppppp ice
                MoveNextTile(direction, _bubble);
                break;

            case TileDown.TileType.Void:
                Destroy(gameObject, 1.3f);
                tempTileUp.type = TileUp.TileUpType.None;
                transform.DOScale(0.72f, 0.2f).OnComplete(() => transform.DOScale(0, 1f));
                break;

            case TileDown.TileType.Water:
                tempTileUp.type = TileUp.TileUpType.None;
                _audioManager.PlaySFX(_audioManager.waterSound);
                tempTile.type = TileDown.TileType.WaterRock;
                Debug.LogWarning(tempTileUp.gameObject);
                int idTemp = tempTile.GetComponent<TileDown>().idWater;

                tempTile.GetComponent<SpriteRenderer>().sharedMaterial = spritesDown.waterMat;
                tempTile.GetComponent<SpriteRenderer>().sprite = spritesDown.spriteWater[idTemp];
                tempTile.GetComponent<SpriteRenderer>().material.SetTexture("_TextureOut", spritesDown.spriteOutWaterBlock[idTemp].texture);
                tempTile.GetComponent<SpriteRenderer>().material.SetTexture("_TextureIn", spritesDown.spriteInWaterBlock[idTemp].texture);

                Destroy(gameObject, 0.4f);
                tempTileUp.type = TileUp.TileUpType.None;
                isInWater = true;
                break;

            default:
                break;
        }

        switch (tempTileUp.type)
        {
            case TileUp.TileUpType.Wind:
                if (!tempTileUp.isActivated)
                {
                    RecursiveCheckNextWind(toGoPosBlock, tempTileUp.direction, true, tempTileUp.spritesUp.spriteNone[0]);
                }

                tempTileUp.wasWind = true;
                break;

            default:
                //tempTileUp.wasWind = false;
                tempTileUp.blockerBlock = 0;
                break;
        }
    }
}
