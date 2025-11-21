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
            SceneManager.LoadScene(firstLevelName);
            return;
        }

        var popup = _uiManager.ShowAndReturn<PopUpView>();
        if (popup == null) return;

        popup.ShowPopup(new PopUpData(
            title: "Nouvelle Partie",
            message: "Une sauvegarde existe déjà.\nVoulez-vous recommencer ?",
            type: PopUpType.Warning,
            confirm: "Oui",
            cancel: "Non",
            onConfirm: () =>
            {
                int totalLevels = SceneManager.sceneCountInBuildSettings;
                LevelProgressionSystem.ResetProgression(totalLevels);
                PlayerPrefs.DeleteKey("LastLevelPlayed");
                PlayerPrefs.Save();
                SceneManager.LoadScene(firstLevelName);
            }
        ));
    }

    private void OnContinueClicked()
    {
        int lastLevel = LevelProgressionSystem.GetLastLevel();

        if (lastLevel == -1)
        {
            SceneManager.LoadScene(firstLevelName);
            return;
        }

        SceneManager.LoadScene(lastLevel);
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
            }
        ));
    }
}
