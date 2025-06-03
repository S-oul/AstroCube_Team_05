using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FractalMaster;

public class GameActionMandelbulbFractal : AGameAction
{
    [SerializeField] private MandelbulbParameters _newMandelbulbParameters = new();
    [SerializeField] private FractalMaster _targetFractal;

    [SerializeField] private float _transitionDuration = 2f;
    [SerializeField] private Ease _easeMode = Ease.Linear;

    private bool _wasExecuted = false;

    protected override void ExecuteSpecific()
    {
        StartCoroutine(FadeParameters());
    }

    private IEnumerator FadeParameters()
    {
        DOTween.To(() => _targetFractal.CurrentMandelbulbParameters.FractalPower, x => _targetFractal.CurrentMandelbulbParameters.FractalPower = x, _newMandelbulbParameters.FractalPower, _transitionDuration).SetEase(_easeMode);
        DOTween.To(() =>0.0f, x => _targetFractal.CurrentMandelbulbParameters.Alpha = x, _newMandelbulbParameters.Alpha, _transitionDuration).SetEase(_easeMode);
        DOTween.To(() => _targetFractal.CurrentMandelbulbParameters.BlackAndWhite, x => _targetFractal.CurrentMandelbulbParameters.BlackAndWhite = x, _newMandelbulbParameters.BlackAndWhite, _transitionDuration).SetEase(_easeMode);
        DOTween.To(() => _targetFractal.CurrentMandelbulbParameters.Darkness, x => _targetFractal.CurrentMandelbulbParameters.Darkness = x, _newMandelbulbParameters.Darkness, _transitionDuration).SetEase(_easeMode);
        
        DOTween.To(() => _targetFractal.CurrentMandelbulbParameters.ColorA, x => _targetFractal.CurrentMandelbulbParameters.ColorA = x, _newMandelbulbParameters.ColorA, _transitionDuration).SetEase(_easeMode);
        yield return DOTween.To(() => _targetFractal.CurrentMandelbulbParameters.ColorB, x => _targetFractal.CurrentMandelbulbParameters.ColorB = x, _newMandelbulbParameters.ColorB, _transitionDuration).SetEase(_easeMode).WaitForCompletion();
        _wasExecuted = true;
    }

    protected override bool IsFinishedSpecific()
    {
        return _wasExecuted;
    }

    public override string BuildGameObjectName()
    {
        string strObject = "";
        if (_targetFractal != null)
        {
            strObject = _targetFractal.gameObject.name;
        }
        return $"MANDELBULB FRACTAL FADE : {strObject}";
    }
}
