using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSequencerArea : PauseSequencerArea
{
    [SerializeField] private GameActionsSequencer _other;
    protected override void PauseSequencer()
    {
        base.PauseSequencer();
        _other.replayOnStop = true;
        _other.Play();
    }

    protected override void ResumeSequencer()
    {
        if (_other.IsRunning())
        {
            _other.Stop();
            _other.replayOnStop = false;
            base.ResumeSequencer();
        }
        else _paused = false;
    }

}
