using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TileUp : Tile
{
    [Header("- Stats -")]
    public TileUpType type;
    private TileUpType oldType;

    [ShowIf("isWinTrappe")]
    public int numberPartKeyRequired = 1;

    [ShowIf("isBrasero")]
    public int refillAmount = 10;

    [ShowIf("isOneWayWall")]
    public TileDown.Direction directionToGoThrough;

    [ShowIf("isBlock")]
    public GameObject block;

    [Header("- ToHook -")]
    [SerializeField] GameObject blockPrefab;
    [SerializeField] SpriteUp sprites;

    //public hide
    [HideInInspector] public bool isActivated = false;

    public enum TileUpType
    {
        None,
        Wall,
        KeyFragment,
        OneWayWall,
        Brasero,
        Torch,
        Block,
        WinTrappe,
    }

    public void GoBackToWhite ()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    // change la door lorqu'on la met dans l'inspecteur
    private void OnValidate()
    {
        // si l'on ne désigne plus la case comme étant block, on delete le block (object)
        if (type != TileUpType.Block && block != null)
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                DestroyImmediate(block);
            };
        }

        if (oldType != type)
        {
            RefreshColorSprite(true);

            switch (oldType)
            {
                default:
                    break;
            }
        }

        oldType = type;
    }

    public void RefreshColorSprite(bool checkBlock)
    {
        switch (type)
        {
            case TileUpType.None:
                GetComponent<SpriteRenderer>().color = sprites.colorNone;
                break;
            case TileUpType.Wall:
                GetComponent<SpriteRenderer>().color = sprites.colorWall;
                break;
            case TileUpType.KeyFragment:
                GetComponent<SpriteRenderer>().color = sprites.colorKeyFragment;
                break;
            case TileUpType.OneWayWall:
                GetComponent<SpriteRenderer>().color = sprites.colorOneWayWall;
                break;
            case TileUpType.Torch:
                GetComponent<SpriteRenderer>().color = sprites.colorTorch;
                break;
            case TileUpType.Brasero:
                GetComponent<SpriteRenderer>().color = sprites.colorBrasero;
                break;
            case TileUpType.WinTrappe:
                GetComponent<SpriteRenderer>().color = sprites.colorWinTrappe;
                break;

            case TileUpType.Block:
                if (block == null && checkBlock)
                {
                    GameObject temp = Instantiate(blockPrefab, transform);
                    block = temp;
                }
                break;
        }
    }


    private bool isWall() { return type == TileUpType.Wall; }
    private bool isOneWayWall() { return type == TileUpType.OneWayWall; }
    private bool isBrasero() { return type == TileUpType.Brasero; }
    private bool isBlock() { return type == TileUpType.Block; }
    private bool isWinTrappe() { return type == TileUpType.WinTrappe; }
}
