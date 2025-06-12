using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ControlsMenuBackButton : MonoBehaviour
{
    [SerializeField] ToggleMenuElements togMenEl;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        togMenEl.Activate(MenuElement.SETTINGS_MENU);
    }
}
