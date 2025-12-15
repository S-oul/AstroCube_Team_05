using System;
using System.Collections.Generic;
using UnityEngine;

public class SequencerSkipper : MonoBehaviour
{
    [SerializeField] private List<AGameAction> _gameActionsToSkip = new();
    [SerializeField] private List<GameObject> _gameObjectsToDeactivate = new();
    [SerializeField] private List<GameActionsSequencer> _sequencersToStart = new();
    
    private float _skipDelay;
    private bool _skipState;
    private bool _isSkipped;

    private void OnEnable()
    {
        EventManager.OnSkipCutscene += SetSkipCutsceneState;
    }

    private void OnDisable()
    {
        EventManager.OnSkipCutscene -= SetSkipCutsceneState;
    }

    private void SetSkipCutsceneState(bool state)
    {
        _skipState = state;
    }

    private void SkipCutscene()
    {
        _isSkipped = true;

        AUDIO_ProgrammerInstrument.Instance.Cancel();
        LocalizationManager.Instance.ClearString();
        
        foreach (AGameAction gameAction in _gameActionsToSkip)
        {
            gameAction.gameObject.SetActive(false);
        }
        
        foreach (GameObject obj in _gameObjectsToDeactivate)
        {
            obj.SetActive(false);
        }
        
        foreach (GameActionsSequencer sequencer in _sequencersToStart)
        {
            sequencer.Play();
        }
    }
    
    private void Update()
    {
        if (_isSkipped)
            return;
        
        if (_skipState)
            _skipDelay += Time.deltaTime;
        else
            _skipDelay = 0.0f;
        LocalizationManager.Instance.SetSkipValue(_skipDelay / GameManager.Instance.Settings.SkipCutsceneDuration);
        
        if(_skipDelay >= GameManager.Instance.Settings.SkipCutsceneDuration)
            SkipCutscene();
    }
}
