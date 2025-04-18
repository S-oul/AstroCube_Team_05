using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeReference] GameObject UIHolder;
    [SerializeReference] GameObject SettingsUIHolder;
    KaleidoscopeManager kaleidoscopeManager;

    private void Start()
    {
        kaleidoscopeManager = GetComponentInChildren<KaleidoscopeManager>();
    }

    private void OnEnable()
    {
        EventManager.OnGamePause += OpenMenu;
        EventManager.OnGameUnpause += CloseMenu;
    }

    private void OnDisable()
    {
        EventManager.OnGamePause -= OpenMenu;
        EventManager.OnGameUnpause -= CloseMenu;
    }

    void OpenMenu()
    {
        kaleidoscopeManager.SetEnabled(true);
        UIHolder.SetActive(true);
    }

    void CloseMenu()
    {
        kaleidoscopeManager.SetEnabled(false);
        UIHolder.SetActive(false);
        SettingsUIHolder.SetActive(false);
    }

    public void SetActiveSettingsMenu(bool isActive = true)
    {
        UIHolder.SetActive(!isActive);
        SettingsUIHolder.SetActive(isActive);
    }
}
