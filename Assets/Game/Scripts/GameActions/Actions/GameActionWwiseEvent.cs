using UnityEngine;
using FMODUnity;

public class GameActionWwiseEvent : AGameAction
{
    [SerializeField] private EventReference _fmodEvent;
    [SerializeField] private GameObject _targetGameObject;

    protected override void ExecuteSpecific()
    {
        if (_fmodEvent.IsNull) return;
        if (_targetGameObject == null) {
            _targetGameObject = gameObject;
        }

        RuntimeManager.PlayOneShotAttached(_fmodEvent, _targetGameObject);
    }

    public override string BuildGameObjectName()
    {
        string strFmodEvent = "[Event]";
        if (!_fmodEvent.IsNull) {
            strFmodEvent = _fmodEvent.Path;
        }

        return $"PLAY FMOD EVENT {strFmodEvent}";
    }
}