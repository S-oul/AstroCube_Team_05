using UnityEngine;

public class GameActionStopSequencer : AGameAction
{
    [SerializeField] private GameActionsSequencer _sequencer;

    protected override void ExecuteSpecific()
    {
        if (_sequencer == null) return;
        _sequencer.Stop(false);
    }

    public override string BuildGameObjectName()
    {
        string strSequencer = "[Sequencer]";
        if (_sequencer != null) {
            strSequencer = _sequencer.name;
        }

        return $"STOP SEQUENCER {strSequencer}";
    }
}