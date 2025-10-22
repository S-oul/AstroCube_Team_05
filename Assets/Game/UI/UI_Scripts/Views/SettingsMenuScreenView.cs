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
    [SerializeField] private Button motionBlurButton;


    [Header("Accessibility Settings")]

    [SerializeField] private Button rumbleButton;
    [SerializeField] private Button previewButton;


    [Header("Others")]

    [SerializeField] private Button quitButton;


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
        SetupHoover();
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



    private void OnSettingSelected(string key)
    {
        if (_descriptionBySettings.TryGetValue(key, out var description))
        {
            titleText.text = key;
            descriptionText.text = description;
        }
    }

    private void OnQuitClicked()
    {
        Hide();
        _uiManager.Show<MainMenuView>();
    }


    private void SetupUI()
    {
        //Lance pas la bonne fonction

        generalSoundSlider.onValueChanged.AddListener((value) => OnSettingSelected("General :"));
        musicSoundSlider.onValueChanged.AddListener((value) => OnSettingSelected("Music :"));
        soundSlider.onValueChanged.AddListener((value) => OnSettingSelected("Sound Effects :"));
        voiceSoundSlider.onValueChanged.AddListener((value) => OnSettingSelected("Voice :"));

        fovSlider.onValueChanged.AddListener((value) => OnSettingSelected("Field of View :"));
        cameraSensitivitySlider.onValueChanged.AddListener((value) => OnSettingSelected("Camera Sensitivity :"));
        motionBlurButton.onClick.AddListener(() => OnSettingSelected("Motion Blur :"));

        rumbleButton.onClick.AddListener(() => OnSettingSelected("Rumble :"));
        previewButton.onClick.AddListener(() => OnSettingSelected("Preview Hints :"));

        quitButton.onClick.AddListener(OnQuitClicked);

    }

    private void SetupHoover()
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


}
