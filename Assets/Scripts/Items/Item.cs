using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item : MonoBehaviour
{
    [HideInInspector] public string itemName;
    [HideInInspector] public bool isVisible;
    [HideInInspector] public Vector2Int position;

    public virtual void PickUp()
    {
        Debug.LogWarning("Override Missing for PickUp() => Item, call Kévin or Justin");
    }

    public virtual IEnumerator AnimationIdle()
    {
        yield return null;
    }

    private void OnValidate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        //JUSTIN UPDATE LA POSITION DE L'OBJET STP
    }
}
