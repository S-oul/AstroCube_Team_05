using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControllerUINavigation : MonoBehaviour
{
    [SerializeField] ToggleMenuElements _toggleMenuElements;
    
    [SerializeField] GameObject _mainMenuDefaultButton;
    [SerializeField] GameObject _levelMenuDefaultButton;
    [SerializeField] GameObject _settingsMenuDefaultButton;
    [SerializeField] GameObject _controlsDefaultButton;

    GameObject _currentDefaultButton;

    [SerializeField] public GraphicRaycaster uiRaycaster; // from Canvas
    EventSystem eventSystem;

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

        // set selected to current mouse hover
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        uiRaycaster.Raycast(pointerData, results);
        foreach (RaycastResult result in results)
        {
            Button button = result.gameObject.GetComponentInParent<Button>();
            if (button != null)
            {
                EventSystem.current.SetSelectedGameObject(button.gameObject);
                break;
            }
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
