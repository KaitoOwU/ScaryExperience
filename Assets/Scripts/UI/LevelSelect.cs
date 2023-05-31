using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelSelect : MonoBehaviour
{
    [SerializeField] Transform _parent;
    [SerializeField] GameObject _levelPrefab;
    private float _height = 0f;
    public bool IsActive { get; set; }

    [Button]
    internal void AddLevel()
    {
        Instantiate(_levelPrefab, _parent).GetComponent<LevelUI>().SetupUI(_parent.childCount, _parent);
    }
}
