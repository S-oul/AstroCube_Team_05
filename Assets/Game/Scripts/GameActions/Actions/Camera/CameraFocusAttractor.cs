using System.Collections;
using UnityEngine;

public class CameraFocusAttractor : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private MouseCamControl cameraControl;
    [SerializeField] private Transform cameraTransform;
    private Transform pointOfInterest;

    private float transitionDuration = 1.0f;
    private float focusDuration = 2.0f;
    private float returnDuration = 0.5f;

    private float strength = 0.5f;

    [Header("Axes affectés")]
    [SerializeField] private bool affectYaw = true;
    [SerializeField] private bool affectPitch = true;

    private Coroutine _focusCoroutine;

    public void StartFocus()
    {
        if (pointOfInterest == null || cameraControl == null || cameraTransform == null)
        {
            Debug.LogWarning("CameraFocusAttractor: Missing reference(s)");
            return;
        }

        if (_focusCoroutine != null)
            StopCoroutine(_focusCoroutine);

        _focusCoroutine = StartCoroutine(FocusRoutine());
    }

    private IEnumerator FocusRoutine()
    {
        float timer = 0f;

        // --- TRANSITION ---
        while (timer < transitionDuration)
        {
            if (pointOfInterest == null)
                yield break;

            Vector3 toTarget = (pointOfInterest.position - cameraTransform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(toTarget);
            Vector3 targetEuler = targetRot.eulerAngles;

            float targetYaw = targetEuler.y;
            float targetPitch = targetEuler.x;

            float currentYaw = cameraControl.PlayerTransformEulerY();
            float newYaw = Mathf.LerpAngle(currentYaw, targetYaw, strength * Time.deltaTime * 10f);

            if (affectYaw)
                cameraControl.SetExternalYaw(newYaw, strength);

            toTarget = (pointOfInterest.position - cameraTransform.position).normalized;
            targetRot = Quaternion.LookRotation(toTarget);
            targetEuler = targetRot.eulerAngles;
            targetPitch = targetEuler.x;

            float currentPitch = cameraTransform.localEulerAngles.x;
            float newPitch = Mathf.LerpAngle(currentPitch, targetPitch, strength * Time.deltaTime * 10f);

            if (affectPitch)
                cameraControl.SetExternalPitch(newPitch, strength);

            timer += Time.deltaTime;
            yield return null;
        }

        // --- MAINTIEN ---
        float hold = 0f;
        while (hold < focusDuration)
        {
            if (pointOfInterest == null)
                break;

            Vector3 toTarget = (pointOfInterest.position - cameraTransform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(toTarget);
            Vector3 targetEuler = targetRot.eulerAngles;

            float targetYaw = targetEuler.y;
            float targetPitch = targetEuler.x;

            float currentYaw = cameraControl.PlayerTransformEulerY();
            float newYaw = Mathf.LerpAngle(currentYaw, targetYaw, strength * Time.deltaTime * 10f);

            if (affectYaw)
                cameraControl.SetExternalYaw(newYaw, strength);

            toTarget = (pointOfInterest.position - cameraTransform.position).normalized;
            targetRot = Quaternion.LookRotation(toTarget);
            targetEuler = targetRot.eulerAngles;
            targetPitch = targetEuler.x;

            float currentPitch = cameraTransform.localEulerAngles.x;
            float newPitch = Mathf.LerpAngle(currentPitch, targetPitch, strength * Time.deltaTime * 10f);

            if (affectPitch)
                cameraControl.SetExternalPitch(newPitch, strength);

            hold += Time.deltaTime;
            yield return null;
        }

        // --- RETOUR ---
        float rt = 0f;
        float initialPitch = cameraTransform.localEulerAngles.x;
        float targetPitchAtEnd = initialPitch;

        float initialYaw = cameraControl.PlayerTransformEulerY();
        float targetYawAtEnd = initialYaw;

        while (rt < returnDuration)
        {
            float p = Mathf.SmoothStep(0f, 1f, rt / returnDuration);

            float newPitch = Mathf.LerpAngle(initialPitch, targetPitchAtEnd, p);

            if (affectPitch)
                cameraControl.SetExternalPitch(newPitch, Mathf.Lerp(strength, 0f, p));

            if (affectYaw)
            {
                cameraControl.SetExternalYaw(targetYawAtEnd, Mathf.Lerp(strength, 0f, p));
            }

            rt += Time.deltaTime;
            yield return null;
        }

        cameraControl.ClearExternalInfluence();
        _focusCoroutine = null;
    }

    public bool IsFocusActive()
    {
        return _focusCoroutine != null;
    }

    public void SetPointOfInterest(Transform newTarget)
    {
        pointOfInterest = newTarget;
    }

    public void SetFocusParameters(float transition, float focus, float strengthValue)
    {
        transitionDuration = transition;
        focusDuration = focus;
        strength = strengthValue;
    }
}
