using DG.Tweening;
using NaughtyAttributes;
using RubiksStatic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionMoveRubiksCube : AGameAction
{

    [SerializeField] private RubiksMove _move = new();
    [SerializeField] private RubiksMovement _RubiksScript;
    [SerializeField] private RubiksCubeController _RubiksControllerScript;
    [SerializeField] private float _rotationDuration;
    private bool _isTurning = false;

    protected override void ExecuteSpecific()
    {
        StartCoroutine(DoMove());
    }

    private IEnumerator DoMove()
    {
        _isTurning = true;
        if (_RubiksControllerScript)
        {
            if (_RubiksControllerScript.IsPlayerOnTile(_move.orientation, _move.cube))
                yield break;
        }
        yield return _RubiksScript.RotateAxisCoroutine(_move.axis, _move.cube, _move.clockWise, _rotationDuration);
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
