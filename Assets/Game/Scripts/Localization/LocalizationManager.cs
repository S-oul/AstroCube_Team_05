using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    private Dictionary<(string id, ELanguage language), string> _idToDialog = new();

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
            string fullText = csv.text;
            string[] lines = fullText.Split('\n')[1..];
            
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
