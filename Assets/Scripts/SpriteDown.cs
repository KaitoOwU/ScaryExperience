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
    [Header("- Ice -")]
    public List<Sprite> spriteIce;
    public List<Sprite> spriteVoid;
    public List<Sprite> spriteWater;
    public List<Sprite> spriteWind;
    public List<Sprite> spriteBreakable;

    [Header("- Color -")]
    public Color colorRock;
    public Color colorIce;
    public Color colorVoid;
    public Color colorWater;
    public Color colorWind;
    public Color colorBreakable;
}

