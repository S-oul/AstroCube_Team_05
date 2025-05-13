using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameActionCanvasGroupApparition : AGameAction
{
    [SerializeField] CanvasGroup _canvasGroup;

    [SerializeField] float _fadeInDuration = 1;
    [SerializeField] private float _stayDuration = 1.5f;
    [SerializeField] float _fadeOutDuration = 1;

    [SerializeField] Ease _fadeEase = Ease.Linear;

    private bool _isExecuting = false;

    private void Start()
    {
        _canvasGroup.alpha = 0.0f;
    }

    protected override void ExecuteSpecific()
    {
        StartCoroutine(CanvasDisplay());
    }

    protected override bool IsFinishedSpecific()
    {
        return _isExecuting;
    }

    public override string BuildGameObjectName()
    {
        string strCanvasGroup = "";
        if (_canvasGroup != null)
        {
            strCanvasGroup = _canvasGroup.name;
        }
        return $"CANVAS GROUP APPARITION : {strCanvasGroup}";
    }

    IEnumerator CanvasDisplay()
    {
        _isExecuting = true;

        _FadeDisplay(1.0f, _fadeInDuration);

        yield return new WaitForSeconds(_stayDuration);

        _FadeDisplay(0.0f, _fadeOutDuration);

        _isExecuting = false;
    }

    private void _FadeDisplay(float newAlpha, float duration)
    {
        DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, newAlpha, duration).SetEase(_fadeEase);
    }
}