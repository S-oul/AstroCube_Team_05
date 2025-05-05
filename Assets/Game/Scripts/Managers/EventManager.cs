using System;
using UnityEngine;
using static UnityEngine.Windows.Speech.PhraseRecognitionSystem;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance => instance;
    private static EventManager instance;

    private GameSettings _gameSettings;

    public static bool gamePaused = false;

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;

        _gameSettings = GameManager.Instance.Settings;
    }

    //Game Events
    public static event Action OnSceneStart;
    public static event Action OnSceneEnd;

    public static event Action OnPlayerWin;
    public static event Action OnPlayerLose;

    public static event Action OnSceneChange;

    public static event Action OnGamePause;
    public static event Action OnGameUnpause;

    public static event Action OnSeeExit;

    //Rubik's Cube Events
    public static event Action OnStartCubeRotation;
    public static event Action OnEndCubeRotation;


    //Object Events
    public static event Action OnButtonPressed;
    public static event Action OnButtonReleased;

    //Player Events
    public static event Action<float> OnPlayerReset;
    public static event Action<float> OnPlayerResetOnce;

    public static event Action OnActivateSequence;
    public static event Action OnEndSequence;

    public static event Action OnStartNarrativeSequence;
    public static event Action OnEndNarrativeSequence;

    public static Delegate[] OnGamePauseCallStack => OnGamePause.GetInvocationList();
    public static Delegate[] OnGameUnpauseCallStack => OnGameUnpause.GetInvocationList();

    public static void TriggerPlayerWin()
    {
        OnPlayerWin?.Invoke();
    }

    public void TriggerPlayerLose()
    {
        Debug.Log("Lose Event Triggered!");
        OnPlayerLose?.Invoke();
        TriggerReset();
    }

    public static void TriggerSceneChange()
    {
        OnSceneChange?.Invoke();
    }

    public static void TriggerGamePause()
    {
        OnGamePause?.Invoke();
        gamePaused = true;
    }

    public static void TriggerGameUnpause()
    {
        OnGameUnpause?.Invoke();
        gamePaused = false;
    }
    public static void TriggerSeeExit()
    {
        OnSeeExit?.Invoke();
    }

    public static void TriggerButtonPressed()
    {
        OnButtonPressed?.Invoke();
    }

    public static void TriggerButtonReleased()
    {
        OnButtonReleased?.Invoke();
    }

    public void TriggerReset()
    {
        OnPlayerReset?.Invoke(_gameSettings.ResetDuration);
    }
    public void TriggerResetOnce()
    {
        OnPlayerResetOnce?.Invoke(_gameSettings.ResetDuration / 4);
    }

    public static void TriggerSceneStart()
    {
        OnSceneStart?.Invoke();
    }

    public static void TriggerSceneEnd()
    {
        OnSceneEnd?.Invoke();
    }

    public static void TriggerNarrativeSequenceStart()
    {
        OnStartNarrativeSequence?.Invoke();
    }

    public static void TriggerNarrativeSequenceEnd()
    {
        OnEndNarrativeSequence?.Invoke();
    }

    public static void TriggerStartCubeRotation()
    {
        OnStartCubeRotation?.Invoke();
    }

    public static void TriggerEndCubeRotation()
    {
        OnEndCubeRotation?.Invoke();
    }

    public static void TriggerActivateCubeSequence()
    {
        OnActivateSequence?.Invoke();
    }    

    public static void TriggerEndCubeSequence()
    {
        OnEndSequence?.Invoke();
    }
}
