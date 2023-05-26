using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Etouch = UnityEngine.InputSystem.EnhancedTouch;

public class GameManage : MonoBehaviour
{
    [SerializeField] public TileMap tileMap;
    [SerializeField] public TileUpMap tileUpMap;

    private void Start()
    {
        Etouch.EnhancedTouchSupport.Enable();
    }

}
