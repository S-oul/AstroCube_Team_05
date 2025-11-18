using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NAME", menuName = "Localization")]
public class LocalizationFile : ScriptableObject
{
    private List<string> _ids = new();
    private List<LocalizationData> _localizationDatas = new();
}

public struct LocalizationData
{
    public ELanguage language;
    public string text;
}

public enum ELanguage
{
    FRENCH,
    ENGLISH
}
