using UnityEngine;

public class GameActionFocusCamera : AGameAction
{
    [Header("Camera Focus")]
    [SerializeField] private CameraFocusAttractor _cameraFocusAttractor;

    [SerializeField] private Transform _targetToLookAt;

    [SerializeField] private float focusStrength = 0.7f;

    [SerializeField] private float transitionDuration = 1.0f;

    [SerializeField] private float focusDuration = 2.0f;

    protected override void ExecuteSpecific()
    {
        if (_cameraFocusAttractor == null || _targetToLookAt == null)
        {
            Debug.LogWarning("GameActionFocusCamera: Missing references!");
            return;
        }

        _cameraFocusAttractor.SetPointOfInterest(_targetToLookAt);
        _cameraFocusAttractor.SetFocusParameters(transitionDuration, focusDuration, focusStrength);

        _cameraFocusAttractor.StartFocus();
    }

    public override string BuildGameObjectName()
    {
        string targetName = _targetToLookAt != null ? _targetToLookAt.name : "[NONE]";
        return $"FOCUS CAM > {targetName}";
    }

    protected override bool IsFinishedSpecific()
    {
        if (_cameraFocusAttractor == null)
            return true;

        return !_cameraFocusAttractor.IsFocusActive();
    }
}
