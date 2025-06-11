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

    [SerializeField] ToggleButtonFunctionality _vibrationButton; 
    [SerializeField] ToggleButtonFunctionality _previewButton; 
    [SerializeField] ToggleButtonFunctionality _motionBlurButton;

    [SerializeField] ToggleMenuElements _toggelMenuElem;
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

        _vibrationButton.SetButtonState(_cs.customVibration);
        _previewButton.SetButtonState(_cs.customPreview);
        _motionBlurButton.SetButtonState(_cs.customMotionBlur);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1)) // if player presses B on an Xbox controler. 
        {
            BackButton();
        }
    }

    public void BackButton()
    {
        if (_pauseMenu) { _pauseMenu.SetActiveSettingsMenu(false); }
        else
        {
            _toggelMenuElem.Activate(MenuElement.START_MENU);
        }
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
        if(_toggelMenuElem)_toggelMenuElem.Activate(MenuElement.CONTROLS); // only for start menu
    }

    public void PreviewButton()
    {
        _cs.customPreview = !_cs.customPreview;
        _previewButton.SetButtonState(_cs.customPreview);
    }
    public void VibrationButton()
    {
        _cs.customVibration = !_cs.customVibration;
        _vibrationButton.SetButtonState(_cs.customVibration);
    }
    public void MotionBlurButton()
    {
        _cs.customMotionBlur = !_cs.customMotionBlur;
        _motionBlurButton.SetButtonState(_cs.customMotionBlur);
    }
}
