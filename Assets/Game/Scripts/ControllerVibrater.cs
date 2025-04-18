using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerVibrater : MonoBehaviour
{
    [SerializeField] float _minimumFrequency;
    [SerializeField] float _maxumumFrequency;
    [SerializeField] float _duration;
    
    Gamepad _gamePad;
    Coroutine _stopVibrationCoroutine;

    private void OnEnable()
    {
        EventManager.OnStartCubeRotation += CubeRotationVibrate;
    }

    private void OnDisable()
    {
        EventManager.OnStartCubeRotation -= CubeRotationVibrate;
    }

    void CubeRotationVibrate()
    {
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
