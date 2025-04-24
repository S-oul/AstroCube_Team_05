using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionChangeLight : AGameAction
{
    [SerializeField] private Light _light;
    [SerializeField] private Ease _easeMode = Ease.Linear;

    [BoxGroup("EndValues"), SerializeField] private bool _changeIntensity = false;
    [BoxGroup("EndValues"), ShowIf("_changeIntensity"), SerializeField] private float _endIntensity = 1f;
    [BoxGroup("EndValues"), ShowIf("_changeIntensity"), SerializeField] private float _fadeIntensityDuration = 1f;

    [BoxGroup("EndValues"), SerializeField] private bool _changeColor = false;
    [BoxGroup("EndValues"), ShowIf("_changeColor"), SerializeField] private Color _endColor = Color.white;
    [BoxGroup("EndValues"), ShowIf("_changeColor"), SerializeField] private float _fadeColorDuration = 1f;

    [BoxGroup("EndValues"), SerializeField] private bool _changeRange = false;
    [BoxGroup("EndValues"), ShowIf("_changeRange"), SerializeField] private float _endRange = 1f;
    [BoxGroup("EndValues"), ShowIf("_changeRange"), SerializeField] private float _fadeRangeDuration = 1f;

    private Tweener _fadeIntensity;
    private Tweener _fadeColor;
    private Tweener _fadeRange;

    protected override void ExecuteSpecific()
    {

        if (_changeIntensity)
            _fadeIntensity = _light.DOIntensity(_endIntensity, _fadeIntensityDuration).SetEase(_easeMode);

        if (_changeColor)
            _fadeColor = _light.DOColor(_endColor, _fadeColorDuration).SetEase(_easeMode);

        if (_changeRange)
            _fadeRange = DOTween.To(() => _light.range, x => _light.range = x, _endRange, _fadeRangeDuration).SetEase(_easeMode);
    }

    protected override bool IsFinishedSpecific()
    {
        return _fadeIntensity.active || _fadeColor.active || _fadeRange.active;
    }

    public override string BuildGameObjectName()
    {
        string strLight = "";
        if (_light != null)
        {
            strLight = _light.name;
        }
        return $"CHANGE LIGHT {strLight}";
    }
}
