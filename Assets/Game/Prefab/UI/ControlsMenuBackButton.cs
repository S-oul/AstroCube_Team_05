using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenuBackButton : MonoBehaviour
{
    [SerializeField] ToggleMenuElements togMenEl;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            OnBackButtonClick();
        }
    }

    public void OnBackButtonClick()
    {
        if (togMenEl != null) togMenEl.Activate(MenuElement.SETTINGS_MENU);
        else GetComponent<Button>().onClick.Invoke();
    }
}
