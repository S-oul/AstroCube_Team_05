using System;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{

    [SerializeField] private string _csvName;
    [SerializeField] private string _localizationId;
    
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        _text.text = LocalizationManager.Instance.GetString(_csvName, _localizationId);
    }
}
