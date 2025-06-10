using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuButtonFunc : MonoBehaviour
{
    [SerializeField] CustomisedSettings _cs;  

    [SerializeField] Slider _fovSlider;
    [SerializeField] Slider _mouseSlider;
    [SerializeField] Slider _volumeSlider;

    [SerializeField] Button _vibrationButton; 
    [SerializeField] Button _previewButton; 
    [SerializeField] Button _motionBlurButton; 

    PauseMenu _pauseMenu;

    private void Start()
    {
        _pauseMenu = GetComponentInParent<PauseMenu>();

        _fovSlider.SetValueWithoutNotify(GetSliderPosition(_cs.customFov, _cs.minFOV, _cs.maxFOV));
        _fovSlider.onValueChanged.AddListener((val) => 
        _cs.customFov = GetCustomVal(val, _cs.minFOV, _cs.maxFOV));

        _mouseSlider.SetValueWithoutNotify(GetSliderPosition(_cs.customMouse, _cs.minMouse, _cs.maxMouse));
        _mouseSlider.onValueChanged.AddListener((val) =>
        _cs.customMouse = GetCustomVal(val, _cs.minMouse, _cs.maxMouse));

        _volumeSlider.SetValueWithoutNotify(GetSliderPosition(_cs.customVolume, _cs.minVolume, _cs.maxVolume));
        _volumeSlider.onValueChanged.AddListener((val) =>
        _cs.customVolume = GetCustomVal(val, _cs.minVolume, _cs.maxVolume));

        SetButtonApperance(_vibrationButton, "Vibration", _cs.customVibration);
        SetButtonApperance(_previewButton, "Preview", _cs.customPreview);
        SetButtonApperance(_motionBlurButton, "MotionBlur", _cs.customSubtitles);
    }

    public void BackButton()
    {
        if (_pauseMenu) { _pauseMenu.SetActiveSettingsMenu(false); }
    }

    float GetSliderPosition(float? currentVal, float minVal, float maxVal)
    {
        return ((float)currentVal - minVal) / Mathf.Abs(maxVal - minVal);
    }

    private float GetCustomVal(float val, float minFOV, float maxFOV)
    {
        return Mathf.Lerp(minFOV, maxFOV, val);
    }

    public void ControlsButton()
    {
        Debug.Log("You clicked the Controls button.\nBut this menu does not exist yet.\n\nVanilla Extract :P\n\n");
    }

    public void PreviewButton(Button button)
    {
        _cs.customPreview = !_cs.customPreview;
        SetButtonApperance(button, "Preview", (bool)_cs.customPreview);
    }
    public void VibrationButton(Button button)
    {
        _cs.customVibration = !_cs.customVibration;
        SetButtonApperance(button, "Vibration", (bool)_cs.customVibration);
    }
    public void SubtitlesButton(Button button)
    {
        _cs.customSubtitles = !_cs.customSubtitles;
        SetButtonApperance(button, "Subtitles", (bool)_cs.customSubtitles);
    }

    private void SetButtonApperance(Button button, string v, bool isActivated)
    {
        string isActiveLabel = isActivated ? "On" : "Off";
        button.GetComponentInChildren<TextMeshProUGUI>().text = v + " : " + isActiveLabel;
    }
}
