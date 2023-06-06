using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDisapear : MonoBehaviour
{
    [SerializeField] Image _fire;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<MoveBubble>() != null)
        {
            _fire.DOColor(new(1, 1, 1, .3f), 0.5f).SetEase(Ease.OutBack);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<MoveBubble>() != null)
        {
            _fire.DOColor(new(1, 1, 1, 1f), 0.5f).SetEase(Ease.OutBack);
        }
    }
}
