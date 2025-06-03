using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonFuntionalities : MonoBehaviour
{
    [SerializeField] ToggleMenuElements _toggleMenuElements;

    public void OnStartButtonClick()
    {
        // Open Scene at index 1
        SceneManager.LoadScene(1);
    }

    public void OnContinueButtonClick()
    {
        // Return to last started level
        Debug.Log("CONTINUE is not yet implemented.");
    }

    public void OnLevelsButtonClick()
    {
        _toggleMenuElements.Activate(MenuElement.LEVELS_MENU);
    }

    public void OnSettingsButtonClick()
    {
        _toggleMenuElements.Activate(MenuElement.SETTINGS_MENU);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
