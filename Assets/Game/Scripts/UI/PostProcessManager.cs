using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PostProcessManager : MonoBehaviour
{
    public static PostProcessManager Instance { get; private set; }
    
    [Header("Kaleidoscope Effect")]
    [SerializeField] GameObject _sourceCamera;
    [SerializeField] float _fadeSpeed = 0.1f;

    [Header("Distortion Effect")]
    [SerializeField] private Material _distortMat;
    [SerializeField] private UniversalRendererData _data;
    [SerializeField, MinMaxSlider(0.0f, 100.0f)] private Vector2 _minMaxDistortionValue = new(1.0f, 5.0f);

    Image _kalScreen;
    bool _isKalEnabled = false;
    float _currentKalOpacity = 0;
    FullScreenPassRendererFeature _distortionRenderFeature;
    DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> _distortTween;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _kalScreen = GetComponent<Image>();
        _isKalEnabled = false;
        _distortionRenderFeature = _data.rendererFeatures.First(x => x.name == "Distort") as FullScreenPassRendererFeature;
        _distortionRenderFeature.passMaterial.SetFloat("_DistortionAmount", 1f);
    }

    void Update()
    {
        if (_isKalEnabled && _currentKalOpacity < 1)
        {
            if (_sourceCamera.activeInHierarchy == false) _sourceCamera.SetActive(true);
            if (_kalScreen.enabled == false) _kalScreen.enabled = true;

            if (_currentKalOpacity < 1) 
            {
                //_currentOpacity += _fadeSpeed;
                _currentKalOpacity = 1;
                _currentKalOpacity = _currentKalOpacity >= 1 ? 1 : _currentKalOpacity;
                _kalScreen.material.SetFloat("_Alpha", _currentKalOpacity);
            }
        } 

        if (!_isKalEnabled && _currentKalOpacity > 0) 
        {
            if (_currentKalOpacity > 0)
            {
                _currentKalOpacity -= _fadeSpeed;
                _currentKalOpacity = _currentKalOpacity <= 0 ? 0 : _currentKalOpacity;
                _kalScreen.material.SetFloat("_Alpha", _currentKalOpacity);

                if (_currentKalOpacity == 0)
                {
                    _sourceCamera.SetActive(false);
                    _kalScreen.enabled = false;
                }
            }
        }
    }

    public void SetEnabled(bool isEnabled)
    {
        _isKalEnabled = isEnabled;
    }

    public void SetScreenDistortion(float value)
    {
        if(_distortTween != null)
            _distortTween.Kill();
        _distortionRenderFeature.passMaterial.SetFloat("_DistortionAmount", Mathf.Lerp(_minMaxDistortionValue.x, _minMaxDistortionValue.y, value));
    }

    public void SetScreenDistortion(float value, float duration, Ease ease = Ease.Linear)
    {
        if (_distortTween != null)
            _distortTween.Kill();
        _distortTween = DOTween.To(() => _distortionRenderFeature.passMaterial.GetFloat("_DistortionAmount"), 
                        x => _distortionRenderFeature.passMaterial.SetFloat("_DistortionAmount", x), 
                        Mathf.Lerp(_minMaxDistortionValue.x, _minMaxDistortionValue.y, value), duration).SetEase(ease);        
    }

    private void OnDisable()
    {
        SetScreenDistortion(0.0f);
    }
}

