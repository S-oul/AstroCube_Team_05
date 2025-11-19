using UnityEngine;
using System.Collections;
using UnityEditor.Timeline;
using UnityEditor.UIElements;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeAmount = 0.7f;
    [SerializeField] AnimationCurve intencityCurve;

    Transform camTransform;
    Vector3 originalPos;
    float shakeDurationHolder = 0;
    float decreaseFactor = 1.0f;

    void Awake()
    {
        camTransform = transform;
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;

        EventManager.OnEndCubeRotation += TriggerCameraShake;
    }

    private void OnDisable()
    {
        EventManager.OnEndCubeRotation -= TriggerCameraShake;
    }

    void Update()
    {
        if (shakeDurationHolder > 0)
        {
            float tempShakeAmount = intencityCurve.Evaluate(1 - shakeDurationHolder/ shakeDuration) * shakeAmount;
            camTransform.localPosition = originalPos + Random.insideUnitSphere * tempShakeAmount;

            shakeDurationHolder -= Time.deltaTime * decreaseFactor;
            Debug.Log(shakeDurationHolder);
        }
        else
        {
            shakeDurationHolder = 0f;
            camTransform.localPosition = originalPos;
        }
    }

    private void TriggerCameraShake()
    {
        shakeDurationHolder += shakeDuration;
    }
}