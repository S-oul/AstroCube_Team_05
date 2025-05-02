using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static InputSystemManager;

public class InputDisplay : MonoBehaviour
{
    [SerializeField] bool _playAtStart;
    [SerializeField] float _fadeInDuration = 1;
    [SerializeField] float _fadeOutDuration = 1;
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Ease _fadeEase = Ease.Linear;
    [HorizontalLine(color: EColor.Blue)]
    [InfoBox("If false, input display should be stopped by code", EInfoBoxType.Normal)]
    [SerializeField] bool _resolveAutomaticallyOnInput = true;
    [SerializeField, ShowIf("_resolveAutomaticallyOnInput")] EInputType _expectedInput;
    bool _isInZone = false;
    Collider _colider;

    public Action OnResolve;

    void Start()
    {
        if(!_canvasGroup) return;

        _canvasGroup.gameObject.SetActive(true);
        _canvasGroup.alpha = 0f;
        if(_playAtStart)
            _FadeDisplay(1, _fadeInDuration);
    }

    private void OnEnable()
    {
        if (!_canvasGroup) return;

        if (_resolveAutomaticallyOnInput)
            InputSystemManager.Instance.GetInputActionFromName(InputSystemManager.Instance.GetNameFromType(_expectedInput)).performed += _End;
        else
            OnResolve += _End;
    }

    private void OnDisable()
    {
        if (!_canvasGroup) return;

        if (_resolveAutomaticallyOnInput)
            InputSystemManager.Instance.GetInputActionFromName(InputSystemManager.Instance.GetNameFromType(_expectedInput)).performed -= _End;
        else
            OnResolve -= _End;       
    }

    private void OnValidate()
    {
        if(_colider == null) 
            _colider = GetComponent<Collider>();
        if (_colider != null)
            _colider.enabled = !_playAtStart;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_canvasGroup) return;

        if (!other.CompareTag("Player")) return;
        _isInZone = true;
        _FadeDisplay(1, _fadeInDuration);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_canvasGroup) return;

        if (!other.CompareTag("Player")) return;
        _isInZone = false;
        _FadeDisplay(0, _fadeOutDuration);
    }

    private void _End()
    {
        if (!_canvasGroup) return;

        if (!_isInZone && !_playAtStart) return;
        StartCoroutine(_StartEnd());
    }

    private void _End(InputAction.CallbackContext callbackContext) => _End();

    private IEnumerator _StartEnd()
    {
        _FadeDisplay(0, _fadeOutDuration);
        yield return new WaitForSeconds(_fadeOutDuration);
        Destroy(gameObject);  
    }

    private void _FadeDisplay(float newAlpha, float duration)
    {
        DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, newAlpha, duration).SetEase(_fadeEase);
    }
}
