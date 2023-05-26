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

    [SerializeField] Color colorNone;
    [SerializeField] Color colorWall;
    [SerializeField] Color colorDoor;
    [SerializeField] Color colorLever;

    [HideInInspector] public bool isActivated = false;


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
            door.GetComponent<SpriteRenderer>().color = new Color32(0xF9, 0x00, 0xFF, 0xFF);
            oldDoor = door;
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
            default:
                break;
        }
    }


    private bool isWall() { return type == MoveBubble.TileUpType.Wall; }
    private bool isDoor() { return type == MoveBubble.TileUpType.Door; }
    private bool isLever() { return type == MoveBubble.TileUpType.Lever; }
}
