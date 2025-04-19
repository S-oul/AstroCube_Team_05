using System;
using UnityEngine;

public class GameScriptPlayerTriggerArea : MonoBehaviour
{
    public Action OnTriggerEnterDelegate;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        OnTriggerEnterDelegate?.Invoke();
    }
}