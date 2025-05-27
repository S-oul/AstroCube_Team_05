using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctionality : MonoBehaviour
{
    PauseMenu _pauseMenu;

    private void Start()
    {
        _pauseMenu = GetComponentInParent<PauseMenu>();
    }

    public void ContinueButton()
    {
        EventManager.TriggerGameUnpause();
    }

    public void SettingsButton()
    {
        _pauseMenu.SetActiveSettingsMenu();
    }

    public void QuitButton()
    {
        //Application.Quit();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
