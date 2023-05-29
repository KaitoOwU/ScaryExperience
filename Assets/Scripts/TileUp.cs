using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TileUp : Tile
{
    [Header("- Stats -")]
    public MoveBubble.TileUpType type;

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

    [SerializeField] Color colorNone;
    [SerializeField] Color colorWall;
    [SerializeField] Color colorDoor;
    [SerializeField] Color colorLever;
    [SerializeField] Color colorKeyFragment;
    [SerializeField] Color colorDoorKey;
    [SerializeField] Color colorOneWayWall;
    [SerializeField] Color colorBrasero;
    [SerializeField] Color colorPortalDoor;

    [HideInInspector] public bool isActivated = false;

    public void GoBackToWhite ()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    // change la door lorqu'on la met dans l'inspecteur
    private void OnValidate()
    {
        if (door != null && type == MoveBubble.TileUpType.Lever)
        {
            if (oldDoor != null)
            {
                oldDoor.GetComponent<TileUp>().type = MoveBubble.TileUpType.None;
                oldDoor.GetComponent<SpriteRenderer>().color = Color.white;
            }

            door.GetComponent<TileUp>().type = MoveBubble.TileUpType.Door;
            door.GetComponent<SpriteRenderer>().color = colorDoor;
            oldDoor = door;
        }

        if (door != null && type != MoveBubble.TileUpType.Lever)
        {
            door.GetComponent<TileUp>().type = MoveBubble.TileUpType.None;
            door.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (otherDoor != null && type != MoveBubble.TileUpType.PortalDoor)
        {
            otherDoor.GetComponent<SpriteRenderer>().color = Color.white;
            otherDoor.GetComponent<TileUp>().otherDoor = null;
            otherDoor.GetComponent<TileUp>().type = MoveBubble.TileUpType.None;
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
            otherDoor.GetComponent<SpriteRenderer>().color = colorPortalDoor;
            oldOtherDoor = otherDoor;
        }

        switch (type)
        {
            case MoveBubble.TileUpType.None:
                GetComponent<SpriteRenderer>().color = colorNone;
                break;
            case MoveBubble.TileUpType.Wall:
                GetComponent<SpriteRenderer>().color = colorWall;
                break;
            case MoveBubble.TileUpType.Door:
                GetComponent<SpriteRenderer>().color = colorDoor;
                break;
            case MoveBubble.TileUpType.Lever:
                GetComponent<SpriteRenderer>().color = colorLever;
                break;
            case MoveBubble.TileUpType.KeyFragment:
                GetComponent<SpriteRenderer>().color = colorKeyFragment;
                break;
            case MoveBubble.TileUpType.KeyDoor:
                GetComponent<SpriteRenderer>().color = colorDoorKey;
                break;
            case MoveBubble.TileUpType.OneWayWall:
                GetComponent<SpriteRenderer>().color = colorOneWayWall;
                break;
            case MoveBubble.TileUpType.PortalDoor:
                GetComponent<SpriteRenderer>().color = colorPortalDoor;
                break;

            default:
                break;
        }
    }


    private bool isWall() { return type == MoveBubble.TileUpType.Wall; }
    private bool isDoor() { return type == MoveBubble.TileUpType.Door; }
    private bool isLever() { return type == MoveBubble.TileUpType.Lever; }
    private bool isKeyDoor() { return type == MoveBubble.TileUpType.KeyDoor;}
    private bool isOneWayWall() { return type == MoveBubble.TileUpType.OneWayWall; }
    private bool isBrasero() { return type == MoveBubble.TileUpType.Brasero; }
    private bool isPortalDoor() { return type == MoveBubble.TileUpType.PortalDoor; }
}
