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

    [ShowIf("isWater")]
    public WaterType waterType;


    [HideInInspector] public int idWater;
    [HideInInspector] public bool isActivated = false;

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
                GetComponent<SpriteRenderer>().color = spritesDown.colorRock;
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
                break;

            case TileType.Void:
                GetComponent<SpriteRenderer>().sprite = spritesDown.spriteVoid[Random.Range(0, spritesDown.spriteVoid.Count)];
                GetComponent<SpriteRenderer>().color = spritesDown.colorVoid;
                GetComponent<SpriteRenderer>().material = spritesDown.voidMat;
                break;
            case TileType.Water:
                switch (waterType)
                {
                    case WaterType.Normal:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteWater[0];
                        idWater = 0;
                        break;
                    case WaterType.SideR:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteWater[1];
                        idWater = 1;
                        break;
                    case WaterType.Middle:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteWater[2];
                        idWater = 2;
                        break;
                    case WaterType.SideL:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteWater[3];
                        idWater = 3;
                        break;

                }
                

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
                GetComponent<SpriteRenderer>().material = spritesDown.normalMat;
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

    public enum WaterType
    {
        Normal,
        SideR,
        Middle,
        SideL
    }

    private bool isRock() { return type == TileType.Rock; }
    private bool isSide() { return position == Position.Side; }
    private bool isCorner() { return position == Position.Corner; }
    private bool isWater() { return type == TileType.Water; }

}
