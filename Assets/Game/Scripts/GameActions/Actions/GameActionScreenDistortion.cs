using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class GameActionScreenDistortion : AGameAction
{
    public enum EDistortAction
    {
        PULSE,
        TO_DISTORT,
        TO_NORMAL
    }

    [SerializeField] private EDistortAction _distortAction = EDistortAction.PULSE;
    [SerializeField, HideIf("_distortAction", EDistortAction.TO_NORMAL), Range(0.0f, 1.0f)] private float _targetValue = 0.5f;
    [SerializeField] private float _duration = 1.0f;

    [SerializeField, HideIf("_distortAction", EDistortAction.TO_NORMAL)] private Ease _easeIn = Ease.Linear;
    [SerializeField, HideIf("_distortAction", EDistortAction.TO_DISTORT)] private Ease _easeOut = Ease.Linear;

    PostProcessManager _postProcessManager = null;
    bool _isExecuting = false;

    void Start()
    {
        _postProcessManager = PostProcessManager.Instance;
    }

    protected override void ExecuteSpecific()
    {
        if(_postProcessManager == null)
            return;

        switch (_distortAction)
        {
            case EDistortAction.PULSE:
                StartCoroutine(_Pulse());
                break;
            case EDistortAction.TO_DISTORT:
                _postProcessManager.SetScreenDistortion(_targetValue, _duration, _easeIn);
                break;
            case EDistortAction.TO_NORMAL:
                _postProcessManager.SetScreenDistortion(0.0f, _duration, _easeOut);
                break;
        }

        StartCoroutine(_Execute());
    }

    IEnumerator _Pulse()
    {
        _StartDistort(_duration / 2, _easeIn);
        yield return new WaitForSeconds(_duration/2);
        _StopDistort(_duration / 2, _easeOut);
    }

    private void _StartDistort(float duration, Ease ease = Ease.Linear) => _postProcessManager.SetScreenDistortion(_targetValue, duration, ease);
    private void _StopDistort(float duration, Ease ease = Ease.Linear) => _postProcessManager.SetScreenDistortion(0.0f, duration, ease);

    protected override bool IsFinishedSpecific()
    {
        return _isExecuting;
    }

    public override string BuildGameObjectName()
    {
        return $"DISTORT : {_distortAction.ToString()}";
    }

    IEnumerator _Execute()
    {
        _isExecuting = true;
        yield return new WaitForSeconds(_duration);
        _isExecuting = false;
    }
}