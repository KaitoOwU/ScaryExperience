using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TileDown : Tile
{
    [Header("- Stats -")]
    public TileType type;
    private TileType oldType;

    [ShowIf("isRock")]
    public Position position;
    [ShowIf("isSide")]
    public SideOrientation orientationSide;
    [ShowIf("isCorner")]
    public CornerOrientation orientationCorner;

    public bool isActivated = false;

    public enum TileType
    {
        Rock,
        Water,
        Breakable,
        WaterRock,
        Ice,
        Void,
    }

    private void OnValidate()
    {
        if (oldType != type)
        {
            RefreshColorSprite();
        }

        oldType = type;
    }

    public void RefreshColorSprite()
    {
        switch (type)
        {
            case TileType.Rock:
                switch (position)
                {
                    case Position.Normal:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteRock[Random.Range(0, spritesDown.spriteRock.Count)];
                        break;

                    case Position.Side:
                        switch (orientationSide)
                        {
                            case SideOrientation.Left:
                                GetComponent<SpriteRenderer>().sprite = spritesDown.spriteSideRock[Random.Range(2, 4)];
                                break;
                            case SideOrientation.Right:
                                GetComponent<SpriteRenderer>().sprite = spritesDown.spriteSideRock[Random.Range(4, 6)];
                                break;
                            case SideOrientation.Up:
                                GetComponent<SpriteRenderer>().sprite = spritesDown.spriteSideRock[6];
                                break;
                            case SideOrientation.Down:
                                GetComponent<SpriteRenderer>().sprite = spritesDown.spriteSideRock[Random.Range(0, 2)];
                                break;
                        }
                        break;
                    case Position.Corner:
                        switch (orientationCorner)
                        {
                            case CornerOrientation.LeftUp:
                                GetComponent<SpriteRenderer>().sprite = spritesDown.spriteCornerRock[2];
                                break;
                            case CornerOrientation.LeftDown:
                                GetComponent<SpriteRenderer>().sprite = spritesDown.spriteCornerRock[0];
                                break;
                            case CornerOrientation.RightUp:
                                GetComponent<SpriteRenderer>().sprite = spritesDown.spriteCornerRock[3];
                                break;
                            case CornerOrientation.RightDown:
                                GetComponent<SpriteRenderer>().sprite = spritesDown.spriteCornerRock[1];
                                break;
                        }
                        break;
                }
                break;

            case TileType.Ice:
                GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[Random.Range(0, spritesDown.spriteIce.Count)];
                GetComponent<SpriteRenderer>().color = spritesDown.colorIce;
                break;

            case TileType.Void:
                GetComponent<SpriteRenderer>().sprite = spritesDown.spriteVoid[Random.Range(0, spritesDown.spriteVoid.Count)];
                GetComponent<SpriteRenderer>().color = spritesDown.colorVoid;
                break;
            case TileType.Water:
                GetComponent<SpriteRenderer>().sprite = spritesDown.spriteWater[Random.Range(0, spritesDown.spriteWater.Count)];

                break;
            case TileType.Breakable:
                GetComponent<SpriteRenderer>().sprite = spritesDown.spriteBreakable[0];
                break;
        }

        switch (oldType)
        {
            case TileType.Ice:
                GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case TileType.Void:
                GetComponent<SpriteRenderer>().color = Color.white;
                break;
        }
    }

    [Button("RefreshTile")]
    private void RefreshTile()
    {
        RefreshColorSprite();
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

    private bool isRock() { return type == TileType.Rock; }
    private bool isSide() { return position == Position.Side; }
    private bool isCorner() { return position == Position.Corner; }

}
