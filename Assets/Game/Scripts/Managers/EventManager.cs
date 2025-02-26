using NaughtyAttributes;
using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private void Awake()
    {
        if(Instance) Destroy(this);
        else Instance = this;
    }


    public static event Action OnPlayerWin;
    public static event Action OnPlayerLose;

    public static event Action OnPlayerReset;

    public static void TriggerPlayerWin()
    {
        OnPlayerWin?.Invoke();
    }

    public static void TriggerPlayerLose()
    {
        OnPlayerLose?.Invoke();
    }

    [Button]
    public static void TriggerReset()
    {
        OnPlayerReset?.Invoke();
    }
}
