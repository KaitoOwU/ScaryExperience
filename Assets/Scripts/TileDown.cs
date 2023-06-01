using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TileDown : Tile
{
    [Header("- Stats -")]
    public TileType type;
    private TileType oldType;

    [ShowIf("isWind")]
    public Direction direction;
    [ShowIf("isWind")]
    public int pushNumberTiles;

    [SerializeField] SpriteDown sprites;

    [HideInInspector] public bool isActivated = false;

    public enum TileType
    {
        Rock,
        Ice,
        Void,
        Water,
        Wind,
        Breakable
    }

    private void OnValidate()
    {
        RefreshColorSprite();
    }

    public void RefreshColorSprite()
    {
        if (oldType != type)
        {
            switch (type)
            {
                case TileType.Rock:
                    GetComponent<SpriteRenderer>().color = sprites.colorRock;
                    break;
                case TileType.Ice:
                    GetComponent<SpriteRenderer>().color = sprites.colorIce;
                    break;
                case TileType.Void:
                    GetComponent<SpriteRenderer>().color = sprites.colorVoid;
                    break;
                case TileType.Water:
                    GetComponent<SpriteRenderer>().color = sprites.colorWater;
                    break;
                case TileType.Wind:
                    GetComponent<SpriteRenderer>().color = sprites.colorWind;
                    break;

                case TileType.Breakable:
                    GetComponent<SpriteRenderer>().color = sprites.colorBreakable;
                    break;
            }
        }

        oldType = type;
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }


    private bool isWind() { return type == TileType.Wind; }
}
