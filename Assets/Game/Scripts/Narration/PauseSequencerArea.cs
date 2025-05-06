using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSequencerArea : MonoBehaviour
{
    [SerializeField] protected GameActionsSequencer _sequencer;
    protected bool _paused;

    private void OnTriggerEnter(Collider other) { if (other.CompareTag("Player") && _sequencer.IsRunning()) PauseSequencer(); }
    protected virtual void PauseSequencer()
    {
        _sequencer.Stop();
        _sequencer.replayOnStop = false;
        _paused = true;
    }

    protected virtual void OnTriggerExit(Collider other) { if (other.CompareTag("Player") && _paused) ResumeSequencer(); }
    protected virtual void ResumeSequencer()
    {
        _paused = false;
        _sequencer.replayOnStop = true;
        if (!_sequencer.IsRunning()) _sequencer.Play();
    }
}
