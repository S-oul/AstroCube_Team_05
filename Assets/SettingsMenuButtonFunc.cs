using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuButtonFunc : MonoBehaviour
{
    PauseMenu _pauseMenu;
    [SerializeField] Slider _fovSlider;
    [SerializeField] Slider _mouseSlider;
    [SerializeField] Slider _volumeSlider;

    private void Start()
    {
        _pauseMenu = GetComponentInParent<PauseMenu>();
        _fovSlider.onValueChanged.AddListener((val) =>
        Debug.Log("FOV changed to " + val));
        _mouseSlider.onValueChanged.AddListener((val) =>
        Debug.Log("mouse sensitivity changed to " + val));
        _volumeSlider.onValueChanged.AddListener((val) =>
        Debug.Log("volume changed to " + val));

    }

    public void BackButton()
    {
        _pauseMenu.SetActiveSettingsMenu(false);
    }
}
