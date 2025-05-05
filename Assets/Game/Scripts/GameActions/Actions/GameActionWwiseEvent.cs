using UnityEngine;

public class GameActionWwiseEvent : AGameAction
{
    [SerializeField] private AK.Wwise.Event _wwiseEvent;
    [SerializeField] private GameObject _targetGameObject;

    protected override void ExecuteSpecific()
    {
        if (_wwiseEvent == null) return;
        if (_targetGameObject == null) {
            _targetGameObject = gameObject;
        }

        _wwiseEvent.Post(_targetGameObject);
    }

    public override string BuildGameObjectName()
    {
        string strWwiseEvent = "[Event]";
        if (_wwiseEvent != null) {
            strWwiseEvent = _wwiseEvent.Name;
        }

        return $"PLAY WWISE EVENT {strWwiseEvent}";
    }
}