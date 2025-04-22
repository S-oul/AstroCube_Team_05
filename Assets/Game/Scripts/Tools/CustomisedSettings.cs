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
    public float? customFov { 
        get
        {
            if (customFov == null) return defaultFOV;
            else return customFov;
        } 
        set
        {
            if (customFov < minFOV)
            {
                customFov = minFOV;
            }
            else if (customFov > maxFOV)
            {
                customFov = maxFOV;
            }
            else { customFov = value; }
        }
    }

    public float defaultMouse => _defaultMouse;
    public float minMouse => _minimumMouse;
    public float maxMouse => _maximumMouse;
    public float? customMouse
    {
        get
        {
            if (customMouse == null) return defaultMouse;
            else return customMouse;
        }
        set
        {
            if (customMouse < minMouse)
            {
                customMouse = minMouse;
            }
            else if (customMouse > maxMouse)
            {
                customMouse = maxMouse;
            }
            else { customMouse = value; }
        }
    }

    public float defaultJoystick => _defaultJoystick;
    public float minJoystick => _minimumJoystick;
    public float maxJoystick => _maximumJoystick;
    public float? customJoystick
    {
        get
        {
            if (customJoystick == null) return defaultJoystick;
            else return customJoystick;
        }
        set
        {
            if (customJoystick < minJoystick)
            {
                customJoystick = minJoystick;
            }
            else if (customJoystick > maxJoystick)
            {
                customJoystick = maxJoystick;
            }
            else { customJoystick = value; }
        }
    }

    public float defaultVolume => _defaultVolume;
    public float minVolume => _minimumVolume;
    public float maxVolume => _maximumVolume;
    public float? customVolume
    {
        get
        {
            if (customVolume == null) return defaultVolume;
            else return customVolume;
        }
        set
        {
            if (customVolume < minVolume)
            {
                customVolume = minVolume;
            }
            else if (customVolume > maxVolume)
            {
                customVolume = maxVolume;
            }
            else { customVolume = value; }
        }
    }

    public bool defaultVibration => _defaultVibration;
    public bool? customVibration {
        get
        {
            if (customVibration == null) return defaultVibration;
            else return customVibration;
        }
        set { customVibration = value; }
    }

    public bool defaultPreview => _defaultPreview;
    public bool? customPreview
    {
        get
        {
            if (customPreview == null) return defaultPreview;
            else return customPreview;
        }
        set { customPreview = value; }
    }

    public bool defaultSubtitles => _defaultSubtitles;
    public bool? customSubtitles
    {
        get
        {
            if (customSubtitles == null) return defaultSubtitles;
            else return customSubtitles;
        }
        set { customSubtitles = value; }
    }

    [Header("FOV")]

    [SerializeField, Label("Default")] float _defaultFOV;
    [SerializeField, Label("Minimum")] float _minimumFOV;
    [SerializeField, Label("Maximum")] float _maximumFOV;    
    
    [Header("MouseSensibility")]

    [SerializeField, Label("Default")] float _defaultMouse;
    [SerializeField, Label("Minimum")] float _minimumMouse;
    [SerializeField, Label("Maximum")] float _maximumMouse;

    [Header("JoystickSensibility")]

    [SerializeField, Label("Default")] float _defaultJoystick;
    [SerializeField, Label("Minimum")] float _minimumJoystick;
    [SerializeField, Label("Maximum")] float _maximumJoystick;

    [Header("Volume")]

    [SerializeField, Label("Default")] float _defaultVolume;
    [SerializeField, Label("Minimum")] float _minimumVolume;
    [SerializeField, Label("Maximum")] float _maximumVolume;

    [Header("Vibration")]
    [SerializeField, Label("Default")] bool _defaultVibration;

    [Header("Preview")]
    [SerializeField, Label("Default")] bool _defaultPreview;

    [Header("Subtitles")]
    [SerializeField, Label("Default")] bool _defaultSubtitles;


}
