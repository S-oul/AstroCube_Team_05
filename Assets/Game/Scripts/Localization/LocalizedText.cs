using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{

    [SerializeField] private string _csvName;
    [SerializeField] private string _localizationId;

    public string CSVName => _csvName;
    public string LocalizationId => _localizationId;
    
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

[CustomEditor(typeof(LocalizedText))]
public class LocalizedTextCustomEditor : Editor
{
    private static string _currentText;
    private static Dictionary<(string csv, string id, ELanguage language), string> _texts;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        LocalizedText localizedText = (LocalizedText)target;
        _texts = LocalizationManager.GenerateCSVInEditor();
        GUILayout.Space(20);
        
        if (_texts.TryGetValue((localizedText.CSVName, localizedText.LocalizationId, ELanguage.ENGLISH), out var text))
        {
            _currentText = text;
            GUI.color = Color.green;
            GUILayout.Label("Text Found", new GUIStyle(GUI.skin.label){fontStyle = FontStyle.Bold, fontSize = 15});
            GUI.color = Color.white;
            GUILayout.Label(_currentText);
        }
        else
        {
            GUI.color = Color.red;
            GUILayout.Label("Error", new GUIStyle(GUI.skin.label){fontStyle = FontStyle.Bold, fontSize = 15});
            GUI.color = Color.white;
            GUILayout.Label($"No text found for <{localizedText.CSVName}:{localizedText.LocalizationId}>");
        }
    }
}
