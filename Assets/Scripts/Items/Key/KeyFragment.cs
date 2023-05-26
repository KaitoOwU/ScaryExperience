using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFragment : Item
{
    [SerializeField] KeyDoor _associatedDoor;
    bool _isTaken;

    public bool IsTaken { get => _isTaken; }

    private void Start()
    {
        itemName = "KeyFragment";
        isVisible = true;

        _associatedDoor.AssociatedFragments.Add(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _isTaken = true;
            _associatedDoor.CheckIfAllKeysTaken();
            Destroy(gameObject);
        }
    }
}
