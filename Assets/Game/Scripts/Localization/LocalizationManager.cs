using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    private Dictionary<(string csv, string id, ELanguage language), string> _idToDialog = new();

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
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
            for (var value = 0; value < values.Length; value++)
            {
                
            }
        }
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
