using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _UIHolder;
    [SerializeField] GameObject _firstSelected;
    KaleidoscopeManager _kaleidoscopeManager;

    private void Start()
    {
        _kaleidoscopeManager = GetComponentInChildren<KaleidoscopeManager>();
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
        _kaleidoscopeManager.SetEnabled(true);
        _UIHolder.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_firstSelected);
    }

    void CloseMenu()
    {
        _kaleidoscopeManager.SetEnabled(false);
        _UIHolder.SetActive(false);
    }
}
