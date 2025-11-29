using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using FMODUnity;

public class PauseMenuView : UIView
{
    UIManager _uiManager;


    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private EventReference menuPauseSnapshot;

    private FMOD.Studio.EventInstance _menuPauseSnapshotInstance;

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

        if (!menuPauseSnapshot.IsNull)
        {
            _menuPauseSnapshotInstance = RuntimeManager.CreateInstance(menuPauseSnapshot);
            _menuPauseSnapshotInstance.start();
        }
    }

    private void OnDisable()
    {
        resumeButton.onClick.RemoveListener(OnResumeClicked);
        settingsButton.onClick.RemoveListener(OnSettingsClicked);
        quitButton.onClick.RemoveListener(OnQuitClicked);
        restartButton.onClick.RemoveListener(OnRestartClicked);
        EventManager.OnGameUnpause -= CloseMenu;

        if (_menuPauseSnapshotInstance.isValid())
        {
            _menuPauseSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _menuPauseSnapshotInstance.release();
        }
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
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameEntry");

    }

    private void OnRestartClicked()
    {
        Debug.Log("Restarting Level");
        var currentScene = SceneManager.GetActiveScene();
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentScene.name);
    }

    private void CloseMenu()
    {
        _uiManager.ShowInGameExclusive<PlayingView>();
    }


}
