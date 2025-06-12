using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ApplyCustomCameraSettings : MonoBehaviour
{
    Camera _vol;
    MotionBlur _motionBlur;

    private void Start()
    {
        _motionBlur = GetComponent<MotionBlur>();
    }

    private void OnEnable()
    {
        EventManager.OnMotionBlurChange += SetMotionBlur;
    }
    private void OnDisable()
    {
        EventManager.OnMotionBlurChange -= SetMotionBlur;
    }

    void SetMotionBlur(bool isActive)
    {
        if (_motionBlur == null)
        {
            Debug.Log("No Motion Blur component found in camera");
            return;
        }
        _motionBlur.active = isActive;
        Debug.Log("Motion Blur changed");
    }

}
