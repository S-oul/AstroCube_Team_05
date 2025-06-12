using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonFunctionality : MonoBehaviour
{
    PauseMenu _pauseMenu;
    [SerializeField] GameObject _firstSelectedButton;

    private void Start()
    {
        _pauseMenu = GetComponentInParent<PauseMenu>();
        EventSystem.current.SetSelectedGameObject(_firstSelectedButton);
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(_firstSelectedButton);
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

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(_firstSelectedButton);
        }
    }
}
