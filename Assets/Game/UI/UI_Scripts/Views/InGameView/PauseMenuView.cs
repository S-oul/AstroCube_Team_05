using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuView : UIView
{
    UIManager _uiManager;


    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;


    private void Awake()
    {
        base.Awake();
        _uiManager = FindObjectOfType<UIManager>();

    }

    private void OnEnable()
    {
        resumeButton.onClick.AddListener(OnResumeClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        restartButton.onClick.AddListener(OnRestartClicked);
        EventManager.OnGameUnpause += CloseMenu;

    }

    private void OnDisable()
    {
        resumeButton.onClick.RemoveListener(OnResumeClicked);
        settingsButton.onClick.RemoveListener(OnSettingsClicked);
        quitButton.onClick.RemoveListener(OnQuitClicked);
        restartButton.onClick.RemoveListener(OnRestartClicked);
        EventManager.OnGameUnpause -= CloseMenu;
    }

    private void OnResumeClicked()
    {
        Debug.Log("Resuming Game");
        _uiManager.ShowInGameExclusive<PlayingView>();
        EventManager.TriggerGameUnpause();
    }

    private void OnSettingsClicked()
    {
        Debug.Log("Opening Settings Menu");
        _uiManager.ShowInGameExclusive<SettingsMenuScreenView>();
    }


    private void OnQuitClicked()
    {
        Debug.Log("Quitting to Main Menu");
        EventManager.TriggerGameUnpause();
        SceneManager.LoadScene("GameEntry");

    }

    private void OnRestartClicked()
    {
        Debug.Log("Restarting Level");
        EventManager.TriggerGameUnpause();
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void CloseMenu()
    {
        _uiManager.ShowInGameExclusive<PlayingView>();
    }


}
