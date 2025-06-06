using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionTransformChange : AGameAction
{
    [SerializeField] GameObject _targetObject;
    [Header("Move Object")]
    [SerializeField] bool _moveObject;
    [SerializeField, ShowIf("_moveObject")] Vector3 _targetPosition = new();
    [SerializeField, ShowIf("_moveObject")] float _moveDuration = 1f;
    [SerializeField, ShowIf("_moveObject")] Ease _moveEase = Ease.Linear;
    [SerializeField, ShowIf("_moveObject")] bool _isLocal = false;

    [Header("Rotate Object")]
    [SerializeField] bool _rotateObject;
    [SerializeField, ShowIf("_rotateObject")] Vector3 _targetRotation = new();
    [SerializeField, ShowIf("_rotateObject")] float _rotationDuration = 1f;
    [SerializeField, ShowIf("_rotateObject")] Ease _rotationEase = Ease.Linear;

    private Tweener _moveTweener;
    private Tweener _rotationTweener;

    protected override void ExecuteSpecific()
    {
        if (_moveObject)
        {
            if(_isLocal)
                _moveTweener = _targetObject.transform.DOLocalMove(_targetPosition, _moveDuration).SetEase(_moveEase);
            else
                _moveTweener = _targetObject.transform.DOMove(_targetPosition, _moveDuration).SetEase(_moveEase);
        }
        if (_rotateObject)
            _rotationTweener = _targetObject.transform.DORotate(_targetRotation, _rotationDuration, RotateMode.FastBeyond360).SetEase(_rotationEase);
    }

    protected override bool IsFinishedSpecific()
    {
        bool result = false;
        if (_moveObject)
            result |= !_moveTweener.active;

        if (_rotateObject)
            result |= !_rotationTweener.active;

        return result;
    }

    public override string BuildGameObjectName()
    {
        string strName = "";
        if (_moveObject)
            strName += "MOVE ";
        if (_rotateObject)
            strName += "ROTATE ";
        if (_targetObject != null)
        {
            strName += _targetObject.name;
        }

        return strName;
    }
}
