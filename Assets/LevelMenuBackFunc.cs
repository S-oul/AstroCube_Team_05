using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenuBackFunc : MonoBehaviour
{
    [SerializeField] ToggleMenuElements _toggleMenuElement;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1)) // if player presses B on an Xbox controler. 
        {
            OnBackButtonClick();
        }
    }

    public void OnBackButtonClick()
    {
        _toggleMenuElement.Activate(MenuElement.START_MENU);
    }
}
