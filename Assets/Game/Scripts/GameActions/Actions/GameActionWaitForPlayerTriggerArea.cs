using UnityEngine;

public class GameActionWaitForPlayerTriggerArea : AGameAction
{
    [SerializeField] private GameScriptPlayerTriggerArea _triggerArea;

    private bool _areaTrigerred = false;

    protected override void ExecuteSpecific()
    {
        _areaTrigerred = false;
        _triggerArea.OnTriggerEnterDelegate += OnAreaTriggered;
    }

    private void OnAreaTriggered()
    {
        _triggerArea.OnTriggerEnterDelegate -= OnAreaTriggered;
        _areaTrigerred = true;
    }

    protected override bool IsFinishedSpecific()
    {
        return _areaTrigerred;
    }

    public override string BuildGameObjectName()
    {
        string strTriggerArea = "[TriggerArea]";
        if (_triggerArea != null) {
            strTriggerArea = _triggerArea.name;
        }

        return $"WAIT FOR PLAYER INSIDE {strTriggerArea}";
    }
}