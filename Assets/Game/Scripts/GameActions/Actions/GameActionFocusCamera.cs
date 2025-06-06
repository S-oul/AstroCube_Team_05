using UnityEngine;

public class GameActionFocusCamera : AGameAction
{
    [SerializeField] private CameraFocusAttractor _controller;
    [SerializeField] private CameraFocusAttractor.CameraFocusParameters _parameters;

    protected override void ExecuteSpecific()
    {
        if (_controller == null || _parameters == null || (_parameters != null && _parameters.PointOfInterest == null))
        {
            Debug.LogWarning("GameActionFocusCamera: Missing references!");
            return;
        }

        _controller.StartFocus(_parameters);
    }

    protected override bool IsFinishedSpecific()
    {
        if (_controller == null)
            return true;

        return !_controller.IsFocusActive();
    }

    public override string BuildGameObjectName()
    {
        string strObject = "";
        if (_parameters != null)
        {
            if (_parameters.PointOfInterest != null)
                strObject = _parameters.PointOfInterest.gameObject.gameObject.name;
        }
        return $"Camera Focus on : {strObject}";
    }


}
