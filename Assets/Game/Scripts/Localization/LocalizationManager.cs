using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    [SerializeField] private ELanguage _currentLanguage = ELanguage.ENGLISH;
    private Dictionary<(string csv, string id, ELanguage language), string> _idToDialog = new();
    
    [SerializeField] private TextAutoSizing _textAutoSizing;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
    }

    private void Start()
    {
        _idToDialog = new();
        var csvFiles = Resources.LoadAll<TextAsset>("Localization");
        foreach (TextAsset csv in csvFiles)
        {
            UnparseCSV(csv);
        }
    }
    
    private void UnparseCSV(TextAsset csv)
    {

        string csvName = csv.name;
        string[] lines = csv.text.Split('\n');
        string[] ids = lines[0].Split(';');

        for (var line = 1; line < lines.Length; line++)
        {
            string[] values = lines[line].Split(';');
            string id = values[0];
            for (var value = 1; value < values.Length - 1; value++)
            {
                if (Enum.TryParse(ids[value], out ELanguage language))
                {
                    _idToDialog[(csvName, id, language)] = values[value];
                }
                else
                {
                    throw new Exception("CSV columns are badly generated.");
                }
            }
        }
    }

    public string GetString(string csvName, string id)
    {
        if (_idToDialog.ContainsKey((csvName, id, _currentLanguage)))
        {
            return _idToDialog[(csvName, id, _currentLanguage)];
        }
        else
        {
            Debug.LogWarning($"Didn't find any key corresponding to : {csvName}:{id}");
            return $"<{csvName}:{id}>";
        }
    }

    public void PrintString(string value, Color? color = null)
    {
        _textAutoSizing.SetText(value, color);
    }

    public void PrintStringFromID(string csvName, string id, Color? color = null)
    {
        PrintString(GetString(csvName, id), color);
    }
}

public enum ELanguage
{
    ENGLISH = 1,
    FRENCH = 2,
    LANGUAGE_3 = 3,
    LANGUAGE_4 = 4,
    LANGUAGE_5 = 5,
    LANGUAGE_6 = 6,
    LANGUAGE_7 = 7,
    LANGUAGE_8 = 8,
    LANGUAGE_9 = 9,
    LANGUAGE_10 = 10
}
