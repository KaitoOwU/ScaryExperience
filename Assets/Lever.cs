using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] GameObject _door;
    [SerializeField] GameObject _player;
    bool isActive = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning("Collision");
        if (!isActive)
        {
            if (collision.gameObject == _player)
            {
                _door.GetComponent<Door>().Open();
                isActive = true;
            }
        }
    }
}
