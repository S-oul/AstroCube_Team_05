using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static InputSystemManager;

public class InputDisplay : MonoBehaviour
{
    private enum EDisplayType
    {
        PLAY_ON_TRIGGER,
        PLAY_AT_START,
        PLAY_ON_CALL
    }

    [SerializeField] EDisplayType _displayType;
    [SerializeField] float _fadeInDuration = 1;
    [SerializeField] float _fadeOutDuration = 1;
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Ease _fadeEase = Ease.Linear;
    [HorizontalLine(color: EColor.Blue)]
    [InfoBox("If false, input display should be stopped by code", EInfoBoxType.Normal)]
    [SerializeField] bool _resolveAutomaticallyOnInput = true;
    [SerializeField] bool _resolveOnLeaveTrigger = false;
    [SerializeField, ShowIf("_resolveAutomaticallyOnInput")] EInputType _expectedInput;

    [SerializeField] UnityEvent _onStartShowText;
    [SerializeField] UnityEvent _onEndShowText;

    bool _isDisplayed = false;
    Collider _colider;

    public Action OnResolve;

    void Start()
    {
        if(!_canvasGroup) return;

        _canvasGroup.gameObject.SetActive(true);
        _canvasGroup.alpha = 0f;
        if(_displayType == EDisplayType.PLAY_AT_START)
            StartDisplay();
    }

    private void OnEnable()
    {
        if (!_canvasGroup) return;

        if (_resolveAutomaticallyOnInput)
        {
            if (InputSystemManager.Instance == null)
                return;
            InputSystemManager.Instance.GetInputActionFromName(InputSystemManager.Instance.GetNameFromType(_expectedInput)).performed += _End;
        }
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
            _colider.enabled = _displayType == EDisplayType.PLAY_ON_TRIGGER;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_displayType != EDisplayType.PLAY_ON_TRIGGER) return;
        if (!_canvasGroup) return;

        if (!other.CompareTag("Player")) return;

        StartDisplay();
    }

    private void OnTriggerExit(Collider other)
    {
        if (_displayType != EDisplayType.PLAY_ON_TRIGGER) return;
        if (!_canvasGroup) return;

        if (!other.CompareTag("Player")) return;

        if(_resolveOnLeaveTrigger)
            EndDisplay();
        else
        {
            _FadeDisplay(0, _fadeOutDuration);
            _isDisplayed = false;
        }
    }
    private void _End(InputAction.CallbackContext callbackContext) => _End();

    private void _End()
    {
        if (!_isDisplayed) return;
        if (!_canvasGroup) return;
        
        StartCoroutine(_EndDisplay());
    }

    public void StartDisplay()
    {
        _isDisplayed = true;
        _onStartShowText?.Invoke();
        _FadeDisplay(1, _fadeOutDuration);
    }

    public void EndDisplay() => StartCoroutine(_EndDisplay()); 

    private IEnumerator _EndDisplay()
    {
        _isDisplayed = false;
        _FadeDisplay(0, _fadeOutDuration);
        yield return new WaitForSeconds(_fadeOutDuration);
        _onEndShowText?.Invoke();
        gameObject.SetActive(false);
    }

    private void _FadeDisplay(float newAlpha, float duration)
    {
        DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, newAlpha, duration).SetEase(_fadeEase);
    }
}
