using System;
using System.Collections;
using UnityEngine;

public class CameraFocusAttractor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MouseCamControl _cameraControl;
    [SerializeField] private Transform _cameraTransform;

    [Header("Affected Axes")]
    [SerializeField] private bool _affectYaw = true;
    [SerializeField] private bool _affectPitch = true;

    private Coroutine _focusCoroutine;

    public void StartFocus(CameraFocusParameters param)
    {
        if (param.PointOfInterest == null || _cameraControl == null || _cameraTransform == null)
        {
            Debug.LogWarning("CameraFocusAttractor: Missing reference(s)");
            return;
        }

        if (_focusCoroutine != null)
            StopCoroutine(_focusCoroutine);

        _focusCoroutine = StartCoroutine(FocusRoutine(param));
    }

    private IEnumerator FocusRoutine(CameraFocusParameters param)
    {
        float timer = 0f;

        // --- TRANSITION ---
        while (timer < param.InDuration)
        {
            if (param.PointOfInterest == null)
                yield break;

            Vector3 toTarget = (param.PointOfInterest.position - _cameraTransform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(toTarget);
            Vector3 targetEuler = targetRot.eulerAngles;

            float targetYaw = targetEuler.y;
            float targetPitch = targetEuler.x;

            float currentYaw = _cameraControl.PlayerTransformEulerY();
            float newYaw = Mathf.LerpAngle(currentYaw, targetYaw, param.Strength * Time.deltaTime * 10f);

            if (_affectYaw)
                _cameraControl.SetExternalYaw(newYaw, param.Strength);

            toTarget = (param.PointOfInterest.position - _cameraTransform.position).normalized;
            targetRot = Quaternion.LookRotation(toTarget);
            targetEuler = targetRot.eulerAngles;
            targetPitch = targetEuler.x;

            float currentPitch = _cameraTransform.localEulerAngles.x;
            float newPitch = Mathf.LerpAngle(currentPitch, targetPitch, param.Strength * Time.deltaTime * 10f);

            if (_affectPitch)
                _cameraControl.SetExternalPitch(newPitch, param.Strength);

            timer += Time.deltaTime;
            yield return null;
        }

        // --- MAINTIEN ---
        float hold = 0f;
        while (hold < param.FocusDuration)
        {
            if (param.PointOfInterest == null)
                break;

            Vector3 toTarget = (param.PointOfInterest.position - _cameraTransform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(toTarget);
            Vector3 targetEuler = targetRot.eulerAngles;

            float targetYaw = targetEuler.y;
            float targetPitch = targetEuler.x;

            float currentYaw = _cameraControl.PlayerTransformEulerY();
            float newYaw = Mathf.LerpAngle(currentYaw, targetYaw, param.Strength * Time.deltaTime * 10f);

            if (_affectYaw)
                _cameraControl.SetExternalYaw(newYaw, param.Strength);

            toTarget = (param.PointOfInterest.position - _cameraTransform.position).normalized;
            targetRot = Quaternion.LookRotation(toTarget);
            targetEuler = targetRot.eulerAngles;
            targetPitch = targetEuler.x;

            float currentPitch = _cameraTransform.localEulerAngles.x;
            float newPitch = Mathf.LerpAngle(currentPitch, targetPitch, param.Strength * Time.deltaTime * 10f);

            if (_affectPitch)
                _cameraControl.SetExternalPitch(newPitch, param.Strength);

            hold += Time.deltaTime;
            yield return null;
        }

        // --- RETOUR ---
        float rt = 0f;
        float initialPitch = _cameraTransform.localEulerAngles.x;
        float targetPitchAtEnd = initialPitch;

        float initialYaw = _cameraControl.PlayerTransformEulerY();
        float targetYawAtEnd = initialYaw;

        while (rt < param.ReturnDuration)
        {
            float p = Mathf.SmoothStep(0f, 1f, rt / param.ReturnDuration);

            float newPitch = Mathf.LerpAngle(initialPitch, targetPitchAtEnd, p);

            if (_affectPitch)
                _cameraControl.SetExternalPitch(newPitch, Mathf.Lerp(param.Strength, 0f, p));

            if (_affectYaw)
            {
                _cameraControl.SetExternalYaw(targetYawAtEnd, Mathf.Lerp(param.Strength, 0f, p));
            }

            rt += Time.deltaTime;
            yield return null;
        }

        _cameraControl.ClearExternalInfluence();
        _focusCoroutine = null;
    }

    public bool IsFocusActive()
    {
        return _focusCoroutine != null;
    }

    /*
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
    */

    [Serializable]
    public class CameraFocusParameters
    {
        public CameraFocusParameters(float inDuration, float focusDuration, float strength)
        {
            _inDuration = inDuration;
            _focusDuration = focusDuration;
            _strength = strength;
            _returnDuration = 0.5f;
        }        
        public CameraFocusParameters()
        {            
            _inDuration = 1.0f;
            _focusDuration = 2.0f;
            _returnDuration = 0.5f;
            _strength = 0.5f;
        }

        public Transform PointOfInterest { get => _pointOfInterest; set => _pointOfInterest = value; }
        public float InDuration { get => _inDuration; set => _inDuration = value; }
        public float FocusDuration { get => _focusDuration; set => _focusDuration = value; }
        public float ReturnDuration { get => _returnDuration; set => _returnDuration = value; }
        public float Strength { get => _strength; set => _strength = value; }

        [Header("References")]
        [SerializeField] private Transform _pointOfInterest;

        [Header("Parameters")]
        [SerializeField] private float _inDuration;
        [SerializeField] private float _focusDuration;
        [SerializeField] private float _returnDuration;
        [SerializeField] private float _strength;
    }
}
