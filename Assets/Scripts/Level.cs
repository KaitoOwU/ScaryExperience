using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Level/Level Data")]
public class Level : ScriptableObject
{
    public SceneAsset levelScene;
    public string levelName;
}
