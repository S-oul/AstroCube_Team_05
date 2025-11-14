using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionPlaceHolderView : UIView
{

    private UIManager _uiManager;

    protected void Awake()
    {
        base.Awake();
        _uiManager = FindObjectOfType<UIManager>();
    }

    [SerializeField] Button backButton;


    [SerializeField] Button level1Button;
    [SerializeField] Button level2Button;
    [SerializeField] Button level3Button;
    [SerializeField] Button level4Button;
    [SerializeField] Button level5Button;

    [Header("Level Scene Names")]

    [SerializeField] string level1SceneName = "Test_1_Level_Greybox";
    [SerializeField] string level2SceneName = "Test_1_Level_Greybox";
    [SerializeField] string level3SceneName = "Test_1_Level_Greybox";
    [SerializeField] string level4SceneName = "Test_1_Level_Greybox";
    [SerializeField] string level5SceneName = "Test_1_Level_Greybox";

    private void Start()
    {
        if (backButton != null)
            backButton.onClick.AddListener(OnBackClicked);

        if (level1Button != null)
            level1Button.onClick.AddListener(() => OnLevelClicked(level1SceneName));
        if (level2Button != null)
            level2Button.onClick.AddListener(() => OnLevelClicked(level2SceneName));
        if (level3Button != null)
            level3Button.onClick.AddListener(() => OnLevelClicked(level3SceneName));
        if (level4Button != null)
            level4Button.onClick.AddListener(() => OnLevelClicked(level4SceneName));
        if (level5Button != null)
            level5Button.onClick.AddListener(() => OnLevelClicked(level5SceneName));


    }

    private void OnLevelClicked(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    private void OnBackClicked()
    {
        Hide();
        _uiManager.Show<MainMenuView>();
    }

}
