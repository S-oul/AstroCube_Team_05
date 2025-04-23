using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextApparitionTrigger : MonoBehaviour
{
    [SerializeField] private List<TextApparition> _texts;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_texts == null) return;

        foreach (TextApparition text in _texts)
        {
            text.Display();
        }

        GetComponent<Collider>().enabled = false;
    }
}
