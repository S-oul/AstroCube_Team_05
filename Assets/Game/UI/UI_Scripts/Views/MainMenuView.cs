using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuView : UIView
{
    [Header("(REQUIRED)")]
    [SerializeField] private string firstLevelName = "LVL01_NAR02";
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button LevelsButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button titleButton;

    private UIManager _uiManager;

    protected void Awake()
    {
        base.Awake();
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        newGameButton.onClick.AddListener(OnNewGameClicked);
        continueButton.onClick.AddListener(OnContinueClicked);
        LevelsButton.onClick.AddListener(OnLevelsClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        titleButton.onClick.AddListener(OnTitleClicked);
    }

    public override void Show()
    {
        base.Show();

        continueButton.interactable = LevelProgressionSystem.HasProgression();
    }

    private void OnNewGameClicked()
    {
        if (!LevelProgressionSystem.HasProgression())
        {
            StartFreshGame();
            return;
        }

        var popup = _uiManager.ShowAndReturn<PopUpView>();
        if (popup == null) return;
        Hide();
        popup.ShowPopup(new PopUpData(
            title: "New Game",
            message: "A save already exists \n do you wanna erase the previous one",
            type: PopUpType.Warning,
            confirm: "Yes",
            cancel: "No",
            onConfirm: () =>
            {
                StartFreshGame();
            }
            , onCancel: () =>
            {
                _uiManager.ShowInGameExclusive<MainMenuView>();
            }
        ));
    }

    private void StartFreshGame()
    {
        int totalLevels = SceneManager.sceneCountInBuildSettings;

        LevelProgressionSystem.ResetProgression(totalLevels);
        LevelProgressionSystem.LockAllLevelsExceptFirst(totalLevels);
        LevelProgressionSystem.ResetLastLevel();

        SceneManager.LoadScene(firstLevelName);
    }

    public void OnContinueClicked()
    {
        int last = LevelProgressionSystem.GetLastLevel();

        if (last != -1)
            SceneManager.LoadScene(last);
    }

    private void OnLevelsClicked()
    {
        Hide();
        _uiManager.Show<LevelSelectionView>();
    }

    private void OnSettingsClicked()
    {
        Hide();
        _uiManager.Show<SettingsMenuScreenView>();
    }

    private int c = 0;
    private void OnTitleClicked()
    {
        if (c > 19)
        {
            Hide();
            c = 0;
            _uiManager.Show<AlternateScreenView>();
        }
        else c++;
    }

    private void OnQuitClicked()
    {
        var popup = _uiManager.ShowAndReturn<PopUpView>();
        if (popup == null) return;
        Hide();
        popup.ShowPopup(new PopUpData(
            title: "Quit Game",
            message: "Are you sure you want to quit the game?",
            type: PopUpType.QuitGamePopUp,
            confirm: "Yes",
            cancel: "No",
            onConfirm: () =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            },
            onCancel: () =>
                        {
                            _uiManager.ShowInGameExclusive<MainMenuView>();
                        }
        ));
    }
}
