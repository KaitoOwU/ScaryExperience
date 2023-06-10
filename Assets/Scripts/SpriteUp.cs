using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteUp",menuName = "Sprite/SpriteUp", order = 0)]
public class SpriteUp : ScriptableObject
{
    [Header("- Sprite -")]
    public List<Sprite> spriteNone;
    public List<Sprite> spriteNoneWall;
    public List<Sprite> spriteSideWall;
    public List<Sprite> spriteCornerWall;
    public List<Sprite> spriteKey;
    public List<Sprite> spriteBrasero;
    public List<Sprite> spriteWinTrappe;
    public List<Sprite> spriteTorch;
    public List<Sprite> spriteVentilateur;
    public List<Sprite> spriteWind;
    public List<Sprite> spriteCollectible;

    [Header("- Color -")]
    public Color colorNone;
    public Color colorWall;
    public Color colorKey;
    public Color colorBrasero;
    public Color colorWinTrappe;
    public Color colorTorch;
    public Color colorVentilateur;
    public Color colorWind;

    [Header("- Lights -")]
    public float radiusLightBrasero;
    public Color colorLightBrasero;
    public float radiusLightTorch;
    public Color colorLightTorch;
    public float radiusLightKey;
    public Color colorLightKey;
    public float radiusLightTrappe;
    public Color colorLightTrappe;

    [Header("- Material -")]
    public List<Material> windMat;
    public Material normalMat;
    
}