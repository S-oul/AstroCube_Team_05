using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class SettingsMenuScreenView : UIView
{
    [Header("(REQUIRED)")]



    [Header("Description Part")]

    [SerializeField] private TMP_Text titleText;  
    [SerializeField] private TMP_Text descriptionText;



    [Header("Sound Settings")]

    [SerializeField] private Slider generalSoundSlider;
    [SerializeField] private Slider musicSoundSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider voiceSoundSlider;


    [Header("Camera Settings")]

    [SerializeField] private Slider fovSlider;
    [SerializeField] private Slider cameraSensitivitySlider;
    [SerializeField] private UIToggleButton motionBlurButton;


    [Header("Accessibility Settings")]

    [SerializeField] private UIToggleButton rumbleButton;
    [SerializeField] private UIToggleButton previewButton;


    [Header("Others")]

    [SerializeField] private Button backButton;

    [Header("Settings Referendes")]
    [SerializeField] private CustomisedSettings _customisedSettings;



    private bool _isInitializing = false;
    private UIManager _uiManager;
    private Dictionary<string, string> _descriptionBySettings;

    private void Awake()
    {
        base.Awake();
        _uiManager = FindObjectOfType<UIManager>();

        _descriptionBySettings = new Dictionary<string, string>()
        {
            { "General :", "Controls the global sound level" },
            { "Music :", "Controls the music and ambiance sound level"},
            { "Sound Effects :", "Controls the sound level of the sound effects"},
            { "Voice :", "controls the sound level of the voicelines"},

            { "Field of View :", "Changes the angle of the player's field of view. "},
            { "Camera Sensitivity :", "Affects the speed at which the camera moves"},
            { "Motion Blur :", "Enables/Disables the motion blur on the player's screen"},


            { "Rumble :", "Enables/disables the controller rumble"},
            { "Preview Hints :", "Enables/disables the preview feature"},
        };


        _customisedSettings.LoadRuntimeValues();
        SetupUI();
        SetupHover();
    }

    private void SetupUI()
    {
        _isInitializing = true;

        generalSoundSlider.onValueChanged.AddListener(OnGeneralSoundSliderValueChanged);
        musicSoundSlider.onValueChanged.AddListener(OnMusicSoundSliderValueChanged);
        soundSlider.onValueChanged.AddListener(OnSoundSoundSliderValueChanged);
        voiceSoundSlider.onValueChanged.AddListener(OnVoiceSoundSliderValueChanged);

        fovSlider.onValueChanged.AddListener(OnFovSliderChanged);
        cameraSensitivitySlider.onValueChanged.AddListener(OnCameraSensitivityChanged);

        motionBlurButton.onToggleChanged.AddListener(OnMotionBlurToggled);
        rumbleButton.onToggleChanged.AddListener(OnRumbleToggled);
        previewButton.onToggleChanged.AddListener(OnPreviewToggled);

        backButton.onClick.AddListener(OnQuitClicked);

        motionBlurButton.SetState(_customisedSettings.customMotionBlur, false);
        rumbleButton.SetState(_customisedSettings.customVibration, false);
        previewButton.SetState(_customisedSettings.customPreview, false);

        fovSlider.minValue = _customisedSettings.minFOV;
        fovSlider.maxValue = _customisedSettings.maxFOV;
        fovSlider.value = _customisedSettings.customFov;

        cameraSensitivitySlider.minValue = _customisedSettings.minMouse;
        cameraSensitivitySlider.maxValue = _customisedSettings.maxMouse;
        cameraSensitivitySlider.value = _customisedSettings.customMouse;

        generalSoundSlider.minValue = _customisedSettings.minVolume;
        generalSoundSlider.maxValue = _customisedSettings.maxVolume;
        generalSoundSlider.value = _customisedSettings.customVolume;


        musicSoundSlider.minValue = _customisedSettings.minMusicVolume;
        musicSoundSlider.maxValue = _customisedSettings.maxMusicVolume;
        musicSoundSlider.value = _customisedSettings.customMusicVolume;

        soundSlider.minValue = _customisedSettings.minSoundEffectsVolume;
        soundSlider.maxValue = _customisedSettings.maxSoundEffectsVolume;
        soundSlider.value = _customisedSettings.customSoundEffectsVolume;

        voiceSoundSlider.minValue = _customisedSettings.minVoiceVolume;
        voiceSoundSlider.maxValue = _customisedSettings.maxVoiceVolume;
        voiceSoundSlider.value = _customisedSettings.customVoiceVolume;

 

        _isInitializing = false;

        Debug.Log($"[Init Settings] MotionBlur={_customisedSettings.customMotionBlur}, Rumble={_customisedSettings.customVibration}, Preview={_customisedSettings.customPreview}, fov value = {_customisedSettings.customFov},  senssitivity value : {_customisedSettings.customMouse}");
    }

    #region Button Methods



    public override void Show()
    {
        base.Show();
        titleText.text = "";
        descriptionText.text = "";
    }

    private void OnQuitClicked()
    {
        Hide();
        _uiManager.Show<MainMenuView>();
    }

    private void OnMotionBlurToggled(bool state)
    {
        Debug.Log("Motion Blur Toggled : " + state);
        _customisedSettings.customMotionBlur = state;
        _customisedSettings.SaveRuntimeValues();
    }

    private void OnRumbleToggled(bool state)
    {
        Debug.Log("Rumble toggled");
        _customisedSettings.customVibration = state;

        _customisedSettings.SaveRuntimeValues();
    }

    private void OnPreviewToggled(bool state)
    {
        Debug.Log("Preview toggled");
        _customisedSettings.customPreview = state;
        _customisedSettings.SaveRuntimeValues();

    }

    public void OnGeneralSoundSliderValueChanged(float value)
    {
        if(_isInitializing) return;

        _customisedSettings.customVolume = value;
        _customisedSettings.SaveRuntimeValues();
        Debug.Log($"[UI] General sound changed to {value}");
    }

    private void OnMusicSoundSliderValueChanged(float value)
    {
        if (_isInitializing) return;

        _customisedSettings.customMusicVolume = value;
        _customisedSettings.SaveRuntimeValues();
        Debug.Log($"[UI] Music sound changed to {value}");
    }

    private void OnSoundSoundSliderValueChanged(float value)
    {
        if (_isInitializing) return;

        _customisedSettings.customSoundEffectsVolume = value;
        _customisedSettings.SaveRuntimeValues();
        Debug.Log($"[UI] Sound Effects changed to {value}");
    }

    private void OnVoiceSoundSliderValueChanged(float value)
    {
        if (_isInitializing) return;

        _customisedSettings.customVoiceVolume = value;
        _customisedSettings.SaveRuntimeValues();
        Debug.Log($"[UI] Voice sound changed to {value}");
    }

    public void OnFovSliderChanged(float value)
    {
        if (_isInitializing) return;

        _customisedSettings.customFov = value;
        _customisedSettings.SaveRuntimeValues();
        Debug.Log($"[UI] FOV changed to {value}");
    }
    public void OnCameraSensitivityChanged(float value)
    {
        if (_isInitializing) return;

        _customisedSettings.customMouse = value;
        _customisedSettings.SaveRuntimeValues();
        Debug.Log($"[UI] Sensitivity changed to {value}");
    }
    #endregion










    #region Hover Interface Implementation


    private void OnSettingSelected(string key)
    {
        if (_descriptionBySettings.TryGetValue(key, out var description))
        {
            titleText.text = key;
            descriptionText.text = description;
        }
    }

    private void SetupHover()
    {
        AddHover(generalSoundSlider.gameObject, "General :");
        AddHover(musicSoundSlider.gameObject, "Music :");
        AddHover(soundSlider.gameObject, "Sound Effects :");
        AddHover(voiceSoundSlider.gameObject, "Voice :");

        AddHover(fovSlider.gameObject, "Field of View :");
        AddHover(cameraSensitivitySlider.gameObject, "Camera Sensitivity :");
        AddHover(motionBlurButton.gameObject, "Motion Blur :");

        AddHover(rumbleButton.gameObject, "Rumble :");
        AddHover(previewButton.gameObject, "Preview Hints :");

    }

    public void OnSettingHovered(string key)
    {
        if (_descriptionBySettings.TryGetValue(key, out var description))
        {
            titleText.text = key;
            descriptionText.text = description;
        }
    }


    private void AddHover(GameObject obj, string key)
    {
        var hover = obj.GetComponent<SettingsHoverElement>();
        if (hover == null)
            hover = obj.AddComponent<SettingsHoverElement>();
        hover.Initialize(this, key);
    }

    #endregion

}
