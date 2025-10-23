using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSequencerArea : MonoBehaviour
{
    [SerializeField] private GameActionsSequencer _sequencer;
    [SerializeField] private bool _finishBeforeStop;
    private bool _sequencerStopped;
    private void OnTriggerEnter(Collider other) { if (other.CompareTag("Player") && !_sequencerStopped) StopSequencer(); }
    protected virtual void StopSequencer()
    {
        if (_finishBeforeStop) _sequencer.replayOnStop = false;
        else _sequencer.Stop();
        _sequencerStopped = true;
    }
}
