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

    [ShowIf("isIce")]
    public IceType iceType;


    [ShowIf("isVoid")]
    public VoidType voidType;

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
        FlashMeng
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
        switch (oldType)
        {
            case TileType.Ice:
                GetComponent<SpriteRenderer>().color = spritesDown.colorRock;
                break;
            case TileType.Void:
                GetComponent<SpriteRenderer>().color = spritesDown.colorRock;
                GetComponent<SpriteRenderer>().material = spritesDown.normalMat;
                break;
            case TileType.Water:
                GetComponent<SpriteRenderer>().material = spritesDown.normalMat;
                break;
        }

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

                GetComponent<SpriteRenderer>().color = spritesDown.colorRock;

                switch (iceType)
                {
                    case IceType.Center:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[0];
                        break;
                    case IceType.SideR:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[1];
                        break;
                    case IceType.SideU:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[2];
                        break;
                    case IceType.SideL:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[3];
                        break;
                    case IceType.SideD:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[4];
                        break;
                    case IceType.ExteriorCornerUR:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[5];
                        break;
                    case IceType.ExteriorCornerUL:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[6];
                        break;
                    case IceType.ExteriorCornerDL:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[7];
                        break;
                    case IceType.ExteriorCornerDR:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[8];
                        break;
                    case IceType.InteriorCornerUR:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[9];
                        break;
                    case IceType.InteriorCornerUL:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[10];
                        break;
                    case IceType.InteriorCornerDL:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[11];
                        break;
                    case IceType.InteriorCornerDR:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[12];
                        break;
                    case IceType.TwoCornerR:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[13];
                        break;
                    case IceType.TwoCornerU:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[14];
                        break;
                    case IceType.TwoCornerL:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[15];
                        break;
                    case IceType.TwoCornerD:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[16];
                        break;
                    case IceType.ThreeCornerUR:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[17];
                        break;
                    case IceType.ThreeCornerUL:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[18];
                        break;
                    case IceType.ThreeCornerDL:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[19];
                        break;
                    case IceType.ThreeCornerDR:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[20];
                        break;
                    case IceType.FourCorner:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[21];
                        break;
                    case IceType.SoloVerticalUp:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[22];
                        break;
                    case IceType.SoloVerticalMiddle:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[23];
                        break;
                    case IceType.SoloVerticalDown:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[24];
                        break;
                    case IceType.SoloHorizontalRight:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[25];
                        break;
                    case IceType.SoloHorizontalMiddle:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[26];
                        break;
                    case IceType.SoloHorizontalLeft:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteIce[27];
                        break;


                }
                break;

            case TileType.Void:

                switch (voidType)
                {
                    case VoidType.None:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteVoid[1];
                        GetComponent<SpriteRenderer>().color = Color.black;
                        GetComponent<SpriteRenderer>().material = spritesDown.voidMat;
                        break;
                    case VoidType.BorderUp:
                        
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteVoid[0];
                        GetComponent<SpriteRenderer>().color = new Color(164, 164, 164, 255);
                        break;
                }
                
                break;

            case TileType.Water:

                GetComponent<SpriteRenderer>().sharedMaterial = new Material(spritesDown.waterMat);

                switch (waterType)
                {
                    case WaterType.Normal:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteWater[0];
                        GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_TextureOut", spritesDown.spriteOutWater[0].texture);
                        GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_TextureIn", spritesDown.spriteInWater[0].texture);
                        idWater = 0;
                        break;

                    case WaterType.SideRight:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteWater[1];
                        GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_TextureOut", spritesDown.spriteOutWater[1].texture);
                        GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_TextureIn", spritesDown.spriteInWater[1].texture);
                        idWater = 1;
                        break;

                    case WaterType.MiddleHorizontal:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteWater[2];
                        GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_TextureOut", spritesDown.spriteOutWater[2].texture);
                        GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_TextureIn", spritesDown.spriteInWater[2].texture);
                        idWater = 2;
                        break;

                    case WaterType.SideLeft:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteWater[3];
                        GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_TextureOut", spritesDown.spriteOutWater[3].texture);
                        GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_TextureIn", spritesDown.spriteInWater[3].texture);
                        idWater = 3;
                        break;
                    case WaterType.SideUp:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteWater[4];
                        GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_TextureOut", spritesDown.spriteOutWater[4].texture);
                        GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_TextureIn", spritesDown.spriteInWater[4].texture);
                        idWater = 4;
                        break;

                    case WaterType.MiddleVertical:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteWater[5];
                        GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_TextureOut", spritesDown.spriteOutWater[5].texture);
                        GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_TextureIn", spritesDown.spriteInWater[5].texture);
                        idWater = 5;
                        break;

                    case WaterType.SideDown:
                        GetComponent<SpriteRenderer>().sprite = spritesDown.spriteWater[6];
                        GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_TextureOut", spritesDown.spriteOutWater[6].texture);
                        GetComponent<SpriteRenderer>().sharedMaterial.SetTexture("_TextureIn", spritesDown.spriteInWater[6].texture);
                        idWater = 6;
                        break;

                }
                

                break;
            case TileType.Breakable:
                GetComponent<SpriteRenderer>().sprite = spritesDown.spriteBreakable[0];
                break;
            case TileType.FlashMeng:
                GetComponent<SpriteRenderer>().sprite = spritesDown.spriteFlashMeng[0];
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
        SideRight,
        MiddleHorizontal,
        SideLeft,
        SideUp,
        MiddleVertical,
        SideDown
    }

    public enum IceType
    {
        Center,
        SideR,
        SideU,
        SideL,
        SideD,
        ExteriorCornerUR,
        ExteriorCornerUL,
        ExteriorCornerDL,
        ExteriorCornerDR,
        InteriorCornerUR,
        InteriorCornerUL,
        InteriorCornerDL,
        InteriorCornerDR,
        TwoCornerR,
        TwoCornerU,
        TwoCornerL,
        TwoCornerD,
        ThreeCornerUR,
        ThreeCornerUL,
        ThreeCornerDL,
        ThreeCornerDR,
        FourCorner,
        SoloVerticalUp,
        SoloVerticalMiddle,
        SoloVerticalDown,
        SoloHorizontalRight,
        SoloHorizontalMiddle,
        SoloHorizontalLeft
    }

    public enum VoidType
    {
        None,
        BorderUp
    }

    private bool isRock() { return type == TileType.Rock; }
    private bool isSide() { return position == Position.Side; }
    private bool isCorner() { return position == Position.Corner; }
    private bool isWater() { return type == TileType.Water; }
    private bool isIce() { return type == TileType.Ice; }

    private bool isVoid() { return type == TileType.Void; }

}
