using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuExclusiveButtonFunc : MonoBehaviour
{
    [SerializeField] ToggleMenuElements _toggleMenuElements;
    public void OnBackButtonClick()
    {
        _toggleMenuElements.Activate(MenuElement.START_MENU);
    }

    public void OnControlsButtonClick()
    {
        _toggleMenuElements.Activate(MenuElement.CONTROLS);
    }
}
