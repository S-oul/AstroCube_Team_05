using System;
using System.Collections;
using System.Collections.Generic;
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
        if (newGameButton != null)
            newGameButton.onClick.AddListener(OnNewGameClicked);
        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);
        if (LevelsButton != null)
            LevelsButton.onClick.AddListener(OnLevelsClicked);
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsClicked);
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);
        if (titleButton != null)
            titleButton.onClick.AddListener(OnTitleClicked);

    }



    private void OnNewGameClicked()
    {
        //TODO
        //Si Save deja présente faire un popup de confirmation pour ecraser la save
        SceneManager.LoadScene(firstLevelName);

    }

    private void OnContinueClicked()
    {
        //TODO
        //Charger la scene du dernier atteint
        SceneManager.LoadScene(firstLevelName);
    }

    private void OnLevelsClicked()
    {
        Hide();
        var uiManager = FindObjectOfType<UIManager>();
        uiManager?.Show<LevelSelectionPlaceHolderView>();
    }

    private void OnSettingsClicked()
    {
        Hide();
        var uiManager = FindObjectOfType<UIManager>();
        uiManager?.Show<SettingsMenuScreenView>();
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
        else
        {
            c++;
        }
    }

    private void OnQuitClicked()
    {
        var popup = _uiManager.ShowAndReturn<PopUpView>();
        if (popup==null)
        {
            return; 
        }

        popup.ShowPopup(new PopUpData(
            title :"Quit Game",
            message: "Are you sure you want to quit the game?",
            type : PopUpType.QuitGamePopUp,
            confirm: "Yes",
            cancel : "No",
            onConfirm: () => { Application.Quit(); },
            onCancel: () => 
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
