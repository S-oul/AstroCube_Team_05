using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameActionTextApparition : AGameAction
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private UnityEvent _onEndTextApparition;

    [Header("Apparition")]
    [SerializeField] private float _apparitionDelay = 0.0f;
    [SerializeField] private float _charFadeInDelay = 0.06f;
    [SerializeField] private float _charFadeInDuration = 0.7f;
    [SerializeField] private float _stayDuration = 1.5f;

    [Header("Disparition")]
    [SerializeField] private float _charFadeOutDelay = 0.03f;
    [SerializeField] private float _charFadeOutDuration = 0.4f;

    private bool _isExecuting = false;

    private void Start()
    {
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0.0f);
    }

    protected override void ExecuteSpecific()
    {
        StartCoroutine(TextDisplay());
    }

    protected override bool IsFinishedSpecific()
    {
        return _isExecuting;
    }

    public override string BuildGameObjectName()
    {
        return $"TEXT APPARITION : {_text.ToString()}";
    }

    IEnumerator TextDisplay()
    {
        _isExecuting = true;
        yield return new WaitForSeconds(_apparitionDelay);

        DOTweenTMPAnimator animator = new DOTweenTMPAnimator(_text);
        for (int i = 0; i < animator.textInfo.characterCount; i++)
        {
            animator.DOFadeChar(i, 1, _charFadeInDuration);
            yield return new WaitForSeconds(_charFadeInDelay);
        }

        yield return new WaitForSeconds(_stayDuration);

        for (int i = 0; i < animator.textInfo.characterCount; i++)
        {
            animator.DOFadeChar(i, 0, _charFadeOutDuration);
            yield return new WaitForSeconds(_charFadeOutDelay);
        }

        _isExecuting = false;
        _onEndTextApparition?.Invoke();
    }
}