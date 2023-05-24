using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Etouch = UnityEngine.InputSystem.EnhancedTouch;

public class GameManage : MonoBehaviour
{
    [SerializeField] public Grid grid;

    private void Start()
    {
        Etouch.EnhancedTouchSupport.Enable();
    }

}
