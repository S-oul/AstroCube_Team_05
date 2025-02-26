using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action OnPlayerWin;
    public static event Action OnPlayerLose;

    public static void TriggerPlayerWin()
    {
        OnPlayerWin?.Invoke();
    }

    public static void TriggerPlayerLose()
    {
        OnPlayerLose?.Invoke();
    }
}
