using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextApparitionTrigger : MonoBehaviour
{
    [SerializeField] private bool _hide;
    [SerializeField] private List<TextApparition> _texts;
    
    private bool _triggered;

    NarraActivationTool _narraActivationTool;

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("TextTrigger");

#if UNITY_EDITOR
        _narraActivationTool = Object.FindAnyObjectByType<NarraActivationTool>();
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
#if UNITY_EDITOR
        if (_narraActivationTool != null)
        {
            if (_narraActivationTool.IsNarraActiveTool == false) return;
        }
#endif

        if (!other.CompareTag("Player") || _triggered || _texts == null) return;

        foreach (TextApparition text in _texts)
        {
            if(!text) continue;
            if (_hide) text.Hide();
            else text.Display();
        }

        _triggered = true;
    }
}
