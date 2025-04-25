using DG.Tweening;
using UnityEngine;

public class GameActionCameraShake : AGameAction
{
    [SerializeField] private float _shakeDuration = 1f;
    [SerializeField] private float _shakeStrength = 1f;

    private Tweener _shakeTweener;

    protected override void ExecuteSpecific()
    {
        _shakeTweener = Camera.main.DOShakePosition(_shakeDuration, _shakeStrength);
    }

    protected override bool IsFinishedSpecific()
    {
        return _shakeTweener.active;
    }

    public override string BuildGameObjectName()
    {
        return $"CAMERA SHAKE {_shakeDuration}s";
    }
}