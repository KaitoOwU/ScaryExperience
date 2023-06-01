using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteUp",menuName = "Sprite/SpriteUp", order = 0)]
public class SpriteUp : ScriptableObject
{
    [Header("- Sprite -")]
    public List<Sprite> spriteNone;
    public List<Sprite> spriteSideWall;
    public List<Sprite> spriteCornerWall;
    public List<Sprite> spriteDoor;
    public List<Sprite> spriteLever;
    public List<Sprite> spriteKeyFragment;
    public List<Sprite> spriteOneWayWall;
    public List<Sprite> spriteBrasero;
    public List<Sprite> spriteWinTrappe;
    public List<Sprite> spriteTorch;

    [Header("- Color -")]
    public Color colorNone;
    public Color colorWall;
    public Color colorKeyFragment;
    public Color colorOneWayWall;
    public Color colorBrasero;
    public Color colorWinTrappe;
    public Color colorTorch;
}