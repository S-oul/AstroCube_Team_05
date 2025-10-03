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
        throw new NotImplementedException();
    }

    private void OnSettingsClicked()
    {
        throw new NotImplementedException();
    }

    private void OnQuitClicked()
    {
        Debug.Log("Quit Game");
    }

}
