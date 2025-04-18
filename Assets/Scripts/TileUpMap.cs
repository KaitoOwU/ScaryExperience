using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Animations;
public class TileUpMap : MonoBehaviour
{
    [SerializeField] int numberTileX;
    [SerializeField] int numberTileY;

    [SerializeField] GameObject tile;
    [HideInInspector] public float tileSize;

    List<TileUp> _tileMap = new List<TileUp>();

    [SerializeField] TileMap downTileMap;
    [SerializeField] SpriteUp spritesUpTiles;
    [SerializeField] SpriteDown spritesDownTiles;

    [SerializeField] GameObject lightPrefab;

    [SerializeField] GameObject torchPrefab;
    [SerializeField] GameObject braseroPrefab;
    [SerializeField] GameObject grillePrefab;

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
                tileTemp.GetComponent<TileUp>().tileUpMap = this;
                tileTemp.GetComponent<TileUp>().tileMap = downTileMap;
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

    [Button("RefreshMap")]
    private void RefreshMap()
    {
        Debug.LogWarning("Refreshing ...");
        for (int i = 0; i < transform.childCount; i++)
        {
            TileUp tile = transform.GetChild(i).GetComponent<TileUp>();
            tile.spritesDown = spritesDownTiles;
            tile.spritesUp = spritesUpTiles;
            
            tile.lightPrefab = lightPrefab;
            tile.flameBraseroPrefab = braseroPrefab;
            tile.flameTorchPrefab = torchPrefab;
            tile.grillePrefab = grillePrefab;
            tile.RefreshColorSprite(false);

            if (tile.GetComponent<PositionConstraint>() == null)
            {
                tile.gameObject.AddComponent<PositionConstraint>();
                ConstraintSource source = new ConstraintSource();
                source.weight = 1;
                source.sourceTransform = tile.transform;
                PositionConstraint posConsTemp = tile.GetComponent<PositionConstraint>();

                posConsTemp.AddSource(source);
                posConsTemp.locked = true;
                posConsTemp.weight = 0;
                posConsTemp.constraintActive = true;
            }
        }
    }

    public TileUp FindTileWithPos(Vector3 pos)
    {
        //Debug.LogWarning( "x : " + Mathf.Floor(Mathf.Abs(pos.x))+ " y : " + Mathf.Floor(Mathf.Abs(pos.y)));

        TileUp test = _tileMap[(int)(Mathf.Floor(Mathf.Abs(pos.x)) + Mathf.Floor(Mathf.Abs(pos.y)) * numberTileX)];
        if (test != null)
        {
            return test;
        }

        foreach (TileUp tile in _tileMap)
        {
            //check x pos
            if (tile.transform.position.x - tile.size / 2 < pos.x && tile.transform.position.x + tile.size / 2 > pos.x)
            {
                //check y pos
                if (tile.transform.position.y - tile.size / 2 < pos.y && tile.transform.position.y + tile.size / 2 > pos.y)
                {
                    //Debug.LogWarning("test : " + test + " vs normal : " + tile);
                    return tile;
                }
            }
        }

        return null;
    }

    public TileUp FindTileWithPosEditor (Vector3 pos)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            TileUp tile = transform.GetChild(i).GetComponent<TileUp>();
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
