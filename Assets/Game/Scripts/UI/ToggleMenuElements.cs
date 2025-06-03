//using Microsoft.Unity.VisualStudio.Editor;
using MoreMountains.FeedbacksForThirdParty;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public enum MenuElement
{
    TITLE_SCREEN,
    START_MENU,
    LEVELS_MENU,
    SETTINGS_MENU, 
    CONTROLS,
    NULL
}

public class ToggleMenuElements : MonoBehaviour
{
    [SerializeField] GameObject _titleScreen;
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _levelsMenu;
    [SerializeField] GameObject _settingsMenu;
    [SerializeField] GameObject _controls;

    MenuElement _currentActivatedMenu;

    // Deactivated everything other than the title screen
    private void Start()
    {
        _currentActivatedMenu = MenuElement.TITLE_SCREEN;
        if (_titleScreen) _titleScreen.SetActive(true);
        if (_mainMenu) _mainMenu.SetActive(false);
        if (_levelsMenu) _levelsMenu.SetActive(false);
        if (_settingsMenu) _settingsMenu.SetActive(false);
        if (_controls) _controls.SetActive(false);
    }

    // Activates given menu element (and deactivates current active menu).
    public void Activate(MenuElement newMenu)
    {
        if (newMenu == _currentActivatedMenu) return; // asked to open a menu that is already open
        
        // identify new menu's game object
        GameObject menuElementGameObject = GetGameObjectFromMenuElementEnum(newMenu);
        if (menuElementGameObject == null)
        {
            Debug.LogWarning(newMenu.ToString() + " is missing from the inspector.");
            return;
        }

        // deactivate current menu
        if (_currentActivatedMenu != MenuElement.NULL) Deactivate(_currentActivatedMenu);

        // activate new menu 
        menuElementGameObject.SetActive(true);
        menuElementGameObject.GetComponent<CanvasGroup>().alpha = 0;
        fadeAlpha(menuElementGameObject.GetComponent<CanvasGroup>(), 1, 1);

        _currentActivatedMenu = newMenu;
    }

    public async void Deactivate(MenuElement oldMenu)
    {
        _currentActivatedMenu = MenuElement.NULL;

        // identify old menu game object
        GameObject oldMenuGameObject = GetGameObjectFromMenuElementEnum(oldMenu);
        if (oldMenuGameObject.activeSelf == false)
        {
            Debug.LogWarning(oldMenu.ToString() + " is already deactivated.");
            return;
        }

        // deactivate old menu
        await fadeAlpha(oldMenuGameObject.GetComponent<CanvasGroup>(), 0, 1);
        oldMenuGameObject.SetActive(false);
    }

    private GameObject GetGameObjectFromMenuElementEnum(MenuElement menu)
    {
        switch (menu)
        {
            case MenuElement.TITLE_SCREEN: return _titleScreen;
            case MenuElement.START_MENU: return _mainMenu;
            case MenuElement.LEVELS_MENU: return _levelsMenu;
            case MenuElement.SETTINGS_MENU: return _settingsMenu;
            case MenuElement.CONTROLS: return _controls;
        }
        Debug.LogWarning("Unable to identify MenuElement Enum. (This is bad.)");
        return null;
    }

    async Task fadeAlpha(CanvasGroup uiGroup, float targetAlpha, float duration)
    {
        float elapsedTime = 0;
        float startAlpha = uiGroup.alpha;

        while (elapsedTime < duration)
        {
            uiGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            await Task.Yield();
        }
    }
}
