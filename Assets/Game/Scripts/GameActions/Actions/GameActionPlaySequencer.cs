using UnityEngine;

public class GameActionPlaySequencer : AGameAction
{
    [SerializeField] private GameActionsSequencer _sequencer;

    protected override void ExecuteSpecific()
    {
        if (_sequencer == null) return;
        _sequencer.Play();
    }

    public override string BuildGameObjectName()
    {
        string strSequencer = "[Sequencer]";
        if (_sequencer != null) {
            strSequencer = _sequencer.name;
        }

        return $"PLAY SEQUENCER {strSequencer}";
    }
}