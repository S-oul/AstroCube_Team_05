using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _UIHolder;
    [SerializeField] GameObject _firstSelected;
    PostProcessManager _kaleidoscopeManager;
    [SerializeReference] GameObject SettingsUIHolder;

    private void Start()
    {
        _kaleidoscopeManager = GetComponentInChildren<PostProcessManager>();
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
        if(_kaleidoscopeManager) _kaleidoscopeManager.SetEnabled(true);
        _UIHolder.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_firstSelected);
    }

    void CloseMenu()
    {
        if (_kaleidoscopeManager) _kaleidoscopeManager.SetEnabled(false);
        _UIHolder.SetActive(false);
        SettingsUIHolder.SetActive(false);
    }

    public void SetActiveSettingsMenu(bool isActive = true)
    {
        _UIHolder.SetActive(!isActive);
        SettingsUIHolder.SetActive(isActive);
    }
}
