using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SettingsHoverElement : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private string settingKey;
    private SettingsMenuScreenView settingsView;

    public void Initialize(SettingsMenuScreenView view, string key)
    {
        settingsView = view;
        settingKey = key;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (settingsView != null && !string.IsNullOrEmpty(settingKey))
        {
            settingsView.OnSettingHovered(settingKey);
        }
    }
}
