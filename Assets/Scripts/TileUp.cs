using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileUp : Tile
{
    [Header("- Stats -")]
    public MoveBubble.TileUpType type;

    private bool isWall() { return type == MoveBubble.TileUpType.Wall; }
    private bool isDoor() { return type == MoveBubble.TileUpType.Door; }
}
