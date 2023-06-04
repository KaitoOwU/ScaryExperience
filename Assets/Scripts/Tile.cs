using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Tile : MonoBehaviour
{

    [HideInInspector] public float size;
    [HideInInspector] public Height height;

    [HideInInspector] public SpriteUp spritesUp;
    [HideInInspector] public SpriteDown spritesDown;

    public enum Height
    {
        Up,
        Down
    }

    private void Awake()
    {
        size = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    public Vector3 DirectionAddMovePos(TileDown.Direction direction)
    {
        switch (direction)
        {
            case TileDown.Direction.Left:
                return new Vector3(-1, 0, 0);

            case TileDown.Direction.Right:
                return new Vector3(1, 0, 0);

            case TileDown.Direction.Up:
                return new Vector3(0, 1, 0);

            case TileDown.Direction.Down:
                return new Vector3(0, -1, 0);
        }

        return Vector3.zero;
    }
}
