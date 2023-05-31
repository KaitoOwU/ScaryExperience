using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TileDown : Tile
{
    [Header("- Stats -")]
    public MoveBubble.TileType type;

    [ShowIf("isWind")]
    public Direction direction;
    [ShowIf("isWind")]
    public int pushNumberTiles;

    [SerializeField] SpriteDown sprites;

    [HideInInspector] public bool isActivated = false;

    private void OnValidate()
    {
        switch (type)
        {
            case MoveBubble.TileType.Rock:
                GetComponent<SpriteRenderer>().color = sprites.colorRock;
                break;
            case MoveBubble.TileType.Ice:
                GetComponent<SpriteRenderer>().color = sprites.colorIce;
                break;
            case MoveBubble.TileType.Void:
                GetComponent<SpriteRenderer>().color = sprites.colorVoid;
                break;
            case MoveBubble.TileType.Water:
                GetComponent<SpriteRenderer>().color = sprites.colorWater;
                break;
            case MoveBubble.TileType.Wind:
                GetComponent<SpriteRenderer>().color = sprites.colorWind;
                break;
            case MoveBubble.TileType.Breakable:
                GetComponent<SpriteRenderer>().color = sprites.colorBreakable;
                break;
        }
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }


    private bool isWind() { return type == MoveBubble.TileType.Wind; }
}
