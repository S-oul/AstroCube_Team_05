using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameActionCallFunction : AGameAction
{
    [SerializeField] private UnityEvent _events;

    public override string BuildGameObjectName()
    {
        return "CALL FUNCTIONS";

    }

    protected override void ExecuteSpecific()
    {
        _events?.Invoke();
    }

}
