using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteDown", menuName = "Sprite/SpriteDown", order = 1)]
public class SpriteDown : ScriptableObject
{
    [Header("- Sprite -")]
    [Header("- Rock -")]
    public List<Sprite> spriteRock;
    public List<Sprite> spriteSideRock;
    public List<Sprite> spriteCornerRock;
    [Header("- Others -")]
    public List<Sprite> spriteIce;
    public List<Sprite> spriteVoid;
    public List<Sprite> spriteWater;
    public List<Sprite> spriteBreakable;
    public List<Sprite> spriteFlashMeng;

    [Header("- Color -")]
    public Color colorRock;
    public Color colorIce;
    public Color colorVoid;
    public Color colorWater;
    public Color colorBreakable;

    [Header("- Material -")]
    public Material normalMat;
    public Material voidMat;
    public Material waterMat;

    public List<Sprite> spriteOutWater;
    public List<Sprite> spriteInWater;

    public List<Sprite> spriteOutWaterBlock;
    public List<Sprite> spriteInWaterBlock;
}

