using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using FMODUnity;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class PauseMenuView : UIView
{
    UIManager _uiManager;


    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private EventReference menuPauseSnapshot;
    [SerializeField] private EventReference openPauseMenu;
    [SerializeField] private EventReference closePauseMenu;

    private FMOD.Studio.EventInstance _menuPauseSnapshotInstance;
    private InputAction _cancelAction;
    private bool _isInitialized = false;
    private bool _keepSnapshotActive = false;


    private void Awake()
    {
        base.Awake();
        _uiManager = FindObjectOfType<UIManager>();


    }

    private void OnEnable()
    {
        var uiModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
        _cancelAction = uiModule.cancel;

        resumeButton.onClick.AddListener(OnResumeClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        restartButton.onClick.AddListener(OnRestartClicked);
        EventManager.OnGameUnpause += CloseMenu;
        _cancelAction.performed += OnCancelPerformed;

        if (!menuPauseSnapshot.IsNull && !_menuPauseSnapshotInstance.isValid())
        {
            _menuPauseSnapshotInstance = RuntimeManager.CreateInstance(menuPauseSnapshot);
            _menuPauseSnapshotInstance.start();
        }
        
        if (_isInitialized)
        {
            FMODUnity.RuntimeManager.PlayOneShot(openPauseMenu);
        }
        _isInitialized = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


    }

    

    private void OnDisable()
    {


        resumeButton.onClick.RemoveListener(OnResumeClicked);
        settingsButton.onClick.RemoveListener(OnSettingsClicked);
        quitButton.onClick.RemoveListener(OnQuitClicked);
        restartButton.onClick.RemoveListener(OnRestartClicked);
        EventManager.OnGameUnpause -= CloseMenu;
        _cancelAction.performed -= OnCancelPerformed;

        if (!_keepSnapshotActive && _menuPauseSnapshotInstance.isValid())
        {
            _menuPauseSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _menuPauseSnapshotInstance.release();
        }
        
        _keepSnapshotActive = false;
    }

    private void OnCancelPerformed(InputAction.CallbackContext ctx)
    {
        Time.timeScale = 1f;
        _uiManager.ShowInGameExclusive<PlayingView>();
    }

    private void OnResumeClicked()
    {
        Debug.Log("Resuming Game");
        _uiManager.ShowInGameExclusive<PauseMenuView>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EventManager.TriggerGameUnpause();
    }

    private void OnSettingsClicked()
    {
        Debug.Log("Opening Settings Menu");
        _keepSnapshotActive = true;
        _uiManager.ShowInGameExclusive<SettingsMenuScreenView>();
    }


    private void OnQuitClicked()
    {
        var popup = _uiManager.ShowAndReturn<PopUpView>();
        if (popup == null) return;

        popup.ShowPopup(new PopUpData(
            title: "Quit Game ?",
            message: "",
            type: PopUpType.Warning,
            confirm: "Yes",
            cancel: "No",
            onConfirm: () =>
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene("GameEntry");
            },
            onCancel: () =>
            {
                _uiManager.ShowInGameExclusive<PauseMenuView>();

            }
        ));

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
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _uiManager.ShowInGameExclusive<PlayingView>();
        FMODUnity.RuntimeManager.PlayOneShot(closePauseMenu);
    }


}
