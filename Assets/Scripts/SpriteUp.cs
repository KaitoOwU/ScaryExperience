using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteUp",menuName = "Sprite/SpriteUp", order = 0)]
public class SpriteUp : ScriptableObject
{
    [Header("- Sprite -")]
    public List<Sprite> spriteNone;
    public List<Sprite> spriteWall;
    public List<Sprite> spriteDoor;
    public List<Sprite> spriteLever;
    public List<Sprite> spriteKeyFragment;
    public List<Sprite> spriteDoorKey;
    public List<Sprite> spriteOneWayWall;
    public List<Sprite> spriteBrasero;
    public List<Sprite> spritePortalDoor;
    public List<Sprite> spriteWinBlock;
    public List<Sprite> spriteWinDoor;
    public List<Sprite> spritePressurePlate;

    [Header("- Color -")]
    public Color colorNone;
    public Color colorWall;
    public Color colorDoor;
    public Color colorLever;
    public Color colorKeyFragment;
    public Color colorDoorKey;
    public Color colorOneWayWall;
    public Color colorBrasero;
    public Color colorPortalDoor;
    public Color colorWin;
    public Color colorPressurePlate;
}