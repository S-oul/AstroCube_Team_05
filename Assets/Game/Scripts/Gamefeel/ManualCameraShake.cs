using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float bigShakeDuration;
    [SerializeField] float bigShakeAmount = 0.7f;    
    [SerializeField] float smalShakeDuration;
    [SerializeField] float smalShakeAmount = 0.7f;
    [SerializeField] AnimationCurve intencityCurve;

    Transform camTransform;
    Vector3 originalPos;
    float bigShakeDurationHolder = 0;
    float smalShakeDurationHolder = 0;
    float decreaseFactor = 1.0f;

    void Awake()
    {
        camTransform = transform;
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;

        EventManager.OnEndCubeRotation += TriggerBigCameraShake;
        //EventManager.OnPlayerStopsFalling += TriggerSmalCameraShake;
    }

    private void OnDisable()
    {
        EventManager.OnEndCubeRotation -= TriggerBigCameraShake;
        //EventManager.OnPlayerStopsFalling -= TriggerSmalCameraShake;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        if (bigShakeDurationHolder > 0)
        {
            float tempShakeAmount = intencityCurve.Evaluate(1 - bigShakeDurationHolder/ bigShakeDuration) * bigShakeAmount;
            camTransform.localPosition = originalPos + Random.insideUnitSphere * tempShakeAmount;

            bigShakeDurationHolder -= Time.deltaTime * decreaseFactor;

            if (bigShakeDurationHolder > 0) smalShakeDurationHolder -= Time.deltaTime * decreaseFactor; //big shake overwrites smal shake

        }
        else if (smalShakeDurationHolder > 0)
        {
            float tempShakeAmount = intencityCurve.Evaluate(1 - smalShakeDurationHolder / smalShakeDuration) * smalShakeAmount;
            camTransform.localPosition = originalPos + Random.insideUnitSphere * tempShakeAmount;

            smalShakeDurationHolder -= Time.deltaTime * decreaseFactor;

        }
        else
        {
            bigShakeDurationHolder = 0f;
            camTransform.localPosition = originalPos;
        }
    }

    private void TriggerBigCameraShake()
    {
        bigShakeDurationHolder += bigShakeDuration;
        bigShakeDurationHolder = Mathf.Clamp(bigShakeDurationHolder, 0, 1);
    }

    private void TriggerSmalCameraShake()
    {
        smalShakeDurationHolder += smalShakeDuration;
    }
}