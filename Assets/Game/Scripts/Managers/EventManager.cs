using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action OnPlayerWin;
    public static event Action OnPlayerLose;
    public static event Action OnButtonPressed;
    public static event Action OnButtonReleased;

    public static void TriggerPlayerWin()
    {
        OnPlayerWin?.Invoke();
    }

    public static void TriggerPlayerLose()
    {
        OnPlayerLose?.Invoke();
    }

    public static void TriggerButtonPressed()
    {
        OnButtonPressed?.Invoke();
    }

    public static void TriggerButtonReleased()
    {
        OnButtonReleased?.Invoke();
    }
}
