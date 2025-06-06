using System.Collections.Generic;
using UnityEngine;

public class GameActionsSequencer : MonoBehaviour
{
    private List<AGameAction> _gameActionsList;
    private int _currentGameActionIndex = 0;

    private bool _isRunning = false;

    [Header("AutoPlay")]
    public bool autoPlay = false;

    [Header(("Replay On Stop"))]
    public bool replayOnStop = false;

    private void Awake()
    {
        _gameActionsList = new List<AGameAction>(GetComponentsInChildren<AGameAction>());
    }

    private void Start()
    {
        if (autoPlay) {
            Play();
        }
    }

    private void Update()
    {
        _UpdateActions();
    }

    private void _UpdateActions()
    {
        if (!_isRunning) return;

        float dt = Time.deltaTime;
        bool allActionsFinished = false;
        AGameAction currentAction = _gameActionsList[_currentGameActionIndex];
        currentAction.ActionUpdate(dt);
        while (!allActionsFinished && currentAction.IsFinished()) 
        {
            currentAction.End();
            _currentGameActionIndex++;
            if (_currentGameActionIndex >= _gameActionsList.Count) allActionsFinished = true;
            else 
            {
                //print("Executing action " + currentAction.name + " at index " + _currentGameActionIndex + " of sequencer " + name);
                currentAction = _gameActionsList[_currentGameActionIndex];
                currentAction.Execute();
            }
        }

        if (allActionsFinished)
        {
            //print("Finished sequencer : " + name);
            Stop(true);
        }
    }

    public void Play()
    {
        if (_isRunning) return;
        if (_gameActionsList.Count == 0) return;
        //print("Playing sequencer : " + name);
        _currentGameActionIndex = 0;
        _isRunning = true;
        _gameActionsList[_currentGameActionIndex].Execute();
        _UpdateActions();
    }

    public void Stop(bool withAutoReplay = false)
    {
        if (!_isRunning) return;
        _currentGameActionIndex = 0;
        _isRunning = false;
        if (withAutoReplay && replayOnStop) {
            Play();
        }
        //else print("Stopped sequencer : " + name);
    }

    public bool IsRunning()
    {
        return _isRunning;
    }

    public bool IsEmpty()
    {
        return _gameActionsList.Count == 0;
    }
}