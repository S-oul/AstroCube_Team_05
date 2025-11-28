using UnityEngine;

public class PlayingView : UIView
{

    UIManager _uiManager;

    private void Awake()
    {
        base.Awake();
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void OnEnable()
    {
        EventManager.OnGamePause += ShowMenu;
    }


    private void OnDisable()
    {
        EventManager.OnGamePause -= ShowMenu;
    }


    private void ShowMenu()
    {
        Debug.Log("Showing Pause Menu");
        _uiManager.ShowInGameExclusive<PauseMenuView>();
    }
}
