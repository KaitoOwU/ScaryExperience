using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{

    List<KeyFragment> _associatedFragments = new();
    public List<KeyFragment> AssociatedFragments { get => _associatedFragments;  }

    internal void CheckIfAllKeysTaken()
    {
        IEnumerable<KeyFragment> keysNotTaken =
            from key in AssociatedFragments
            where !key.IsTaken
            select key;

        if (keysNotTaken.Count() == 0)
        {
            Destroy(gameObject);
        }
    }

}
