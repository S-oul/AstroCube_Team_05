using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySequencerArea : MonoBehaviour
{
    [SerializeField] private GameActionsSequencer _sequencer;
    [SerializeField] private bool _onEnter;
    [SerializeField] private bool _onExit;
    [SerializeField] private bool _once;
    private bool _sequencerStarted;

    private void OnTriggerEnter(Collider other) { if (other.CompareTag("Player") && _onEnter && !(_sequencerStarted && _once)) PlaySequencer(); }
    private void OnTriggerExit(Collider other) { if (other.CompareTag("Player") && _onExit && !(_sequencerStarted && _once)) PlaySequencer(); }
    protected virtual void PlaySequencer()
    {
        _sequencer.Play();
        _sequencerStarted = true;
    }
}
