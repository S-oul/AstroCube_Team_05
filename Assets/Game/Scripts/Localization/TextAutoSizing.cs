using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class TextAutoSizing : MonoBehaviour
{

    [SerializeField] private Vector2 _margin;
    [SerializeField] private TMP_Text _text;
    public float PreferredHeight => _text.preferredHeight;

    private void Reset()
    {
        _text = GetComponent<TMP_Text>();
        _text.textWrappingMode = TextWrappingModes.NoWrap;
    }

    public void AutoResize()
    {
        _text.rectTransform.sizeDelta = _text.GetPreferredValues(_text.text) + _margin;
    }

    public void SetText(string value, Color? color)
    {
        _text.text = value;
        _text.color = color ?? Color.white;
        _text.gameObject.SetActive(value != "");

        AutoResize();
    }
}
