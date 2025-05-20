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

    [BoxGroup("EndValues"), SerializeField] private bool _changeShadowStrength = false;
    [BoxGroup("EndValues"), ShowIf("_changeShadowStrength"), SerializeField] private float _endShadowStrength = 1f;
    [BoxGroup("EndValues"), ShowIf("_changeShadowStrength"), SerializeField] private float _fadeShadowStrengthDuration = 1f;

    private Tweener _fadeIntensity;
    private Tweener _fadeColor;
    private Tweener _fadeRange;
    private Tweener _fadeShadowStrength;

    protected override void ExecuteSpecific()
    {
        if (_changeIntensity)
            _fadeIntensity = _light.DOIntensity(_endIntensity, _fadeIntensityDuration).SetEase(_easeMode);

        if (_changeColor)
            _fadeColor = _light.DOColor(_endColor, _fadeColorDuration).SetEase(_easeMode);

        if (_changeRange)
            _fadeRange = DOTween.To(() => _light.range, x => _light.range = x, _endRange, _fadeRangeDuration).SetEase(_easeMode);

        if (_changeShadowStrength && _light.shadows != LightShadows.None)
            _fadeShadowStrength = DOTween.To(() => _light.shadowStrength, x => _light.shadowStrength = x, _endShadowStrength, _fadeShadowStrengthDuration).SetEase(_easeMode);
    }

    protected override bool IsFinishedSpecific()
    {
        bool result = false;
        if (_changeIntensity)
            result |= _fadeIntensity.active;

        if (_changeColor)
            result |= _fadeColor.active;

        if (_changeRange)
            result |= _fadeRange.active;

        if (_changeShadowStrength && _light.shadows != LightShadows.None)
            result |= _fadeShadowStrength.active;

        return result;
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
