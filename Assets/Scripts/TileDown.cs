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

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }


    private bool isWind() { return type == MoveBubble.TileType.Wind; }
}
