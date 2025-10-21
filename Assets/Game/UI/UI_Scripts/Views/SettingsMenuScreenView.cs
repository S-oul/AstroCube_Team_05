using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class SettingsMenuScreenView : UIView
{
    private UIManager _uiManager;

    private void Awake()
    {
        base.Awake();
    }

    public override void Show()
    {
        base.Show();
        gameObject.SetActive(true);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public override void Hide()
    {
        base.Hide();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

}
