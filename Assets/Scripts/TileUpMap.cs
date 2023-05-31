using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TileUpMap : MonoBehaviour
{
    [SerializeField] int numberTileX;
    [SerializeField] int numberTileY;

    [SerializeField] GameObject tile;
    [HideInInspector] public float tileSize;

    List<TileUp> _tileMap = new List<TileUp>();

    private void Awake()
    {
        AddAllTiles();
        tileSize = _tileMap[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void AddAllTiles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _tileMap.Add(transform.GetChild(i).GetComponent<TileUp>());
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
            }

            xOffset = 0;
            yOffset -= tile.GetComponent<SpriteRenderer>().bounds.size.y;
        }
    }

    [Button("ClearMap")]
    private void DestroyMap()
    {
        Debug.LogWarning("Destroying");
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

    public TileUp FindTileWithPos(Vector3 pos)
    {
        foreach (TileUp tile in _tileMap)
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

}
