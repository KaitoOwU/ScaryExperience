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

    [SerializeField] Color colorRock;
    [SerializeField] Color colorIce;
    [SerializeField] Color colorVoid;
    [SerializeField] Color colorWater;
    [SerializeField] Color colorWind;
    [SerializeField] Color colorBreakable;

    [HideInInspector] public bool isActivated = false;

    private void OnValidate()
    {
        switch (type)
        {
            case MoveBubble.TileType.Rock:
                GetComponent<SpriteRenderer>().color = colorRock;
                break;
            case MoveBubble.TileType.Ice:
                GetComponent<SpriteRenderer>().color = colorIce;
                break;
            case MoveBubble.TileType.Void:
                GetComponent<SpriteRenderer>().color = colorVoid;
                break;
            case MoveBubble.TileType.Water:
                GetComponent<SpriteRenderer>().color = colorWater;
                break;
            case MoveBubble.TileType.Wind:
                GetComponent<SpriteRenderer>().color = colorWind;
                break;
            case MoveBubble.TileType.Breakable:
                GetComponent<SpriteRenderer>().color = colorBreakable;
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
