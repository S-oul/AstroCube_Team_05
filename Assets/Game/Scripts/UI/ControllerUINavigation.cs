using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerUINavigation : MonoBehaviour
{
    [SerializeField] ToggleMenuElements _toggleMenuElements;
    
    [SerializeField] GameObject _mainMenuDefaultButton;
    [SerializeField] GameObject _levelMenuDefaultButton;
    [SerializeField] GameObject _settingsMenuDefaultButton;
    [SerializeField] GameObject _controlsDefaultButton;

    GameObject _currentDefaultButton;

    private void Start()
    {
        _currentDefaultButton = _mainMenuDefaultButton;
    }

    void Update()
    {
        // resets button navigation if it gets lost (due to a mouse click, for example). 
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(_currentDefaultButton);
        }
    }

    private void OnEnable() { if (_toggleMenuElements != null) _toggleMenuElements.OnCurrentActivatedMenuChanged += SetNewDefaultButton; }
    private void OnDisable() { if (_toggleMenuElements != null) _toggleMenuElements.OnCurrentActivatedMenuChanged -= SetNewDefaultButton; }

    // called by ToggleGameElement event to transfer button navigation to a new set of buttons. 
    void SetNewDefaultButton(MenuElement menuElement)
    {
        switch (menuElement)
        {
            case MenuElement.START_MENU:
                _currentDefaultButton = _mainMenuDefaultButton;
                break;
            case MenuElement.LEVELS_MENU:
                _currentDefaultButton = _levelMenuDefaultButton;
                break;
            case MenuElement.SETTINGS_MENU:
                _currentDefaultButton = _settingsMenuDefaultButton;
                break;
            case MenuElement.CONTROLS:
                _currentDefaultButton= _controlsDefaultButton;
                break;
            default: // TITLE_SCREEN and NULL
                break;
        }
        EventSystem.current.SetSelectedGameObject(_currentDefaultButton);
    }
}
