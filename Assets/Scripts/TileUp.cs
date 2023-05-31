using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TileUp : Tile
{
    [Header("- Stats -")]
    public MoveBubble.TileUpType type;
    private MoveBubble.TileUpType oldType;

    [ShowIf("isLever")]
    public GameObject door;
    private GameObject oldDoor;

    [ShowIf("isKeyDoor")]
    public int numberKeyRequired = 1;

    [ShowIf("isBrasero")]
    public int refillAmount = 10;

    [ShowIf("isOneWayWall")]
    public TileDown.Direction directionToGoThrough;

    [ShowIf("isPortalDoor")]
    public GameObject otherDoor;
    private GameObject oldOtherDoor;

    [ShowIf("isBlock")]
    public GameObject block;

    [ShowIf("isPressurePlate")]
    public GameObject doorPressurePlate;
    private GameObject oldDoorPressurePlate;

    [Header("- ToHook -")]
    [SerializeField] GameObject blockPrefab;
    [SerializeField] SpriteUp sprites;

    [HideInInspector] public bool isActivated = false;

    public void GoBackToWhite ()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    // change la door lorqu'on la met dans l'inspecteur
    private void OnValidate()
    {
        // si l'on ne désigne plus la case comme étant block, on delete le block (object)
        if (type != MoveBubble.TileUpType.Block && block != null)
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                DestroyImmediate(block);
            };
        }

        if (doorPressurePlate != null && type == MoveBubble.TileUpType.PressurePlate)
        {
            if (oldDoorPressurePlate != null)
            {
                oldDoorPressurePlate.GetComponent<TileUp>().type = MoveBubble.TileUpType.None;
                oldDoorPressurePlate.GetComponent<SpriteRenderer>().color = Color.white;
            }

            doorPressurePlate.GetComponent<TileUp>().type = MoveBubble.TileUpType.Door;
            doorPressurePlate.GetComponent<SpriteRenderer>().color = sprites.colorDoor;
            oldDoorPressurePlate = doorPressurePlate;
        }


        if (door != null && type == MoveBubble.TileUpType.Lever)
        {
            if (oldDoor != null)
            {
                oldDoor.GetComponent<TileUp>().type = MoveBubble.TileUpType.None;
                oldDoor.GetComponent<SpriteRenderer>().color = Color.white;
            }

            door.GetComponent<TileUp>().type = MoveBubble.TileUpType.Door;
            door.GetComponent<SpriteRenderer>().color = sprites.colorDoor;
            oldDoor = door;
        }

        if (otherDoor != null && type == MoveBubble.TileUpType.PortalDoor)
        {
            if (oldOtherDoor != null)
            {
                oldOtherDoor.GetComponent<TileUp>().type = MoveBubble.TileUpType.None;
                oldOtherDoor.GetComponent<SpriteRenderer>().color = Color.white;
            }

            otherDoor.GetComponent<TileUp>().type = MoveBubble.TileUpType.PortalDoor;
            otherDoor.GetComponent<TileUp>().otherDoor = gameObject;
            otherDoor.GetComponent<SpriteRenderer>().color = sprites.colorPortalDoor;
            oldOtherDoor = otherDoor;
        }

        if (oldType != type)
        {
            switch (type)
            {
                case MoveBubble.TileUpType.None:
                    GetComponent<SpriteRenderer>().color = sprites.colorNone;
                    break;
                case MoveBubble.TileUpType.Wall:
                    GetComponent<SpriteRenderer>().color = sprites.colorWall;
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
                case MoveBubble.TileUpType.WinDoor:
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

                default:
                    break;
            }
        }

        oldType = type;
    }


    private bool isWall() { return type == MoveBubble.TileUpType.Wall; }
    private bool isDoor() { return type == MoveBubble.TileUpType.Door; }
    private bool isLever() { return type == MoveBubble.TileUpType.Lever; }
    private bool isKeyDoor() { return type == MoveBubble.TileUpType.KeyDoor;}
    private bool isOneWayWall() { return type == MoveBubble.TileUpType.OneWayWall; }
    private bool isBrasero() { return type == MoveBubble.TileUpType.Brasero; }
    private bool isPortalDoor() { return type == MoveBubble.TileUpType.PortalDoor; }
    private bool isBlock() { return type == MoveBubble.TileUpType.Block; }
    private bool isPressurePlate() { return type == MoveBubble.TileUpType.PressurePlate; }
}
