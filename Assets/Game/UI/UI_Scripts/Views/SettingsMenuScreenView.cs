using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;

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


        SetupUI();
        SetupHover();
    }

    private void SetupUI()
    {
        generalSoundSlider.onValueChanged.AddListener(OnGeneralSoundSliderValueChanged);
        musicSoundSlider.onValueChanged.AddListener((value) => OnSettingSelected("Music :"));
        soundSlider.onValueChanged.AddListener((value) => OnSettingSelected("Sound Effects :"));
        voiceSoundSlider.onValueChanged.AddListener((value) => OnSettingSelected("Voice :"));

        fovSlider.onValueChanged.AddListener((value) => OnSettingSelected("Field of View :"));
        cameraSensitivitySlider.onValueChanged.AddListener((value) => OnSettingSelected("Camera Sensitivity :"));

        motionBlurButton.onToggleChanged.AddListener(OnMotionBlurToggled);
        rumbleButton.onToggleChanged.AddListener(OnRumbleToggled);
        previewButton.onToggleChanged.AddListener(OnPreviewToggled);

        backButton.onClick.AddListener(OnQuitClicked);

        motionBlurButton.SetState(_customisedSettings.customMotionBlur, false);
        rumbleButton.SetState(_customisedSettings.customVibration, false);
        previewButton.SetState(_customisedSettings.customPreview, false);

        Debug.Log($"[Init Settings] MotionBlur={_customisedSettings.customMotionBlur}, Rumble={_customisedSettings.customVibration}, Preview={_customisedSettings.customPreview}");
    }

    #region Button Methods

    private void OnQuitClicked()
    {
        Hide();
        _uiManager.Show<MainMenuView>();
    }

    private void OnMotionBlurToggled(bool state)
    {
        Debug.Log("Motion Blur Toggled : " + state);
        _customisedSettings.customMotionBlur = state;
    }

    private void OnRumbleToggled(bool state)
    {
        Debug.Log("Rumble toggled");
        _customisedSettings.customVibration = state;
    }

    private void OnPreviewToggled(bool state)
    {
        Debug.Log("Preview toggled");
        _customisedSettings.customPreview = state;

    }

    public void OnGeneralSoundSliderValueChanged(float value)
    {

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
