using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TileMap : MonoBehaviour
{
    [SerializeField] int numberTileX;
    [SerializeField] int numberTileY;

    [SerializeField] GameObject tile;
    [HideInInspector] public float tileSize;

    List<TileDown> _tileMap = new List<TileDown>();

    [SerializeField] SpriteUp spritesUpTiles;
    [SerializeField] SpriteDown spritesDownTiles;

    private void Start()
    {
        AddAllTiles();
        tileSize = 1;
    }

    private void AddAllTiles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _tileMap.Add(transform.GetChild(i).GetComponent<TileDown>());
        }
    }

    [Button("GenerateMap")]
    private void GenerateMap()
    {
        Debug.LogWarning("Generating Map ...");
        float xOffset = 0;
        float yOffset = 0;

        for (int i = 0; i < numberTileY; i++)
        {
            for (int j = 0; j < numberTileX; j++)
            {
                Tile tileTemp = Instantiate(tile, new Vector3(xOffset, yOffset, 0), Quaternion.identity, transform).GetComponent<Tile>();
                tileTemp.name = "X : " + j.ToString() + " / Y : " + i.ToString();
                xOffset += tile.GetComponent<SpriteRenderer>().bounds.size.x;
                tileTemp.spritesDown = spritesDownTiles;
                tileTemp.spritesUp = spritesUpTiles;
            }

            xOffset = 0;
            yOffset -= tile.GetComponent<SpriteRenderer>().bounds.size.y;
        }
    }

    [Button("ClearMap")]
    private void DestroyMap()
    {
        Debug.LogWarning("Clearing ...");
        for (int i = 0; i < transform.childCount; i++)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
                i--;
            }
            else
            {
                Destroy(transform.GetChild(i).gameObject);
                i--;
            }
        }
    }

    [Button("RefreshMap")]
    private void RefreshMap ()
    {
        Debug.LogWarning("Refreshing ...");
        for (int i = 0; i < transform.childCount; i++)
        {
            TileDown tile = transform.GetChild(i).GetComponent<TileDown>();
            tile.spritesDown = spritesDownTiles;
            tile.spritesUp = spritesUpTiles;
            tile.RefreshColorSprite();
        }
    }

    public TileDown FindTileWithPos(Vector3 pos)
    {
        TileDown test = _tileMap[(int)(Mathf.Floor(Mathf.Abs(pos.x)) + Mathf.Floor(Mathf.Abs(pos.y)) * numberTileX)];
        if (test != null)
        {
            return test;
        }

        foreach (TileDown tile in _tileMap)
        {
            //check x pos
            if (tile.transform.position.x - tile.size / 2 < pos.x && tile.transform.position.x + tile.size / 2 > pos.x)
            {
                //check y pos
                if (tile.transform.position.y - tile.size / 2 < pos.y && tile.transform.position.y + tile.size / 2 > pos.y)
                {
                    return tile;
                }
            }
        }

        return null;
    }

    public TileDown FindTileWithPosEditor(Vector3 pos)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            TileDown tile = transform.GetChild(i).GetComponent<TileDown>();

            //check x pos
            if (tile.transform.position.x - 1 / 2 <= pos.x && tile.transform.position.x + 1 / 2 >= pos.x)
            {

                //check y pos
                if (tile.transform.position.y - 1 / 2 <= pos.y && tile.transform.position.y + 1 / 2 >= pos.y)
                {
                    return tile;
                }
            }
        }

        return null;
    }

}
