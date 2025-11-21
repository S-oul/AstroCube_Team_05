using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class TextAutoSizing : MonoBehaviour
{

    [SerializeField] private Vector2 _margin;
    private TMP_Text _text;

    private void Reset()
    {
        _text = GetComponent<TMP_Text>();
        _text.textWrappingMode = TextWrappingModes.NoWrap;
    }

    public void AutoResize()
    {
        _text.rectTransform.sizeDelta = _text.GetPreferredValues(_text.text) + _margin;
    }
}
