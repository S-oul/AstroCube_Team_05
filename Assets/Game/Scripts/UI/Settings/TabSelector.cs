using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Math = UnityEngine.ProBuilder.Math;

public class TabSelector : MonoBehaviour
{
    private static Color ActiveTabColor => new Color(0.9529412f, 0.7686275f, 0.6313726f, 1f);
    private static Color InactiveTabColor => new Color(1f, 1f, 1f, 1f);
    
    [SerializeField] private CanvasGroup[] _tabs;
    [SerializeField] private TMP_Text[] _tabsName;
    private int _currentTabIndex = 0;

    private void OnEnable()
    {
        SelectTab(0);
    }

    public void SelectNextTab()
    {
        _currentTabIndex = (_currentTabIndex + 1) % _tabs.Length;
        _UpdateTabVisibility();
    }

    public void SelectPreviousTab()
    {
        _currentTabIndex = (_currentTabIndex - 1 + _tabs.Length) % _tabs.Length;
        _UpdateTabVisibility();
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
