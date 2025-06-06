using DG.Tweening;
using NaughtyAttributes;
using RubiksStatic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameActionMoveRubiksCube : AGameAction
{

    [SerializeField] private RubiksMove _move = new();
    [SerializeField] private RubiksMovement _rubiksScript;
    [SerializeField] private RubiksCubeController _rubiksControllerScript;
    [SerializeField] private float _rotationDuration;
    [SerializeField] private bool _stopWhenPlayerOnTile = false;
    private bool _isTurning = false;

    [SerializeField] private UnityEvent _OnRotation;
    protected override void ExecuteSpecific()
    {
        StartCoroutine(DoMove());
    }

    private IEnumerator DoMove()
    {
        _isTurning = true;
        if (_rubiksControllerScript)
        {
            if (_rubiksControllerScript.IsPlayerOnTile(_move.orientation, _move.cube) && _stopWhenPlayerOnTile)
                yield break;
        }
        _OnRotation.Invoke();
        yield return _rubiksScript.RotateAxisCoroutine(_move.axis, _move.cube, _move.clockWise, _rotationDuration);
        _isTurning = false;
    }

    protected override bool IsFinishedSpecific()
    {
        return _isTurning;
    }

    public override string BuildGameObjectName()
    {
        string name = "MOVE RUBIKSCUBE ";
        if (_move.cube != null)
            name += $"AT AXIS {_move.axis} OF CUBE {_move.cube.name}";
        return name;
    }
}
