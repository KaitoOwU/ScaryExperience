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

    [ShowIf("isPressurePlate")]
    public GameObject doorPressurePlate;
    private GameObject oldDoorPressurePlate;

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
    }

    public void GoBackToWhite ()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
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
            switch (type)
            {
                case MoveBubble.TileUpType.None:
                    GetComponent<SpriteRenderer>().color = sprites.colorNone;
                    break;
                case MoveBubble.TileUpType.Wall:
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
                case MoveBubble.TileUpType.Door:
                    GetComponent<SpriteRenderer>().color = sprites.colorDoor;
                    break;
                case MoveBubble.TileUpType.Lever:
                    GetComponent<SpriteRenderer>().color = sprites.colorLever;
                    break;
                case MoveBubble.TileUpType.KeyFragment:
                    GetComponent<SpriteRenderer>().color = sprites.colorKeyFragment;
                    break;
                case MoveBubble.TileUpType.KeyDoor:
                    GetComponent<SpriteRenderer>().color = sprites.colorDoorKey;
                    break;
                case MoveBubble.TileUpType.OneWayWall:
                    GetComponent<SpriteRenderer>().color = sprites.colorOneWayWall;
                    break;
                case MoveBubble.TileUpType.PortalDoor:
                    GetComponent<SpriteRenderer>().color = sprites.colorPortalDoor;
                    break;
                case MoveBubble.TileUpType.Block:
                    if (block == null)
                    {
                        GameObject temp = Instantiate(blockPrefab, transform);
                        block = temp;
                    }
                    break;
                case MoveBubble.TileUpType.PressurePlate:
                    GetComponent<SpriteRenderer>().color = sprites.colorPressurePlate;
                    break;
                case MoveBubble.TileUpType.WinBlock:
                    GetComponent<SpriteRenderer>().color = sprites.colorWin;
                    break;

                default:
                    break;
            }

            switch (oldType)
            {
                case MoveBubble.TileUpType.Lever:
                    if (door != null)
                    {
                        door.GetComponent<TileUp>().type = MoveBubble.TileUpType.None;
                        door.GetComponent<SpriteRenderer>().color = Color.white;
                        door = null;
                    }
                    break;

                case MoveBubble.TileUpType.PressurePlate:
                    if (doorPressurePlate != null)
                    {
                        doorPressurePlate.GetComponent<TileUp>().type = MoveBubble.TileUpType.None;
                        doorPressurePlate.GetComponent<SpriteRenderer>().color = Color.white;
                        doorPressurePlate = null;
                    }
                    break;

                case MoveBubble.TileUpType.PortalDoor:
                    if (otherDoor != null)
                    {
                        otherDoor.GetComponent<SpriteRenderer>().color = Color.white;
                        otherDoor.GetComponent<TileUp>().otherDoor = null;
                        otherDoor.GetComponent<TileUp>().type = MoveBubble.TileUpType.None;
                    }
                    break;
                case MoveBubble.TileUpType.Wall:
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
                GetComponent<SpriteRenderer>().color = sprites.colorNone;
                break;
            case TileUpType.Wall:
                GetComponent<SpriteRenderer>().color = sprites.colorWall;
                break;
            case TileUpType.KeyFragment:
                GetComponent<SpriteRenderer>().color = sprites.colorKeyFragment;
                break;
            case TileUpType.OneWayWall:
                GetComponent<SpriteRenderer>().color = sprites.colorOneWayWall;
                break;
            case TileUpType.Torch:
                GetComponent<SpriteRenderer>().color = sprites.colorTorch;
                break;
            case TileUpType.Brasero:
                GetComponent<SpriteRenderer>().color = sprites.colorBrasero;
                break;
            case TileUpType.WinTrappe:
                GetComponent<SpriteRenderer>().color = sprites.colorWinTrappe;
                break;
    public enum WallPosition
    {
        Side,
        Corner
    }

            case TileUpType.Block:
                if (block == null && checkBlock)
                {
                    GameObject temp = Instantiate(blockPrefab, transform);
                    block = temp;
                }
                break;
        }
    }


    private bool isWall() { return type == TileUpType.Wall; }
    private bool isOneWayWall() { return type == TileUpType.OneWayWall; }
    private bool isBrasero() { return type == TileUpType.Brasero; }
    private bool isBlock() { return type == TileUpType.Block; }
    private bool isWinTrappe() { return type == TileUpType.WinTrappe; }
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
    }


    private bool isWall() { return type == MoveBubble.TileUpType.Wall; }
    private bool isWallSide() { return wallPosition == WallPosition.Side; }
    private bool isWallCorner() { return wallPosition == WallPosition.Corner; }
    private bool isDoor() { return type == MoveBubble.TileUpType.Door; }
    private bool isLever() { return type == MoveBubble.TileUpType.Lever; }
    private bool isKeyDoor() { return type == MoveBubble.TileUpType.KeyDoor;}
    private bool isOneWayWall() { return type == MoveBubble.TileUpType.OneWayWall; }
    private bool isBrasero() { return type == MoveBubble.TileUpType.Brasero; }
    private bool isPortalDoor() { return type == MoveBubble.TileUpType.PortalDoor; }
    private bool isBlock() { return type == MoveBubble.TileUpType.Block; }
    private bool isPressurePlate() { return type == MoveBubble.TileUpType.PressurePlate; }
}
