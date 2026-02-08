using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Math = UnityEngine.ProBuilder.Math;

public class TabSelector : MonoBehaviour
{
    private static Color ActiveTabColor => new Color(0.9529412f, 0.7686275f, 0.6313726f, 1f);
    private static Color InactiveTabColor => new Color(1f, 1f, 1f, 1f);
    
    [SerializeField] private CanvasGroup[] _tabs;
    [SerializeField] private TMP_Text[] _tabsName;
    [SerializeField] private GameObject[] _firstTabButtons;
    [SerializeField] private GameObject _controllerIcons;
    private int _currentTabIndex = 0;
    
    [SerializeField] InputAction _controllerSelectTab;

    private void OnEnable()
    {
        SelectTab(0);

        InputSystemManager.Instance.OnCurrentInputModeChange.AddListener(ToggleInputs);
    }

    private void OnDisable()
    {
        InputSystemManager.Instance.OnCurrentInputModeChange.RemoveListener(ToggleInputs);
    }

    private void ToggleInputs(InputSystemManager.EInputMode obj)
    {
        if (obj == InputSystemManager.EInputMode.CONTROLLER)
        {
            _controllerIcons.SetActive(true);
            _controllerSelectTab.Enable();
            _controllerSelectTab.performed += OnControllerSelectTab;
        }
        else
        {
            _controllerIcons.SetActive(false);
            _controllerSelectTab.Disable();
            _controllerSelectTab.performed -= OnControllerSelectTab;
        } 
    }

    private void OnControllerSelectTab(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<float>() <= -0.5f)
        {
            SelectPreviousTab();
        } else if (obj.ReadValue<float>() >= 0.5f)
        {
            SelectNextTab();
        }
    }

    public void SelectNextTab()
    {
        _currentTabIndex = (_currentTabIndex + 1) % _tabs.Length;
        _UpdateTabVisibility();
        EventSystem.current.SetSelectedGameObject(_firstTabButtons[_currentTabIndex]);
    }

    public void SelectPreviousTab()
    {
        _currentTabIndex = (_currentTabIndex - 1 + _tabs.Length) % _tabs.Length;
        _UpdateTabVisibility();
        EventSystem.current.SetSelectedGameObject(_firstTabButtons[_currentTabIndex]);
    }
    
    public void SelectTab(int tabIndex)
    {
        if (tabIndex == _currentTabIndex)
            return;
        
        tabIndex = Math.Clamp(tabIndex, 0, _tabs.Length);
        
        _currentTabIndex = tabIndex;
        _UpdateTabVisibility();
    }

    private void _UpdateTabVisibility()
    {
        for (int i = 0; i < _tabs.Length; i++)
        {
            if (i == _currentTabIndex)
            {
                _tabs[i].DOFade(1f, 0.25f).SetEase(Ease.InOutSine);
                _tabs[i].gameObject.SetActive(true);
                _tabsName[i].color = ActiveTabColor;
            }
            else
            {
                _tabs[i].DOFade(0f, 0.25f).SetEase(Ease.InOutSine);
                _tabs[i].gameObject.SetActive(false);
                _tabsName[i].color = InactiveTabColor;
            }
        }
    }
    
}
