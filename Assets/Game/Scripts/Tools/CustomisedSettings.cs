using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "CustomisedSettings", menuName = "ScriptableObjects/CustomisedSettings", order = 2)]
public class CustomisedSettings : ScriptableObject
{
    public float defaultFOV => _defaultFOV;
    public float minFOV => _minimumFOV;
    public float maxFOV => _maximumFOV;
    public float customFov
    {
        get => _customFov ?? defaultFOV;
        set
        {
            if (value < minFOV)
                _customFov = minFOV;
            else if (value > maxFOV)
                _customFov = maxFOV;
            else
                _customFov = value;

            EventManager.TriggerFOVChange(customFov);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }

    public float defaultMouse => _defaultMouse;
    public float minMouse => _minimumMouse;
    public float maxMouse => _maximumMouse;
    public float customMouse
    {
        get => _customMouse ?? defaultMouse;
        set
        {
            if (value < minMouse)
                _customMouse = minMouse;
            else if (value > maxMouse)
                _customMouse = maxMouse;
            else
                _customMouse = value;

            EventManager.TriggerMouseChange(customMouse);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }

    public float defaultVolume => _defaultVolume;
    public float minVolume => _minimumVolume;
    public float maxVolume => _maximumVolume;
    public float customVolume
    {
        get => _customVolume ?? defaultVolume;
        set
        {
            if (value < minVolume)
                _customVolume = minVolume;
            else if (value > maxVolume)
                _customVolume = maxVolume;
            else
                _customVolume = value;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }

    public bool defaultVibration => _defaultVibration;
    public bool customVibration
    {
        get => _customVibration ?? _defaultVibration;
        set
        {
            _customVibration = value;
            EventManager.TriggerVibrationChange(customVibration);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }

    public bool defaultMotionBlur => _defaultMotionBlur;
    public bool customMotionBlur
    {
        get => _customMotionBlur ?? _defaultMotionBlur;
        set
        {
            _customMotionBlur = value;
            EventManager.TriggerMotionBlurChange(customMotionBlur);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }

    public bool defaultPreview => _defaultPreview;
    public bool customPreview
    {
        get => _customPreview ?? defaultPreview;
        set
        {
            _customPreview = value;
            EventManager.TriggerPreviewChange(customPreview);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }

    public bool defaultSubtitles => _defaultSubtitles;
    public bool customSubtitles
    {
        get => _customSubtitles ?? defaultSubtitles;
        set
        {
            _customSubtitles = value;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }

    [Header("FOV")]
    [SerializeField, Label("Default")] float _defaultFOV;
    [SerializeField, Label("Minimum")] float _minimumFOV;
    [SerializeField, Label("Maximum")] float _maximumFOV;
    float? _customFov;

    [Header("Mouse Sensibility")]
    [SerializeField, Label("Default")] float _defaultMouse;
    [SerializeField, Label("Minimum")] float _minimumMouse;
    [SerializeField, Label("Maximum")] float _maximumMouse;
    float? _customMouse;

    [Header("Volume")]
    [SerializeField, Label("Default")] float _defaultVolume;
    [SerializeField, Label("Minimum")] float _minimumVolume;
    [SerializeField, Label("Maximum")] float _maximumVolume;
    float? _customVolume;

    [Header("Vibration")]
    [SerializeField, Label("Default")] bool _defaultVibration;
    bool? _customVibration;

    [Header("Motion Blur")]
    [SerializeField, Label("Default")] bool _defaultMotionBlur;
    bool? _customMotionBlur;

    [Header("Preview")]
    [SerializeField, Label("Default")] bool _defaultPreview;
    bool? _customPreview;

    [Header("Subtitles")]
    [SerializeField, Label("Default")] bool _defaultSubtitles;
    bool? _customSubtitles;
}
