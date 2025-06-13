using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _UIHolder;
    [SerializeField] GameObject _firstSelected;
    PostProcessManager _kaleidoscopeManager;
    [SerializeReference] GameObject SettingsUIHolder;
    [SerializeField] TextMeshProUGUI _sceneName;
    [SerializeField] GameObject ControlsUIHolder;

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
        _sceneName.text = SceneManager.GetActiveScene().name;
    }

    void CloseMenu()
    {
        if (_kaleidoscopeManager) _kaleidoscopeManager.SetEnabled(false);
        _UIHolder.SetActive(false);
        SettingsUIHolder.SetActive(false);
        ControlsUIHolder.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetActiveSettingsMenu(bool isActive = true)
    {
        _UIHolder.SetActive(!isActive);
        SettingsUIHolder.SetActive(isActive);
    }
}
