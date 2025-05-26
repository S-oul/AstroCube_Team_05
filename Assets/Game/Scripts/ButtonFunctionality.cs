using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonFunctionality : MonoBehaviour, IPointerEnterHandler
{
    PauseMenu _pauseMenu;

    [Header("Wwise Events")]
    [SerializeField] private AK.Wwise.Event continueButtonSound;
    [SerializeField] private AK.Wwise.Event settingsButtonSound;
    [SerializeField] private AK.Wwise.Event quitButtonSound;
    [SerializeField] private AK.Wwise.Event hoverButtonSound;

    private void Start()
    {
        _pauseMenu = GetComponentInParent<PauseMenu>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverButtonSound.Post(gameObject);
    }

    public void ContinueButton()
    {
        continueButtonSound.Post(gameObject);
        EventManager.TriggerGameUnpause();
    }

    public void SettingsButton()
    {
        settingsButtonSound.Post(gameObject);
        _pauseMenu.SetActiveSettingsMenu();
    }

    public void QuitButton()
    {
        quitButtonSound.Post(gameObject);
        //Application.Quit();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
