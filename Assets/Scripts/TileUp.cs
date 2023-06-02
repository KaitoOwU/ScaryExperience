using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Rendering.Universal;

public class TileUp : Tile
{
    [Header("- Stats -")]
    public TileUpType type;
    private TileUpType oldType;

    [ShowIf("isWinTrappe")]
    public int numberPartKeyRequired = 1;

    [ShowIf("isBrasero")]
    public int refillAmount = 10;

    [ShowIf("isOneWayWall")]
    public TileDown.Direction directionToGoThrough;

    [ShowIf("isBlock")]
    public GameObject block;

    [ShowIf("isWall")]
    public WallPosition wallPosition;

    [ShowIf("isWallSide")]
    public WallSideOrientation _wallSideOrientation;

    [ShowIf("isWallCorner")]
    public WallCornerOrientation _wallCornerOrientation;

    [Header("- ToHook -")]
    [SerializeField] GameObject blockPrefab;
    [SerializeField] SpriteUp sprites;

    //public hide
    [HideInInspector] public bool isActivated = false;

    public enum TileUpType
    {
        None,
        Wall,
        KeyFragment,
        OneWayWall,
        Brasero,
        Torch,
        Block,
        WinTrappe,
        Collectible
    }

    public void GoBackToWhite ()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<SpriteRenderer>().sprite = sprites.spriteNone[0];
    }

    // change la door lorqu'on la met dans l'inspecteur
    private void OnValidate()
    {
        // si l'on ne d�signe plus la case comme �tant block, on delete le block (object)
        if (type != TileUpType.Block && block != null)
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                DestroyImmediate(block);
            };
        }

        if (oldType != type)
        {
            RefreshColorSprite(true);

            switch (oldType)
            {
                case TileUpType.Wall:
                    DestroyImmediate(GetComponent<ShadowCaster2D>());
                    break;
                default:
                    break;
            }
        }

        oldType = type;
    }

    public void RefreshColorSprite(bool checkBlock)
    {
        switch (type)
        {
            case TileUpType.None:
                GetComponent<SpriteRenderer>().sprite = sprites.spriteNone[0];
                
                break;
            case TileUpType.Wall:
                GetComponent<SpriteRenderer>().color = Color.white;
                switch (wallPosition)
                {
                    case WallPosition.Side:
                        switch (_wallSideOrientation)
                        {
                            case WallSideOrientation.UpOneSide:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteSideWall[Random.Range(0, 2)];
                                break;
                            case WallSideOrientation.DownOneSide:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteSideWall[2];
                                break;
                            case WallSideOrientation.LeftOneSide:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteSideWall[Random.Range(3, 5)];
                                break;
                            case WallSideOrientation.RightOneSide:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteSideWall[Random.Range(5, 7)];
                                break;
                            case WallSideOrientation.VerticalTwoSides:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteSideWall[Random.Range(7, 9)];
                                break;
                            case WallSideOrientation.HorizontalTwoSides:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteSideWall[9];
                                break;
                            case WallSideOrientation.EndHorizontalTwoSidesR:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteSideWall[10];
                                break;
                            case WallSideOrientation.EndHorizontalTwoSidesL:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteSideWall[11];
                                break;
                            case WallSideOrientation.EndVerticalTwoSidesU:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteSideWall[12];
                                break;
                            case WallSideOrientation.EndVerticalTwoSidesD:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteSideWall[13];
                                break;
                            case WallSideOrientation.SoloSide:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteSideWall[14];
                                break;
                        }
                        break;
                    case WallPosition.Corner:
                        switch (_wallCornerOrientation)
                        {
                            case WallCornerOrientation.LeftDownExterior:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteCornerWall[0];
                                break;
                            case WallCornerOrientation.LeftUpExterior:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteCornerWall[1];
                                break;
                            case WallCornerOrientation.RightDownExterior:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteCornerWall[2];
                                break;
                            case WallCornerOrientation.RightUpExterior:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteCornerWall[3];
                                break;
                            case WallCornerOrientation.LeftDownInterior:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteCornerWall[4];
                                break;
                            case WallCornerOrientation.LeftUpInterior:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteCornerWall[5];
                                break;
                            case WallCornerOrientation.RightDownInterior:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteCornerWall[6];
                                break;
                            case WallCornerOrientation.RightUpInterior:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteCornerWall[7];
                                break;
                        }
                        break;
                }
                gameObject.AddComponent<ShadowCaster2D>();
                break;

            case TileUpType.KeyFragment:
                GetComponent<SpriteRenderer>().sprite = sprites.spriteKeyFragment[0];
                break;
            case TileUpType.OneWayWall:
                GetComponent<SpriteRenderer>().sprite = sprites.spriteOneWayWall[0];
                break;
            case TileUpType.Torch:
                GetComponent<SpriteRenderer>().sprite = sprites.spriteTorch[0];
                break;
            case TileUpType.Brasero:
                GetComponent<SpriteRenderer>().sprite = sprites.spriteBrasero[0];
                break;
            case TileUpType.WinTrappe:
                GetComponent<SpriteRenderer>().sprite = sprites.spriteWinTrappe[0];
                break;

            case TileUpType.Block:
                if (block == null && checkBlock)
                {
                    GameObject temp = Instantiate(blockPrefab, transform);
                    block = temp;
                }
                break;
        }
    }

    public enum WallPosition
    {
        None,
        Side,
        Corner
    }

    private bool isWall() { return type == TileUpType.Wall; }
    private bool isOneWayWall() { return type == TileUpType.OneWayWall; }
    private bool isBrasero() { return type == TileUpType.Brasero; }
    private bool isBlock() { return type == TileUpType.Block; }
    private bool isWinTrappe() { return type == TileUpType.WinTrappe; }
    private bool isWallSide() { return wallPosition == WallPosition.Side; }
    private bool isWallCorner() { return wallPosition == WallPosition.Corner; }

    public enum WallCornerOrientation
    {
        LeftUpExterior,
        RightUpExterior,
        LeftDownExterior,
        RightDownExterior,
        LeftUpInterior,
        RightUpInterior,
        LeftDownInterior,
        RightDownInterior
    }

    public enum WallSideOrientation
    {
        LeftOneSide,
        RightOneSide,
        DownOneSide,
        UpOneSide,
        HorizontalTwoSides,
        VerticalTwoSides,
        EndHorizontalTwoSidesL,
        EndHorizontalTwoSidesR,
        EndVerticalTwoSidesU,
        EndVerticalTwoSidesD,
        SoloSide
    }


}
