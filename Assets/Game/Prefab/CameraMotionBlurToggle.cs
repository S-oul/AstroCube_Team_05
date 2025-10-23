using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraMotionBlurToggle : MonoBehaviour
{
    [SerializeField] Volume _volume;
    MotionBlur _motionBlur;

    private void Awake()
    {
        if (_volume.profile.TryGet(out _motionBlur))
        {
            // pass
        }
        else
        {
            Debug.LogWarning("Motion Blur not found in the volume profile!");
        }
    }

    private void OnEnable()
    {
        EventManager.OnMotionBlurChange += SetMotionBlur;
    }
    private void OnDisable()
    {
        EventManager.OnMotionBlurChange -= SetMotionBlur;
    }

    public void SetMotionBlur(bool enabled)
    {
        if (_motionBlur == null) return;
        _motionBlur.active = enabled;
    }
}
