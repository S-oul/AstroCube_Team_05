using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuButtonFunc : MonoBehaviour
{
    PauseMenu _pauseMenu;

    private void Start()
    {
        _pauseMenu = GetComponentInParent<PauseMenu>();
    }

    public void BackButton()
    {
        _pauseMenu.SetActiveSettingsMenu(false);
    }
}
