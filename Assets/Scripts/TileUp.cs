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
                    GetComponent<SpriteRenderer>().sprite = sprites.spriteWall[0];
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
}
