using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Tile : MonoBehaviour
{

    [HideInInspector] public float size;
    public Height height;

    public enum Height
    {
        Up,
        Down
    }

    private void Awake()
    {
        size = GetComponent<SpriteRenderer>().bounds.size.x;
        Debug.LogWarning(size);
    }
}
