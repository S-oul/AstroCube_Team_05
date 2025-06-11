using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusAttractor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MouseCamControl _cameraControl;
    [SerializeField] private Transform _cameraTransform;

    [Header("Affected Axes")]
    [SerializeField] private bool _affectYaw = true;
    [SerializeField] private bool _affectPitch = true;

    private List<Coroutine> _activeCoroutines = new List<Coroutine>();
    private CameraFocusParameters _continuousFocusParam;
    private bool _isContinuousFocusActive = false;

    private void Update()
    {
        if (_isContinuousFocusActive && _continuousFocusParam != null && _continuousFocusParam.PointOfInterest != null)
        {
            Vector3 toTarget = (_continuousFocusParam.PointOfInterest.position - _cameraTransform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(toTarget);
            Vector3 targetEuler = targetRot.eulerAngles;

            float targetYaw = targetEuler.y;
            float targetPitch = targetEuler.x;

            float currentYaw = _cameraControl.PlayerTransformEulerY();
            float newYaw = Mathf.LerpAngle(currentYaw, targetYaw, _continuousFocusParam.Strength * Time.deltaTime * 10f);

            if (_affectYaw)
                _cameraControl.SetExternalYaw(newYaw, _continuousFocusParam.Strength);

            float currentPitch = _cameraTransform.localEulerAngles.x;
            float newPitch = Mathf.LerpAngle(currentPitch, targetPitch, _continuousFocusParam.Strength * Time.deltaTime * 10f);

            if (_affectPitch)
                _cameraControl.SetExternalPitch(newPitch, _continuousFocusParam.Strength);
        }
    }

    public void StartFocus(CameraFocusParameters param)
    {
        if (param.PointOfInterest == null || _cameraControl == null || _cameraTransform == null)
        {
            Debug.LogWarning("CameraFocusAttractor: Missing reference(s)");
            return;
        }

        Coroutine routine = StartCoroutine(FocusRoutine(param));
        _activeCoroutines.Add(routine);
    }

    public void StartContinuousFocus(CameraFocusParameters param)
    {
        if (param.PointOfInterest == null || _cameraControl == null || _cameraTransform == null)
        {
            Debug.LogWarning("CameraFocusAttractor: Missing reference(s)");
            return;
        }

        _continuousFocusParam = param;
        _isContinuousFocusActive = true;
    }

    public void StopFocus()
    {
        StopAllFocus();
    }

    public void StopAllFocus()
    {
        foreach (var coroutine in _activeCoroutines)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }

        _activeCoroutines.Clear();
        _isContinuousFocusActive = false;
        _continuousFocusParam = null;
        _cameraControl.ClearExternalInfluence();
    }

    private IEnumerator FocusRoutine(CameraFocusParameters param)
    {
        float timer = 0f;

        // --- TRANSITION ---
        if (param.DoIn)
        {
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
        }

        // --- MAINTIEN ---
        if (param.DoStay)
        {
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
        }

        // --- RETOUR ---
        if (param.DoOut)
        {
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
                    _cameraControl.SetExternalYaw(targetYawAtEnd, Mathf.Lerp(param.Strength, 0f, p));

                rt += Time.deltaTime;
                yield return null;
            }
        }

        _cameraControl.ClearExternalInfluence();
    }

    public bool IsFocusActive()
    {
        return _activeCoroutines.Count > 0 || _isContinuousFocusActive;
    }

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
            _doIn = true;
            _doStay = true;
            _doOut = true;
        }

        public Transform PointOfInterest { get => _pointOfInterest; set => _pointOfInterest = value; }
        public float InDuration { get => _inDuration; set => _inDuration = value; }
        public float FocusDuration { get => _focusDuration; set => _focusDuration = value; }
        public float ReturnDuration { get => _returnDuration; set => _returnDuration = value; }
        public float Strength { get => _strength; set => _strength = value; }
        public bool DoIn { get => _doIn; set => _doIn = value; }
        public bool DoStay { get => _doStay; set => _doStay = value; }
        public bool DoOut { get => _doOut; set => _doOut = value; }

        [Header("References")]
        [SerializeField] private Transform _pointOfInterest;

        [Header("Parameters")]
        [SerializeField] private float _inDuration;
        [SerializeField] private float _focusDuration;
        [SerializeField] private float _returnDuration;
        [SerializeField] private float _strength;

        [Header("Actions")]
        [SerializeField] private bool _doIn;
        [SerializeField] private bool _doStay;
        [SerializeField] private bool _doOut;
    }
}
