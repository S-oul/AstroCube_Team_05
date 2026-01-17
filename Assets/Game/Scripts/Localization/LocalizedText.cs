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
        UpdateText();
    }

    public void UpdateText()
    {
        if(_text == null)
            return;
        
        string localizedText = LocalizationManager.Instance.GetString(_csvName, _localizationId);
        _text.text = localizedText;
    }
}
