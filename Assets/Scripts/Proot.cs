using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Etouch = UnityEngine.InputSystem.EnhancedTouch;

public class Proot : MonoBehaviour, IPointerDownHandler
{
    private void Start()
    {
        Etouch.EnhancedTouchSupport.Enable();
        Etouch.Touch.onFingerDown += Touch_onFingerDown;
    }

    private void Touch_onFingerDown(Etouch.Finger obj)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("print log debug");
    }

    private void OnMouseDown()
    {
        Debug.Log("print log debug");
    }
}
