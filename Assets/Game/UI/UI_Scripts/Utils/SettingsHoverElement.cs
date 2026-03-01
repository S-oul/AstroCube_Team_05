using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SettingsHoverElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
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
            settingsView.SetCurrentKey(settingKey);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (settingsView != null && !string.IsNullOrEmpty(settingKey))
        {
            settingsView.OnSettingHovered("");
            settingsView.SetCurrentKey("");
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        showDescription();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        hideDescription();
    }

    private void showDescription()
    {
        if (settingsView != null && !string.IsNullOrEmpty(settingKey))
        {
            settingsView.OnSettingHovered(settingKey);
            settingsView.SetCurrentKey(settingKey);
        }
    }

    private void hideDescription()
    {
        if (settingsView != null)
        {
            settingsView.OnSettingHovered("");
            settingsView.SetCurrentKey("");
        }
    }
}
