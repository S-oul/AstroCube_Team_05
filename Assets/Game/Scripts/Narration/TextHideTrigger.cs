using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHideTrigger : MonoBehaviour
{
    [SerializeField] private List<TextApparition> _texts;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_texts == null) return;

        foreach (TextApparition text in _texts)
        {
            text.Hide();
        }

        GetComponent<Collider>().enabled = false;
    }
}
