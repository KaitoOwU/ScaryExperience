using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class ScriptTest : MonoBehaviour
{
    [SerializeField] InputAction _moveControls;
    [SerializeField] float _sens;

    private void OnEnable()
    {
        _moveControls.Enable();
    }

    private void OnDisable()
    {
        _moveControls.Disable();
    }

    private void Start()
    {
        _moveControls.performed += MoveControls_Started;
    }

    private void MoveControls_Started(InputAction.CallbackContext obj)
    {
        transform.position += (Vector3) (obj.ReadValue<Vector2>() * _sens);
    }
}
