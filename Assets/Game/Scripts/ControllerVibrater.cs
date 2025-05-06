using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerVibrater : MonoBehaviour
{
    [SerializeField] CustomisedSettings customSettings;
    [SerializeField] float _minimumFrequency;
    [SerializeField] float _maxumumFrequency;
    [SerializeField] float _duration;
        
    Gamepad _gamePad;
    Coroutine _stopVibrationCoroutine;
    bool _vibrationIsActive = true;

    private void OnEnable()
    {
        EventManager.OnStartCubeRotation += CubeRotationVibrate;
        EventManager.OnVibrationChange += SetVibrationIsActive;
    }

    private void OnDisable()
    {
        EventManager.OnStartCubeRotation -= CubeRotationVibrate;
        EventManager.OnVibrationChange -= SetVibrationIsActive;
    }

    private void Start()
    {
        _vibrationIsActive = customSettings.customVibration;
    }

    void SetVibrationIsActive(bool isActive)
    {
        _vibrationIsActive = isActive;
    }

    void CubeRotationVibrate()
    {
        if (_vibrationIsActive == false) { return; }
        VibrateController(_minimumFrequency, _maxumumFrequency, _duration);
    }

    void VibrateController(float lowFreq, float hightFreq, float duration)
    {
        _gamePad = Gamepad.current;

        if (_gamePad == null ) return;

        _gamePad.SetMotorSpeeds(lowFreq, hightFreq);

        _stopVibrationCoroutine = StartCoroutine(StopVibration(duration, _gamePad));

    }

    IEnumerator StopVibration(float duration, Gamepad gamepad)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gamepad.SetMotorSpeeds(0, 0);
    }
}
