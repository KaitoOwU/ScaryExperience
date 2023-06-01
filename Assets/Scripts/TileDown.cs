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

    [ShowIf("isRock")]
    public Position position;
    [ShowIf("isSide")]
    public SideOrientation orientationSide;
    [ShowIf("isCorner")]
    public CornerOrientation orientationCorner;


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
            case MoveBubble.TileType.Rock:
                
                switch (position)
                {
                    case Position.Normal:
                        GetComponent<SpriteRenderer>().sprite = sprites.spriteRock[Random.Range(0, sprites.spriteRock.Count)];
                        break;
                    case Position.Side:
                        switch (orientationSide)
                        {
                            case SideOrientation.Left:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteSideRock[Random.Range(2, 4)];
                                break;
                            case SideOrientation.Right:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteSideRock[Random.Range(4, 6)];
                                break;
                            case SideOrientation.Up:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteSideRock[Random.Range(6, 8)];
                                break;
                            case SideOrientation.Down:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteSideRock[Random.Range(0, 2)];
                                break;
                        }
                        break;
                    case Position.Corner:
                        switch (orientationCorner)
                        {
                            case CornerOrientation.LeftUp:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteCornerRock[2];
                                break;
                            case CornerOrientation.LeftDown:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteCornerRock[0];
                                break;
                            case CornerOrientation.RightUp:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteCornerRock[3];
                                break;
                            case CornerOrientation.RightDown:
                                GetComponent<SpriteRenderer>().sprite = sprites.spriteCornerRock[1];
                                break;
                        }
                        break;
                }
                break;
            case MoveBubble.TileType.Ice:
                GetComponent<SpriteRenderer>().color = sprites.colorIce;
                break;
            case MoveBubble.TileType.Void:
                GetComponent<SpriteRenderer>().color = sprites.colorVoid;
                break;
            case MoveBubble.TileType.Water:
                GetComponent<SpriteRenderer>().sprite = sprites.spriteWater[0];
                break;
            case MoveBubble.TileType.Wind:
                GetComponent<SpriteRenderer>().color = sprites.colorWind;
                break;
            case MoveBubble.TileType.Breakable:
                GetComponent<SpriteRenderer>().color = sprites.colorBreakable;
                break;
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

    public enum Position
    {
        Normal,
        Side,
        Corner
    }

    public enum SideOrientation
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum CornerOrientation
    {
        LeftUp,
        RightUp,
        LeftDown,
        RightDown
    }


    private bool isWind() { return type == TileType.Wind; }
    private bool isRock() { return type == TileType.Rock; }
    private bool isSide() { return position == Position.Side; }
    private bool isCorner() { return position == Position.Corner; }

}
