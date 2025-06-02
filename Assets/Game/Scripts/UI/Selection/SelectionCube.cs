//
//  Outline.cs
//  QuickOutline
//
//  Created by Chris Nolet on 3/30/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

using DG.Tweening;
using DG.Tweening.Core;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

[DisallowMultipleComponent]
public class SelectionCube : MonoBehaviour
{
    [SerializeField]
    bool _isTileLocked;
    [SerializeField]
    int _defaultRenderingLayerMask, _cubeObjectSelectionRenderingLayerMask = 9, _axisObjectSelectionRenderingLayerMask = 10, _cubeSelectionRenderingLayerMask, _axisSelectionRenderingLayerMask, _axisLockRenderingLayerMask = 6, _playerOnTileRenderingLayerMask = 5, _objectLockRenderingLayerMask = 11;

    private Renderer[] _renderers;
    private Material[] allOldMat = new Material[0];

    public SelectionMode CurrentSelectionMode { get; private set; }

    class SelectionTweens
    {
        public TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> EnableSelectionTween;
        public TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> DisableSelectionTween;

        public SelectionTweens()
        {
            this.EnableSelectionTween = null;
            this.DisableSelectionTween = null;
        }
    }

    Dictionary<Renderer, SelectionTweens> _selectionCurrentValues = new();
    public bool IsTileLocked { get => _isTileLocked; set => _isTileLocked = value; }

    public enum SelectionMode
    {
        AXIS,
        CUBE,
        LOCKED,
        PLAYERONTILE,
        ENABLE,
        DISABLE,
        NOT_SELECTED
    }
    
    void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in _renderers) 
        {
            if (renderer.transform.CompareTag("Floor") || renderer.transform.CompareTag("SelectionShine"))
            {
                renderer.material = Instantiate(renderer.material);
                _selectionCurrentValues.Add(renderer, new SelectionTweens());
                _selectionCurrentValues[renderer] = new SelectionTweens();
            }
        }

        CurrentSelectionMode = SelectionMode.NOT_SELECTED;

        if (_isTileLocked) Select(SelectionMode.LOCKED);
    }

    public void Select(SelectionMode mode)
    {
        if (_renderers == null)
            return;
        if(mode == CurrentSelectionMode)
            return;

        foreach (var renderer in _renderers)
        {
            switch (mode)
            {
                case SelectionMode.AXIS:
                case SelectionMode.CUBE:
                    if (renderer.transform.CompareTag("Floor"))
                    {
                        _ToggleSelectionShader(true, renderer, GameManager.Instance.Settings.AxisSelectionFadeInDuration);
                    }
                    //else
                    //    renderer.renderingLayerMask = (uint)Mathf.Pow(2, _axisObjectSelectionRenderingLayerMask);    
                    break;
                /*
                case SelectionMode.CUBE:
                    if (renderer.transform.CompareTag("Floor"))
                        renderer.renderingLayerMask = (uint)Mathf.Pow(2, _cubeSelectionRenderingLayerMask);
                    else
                        renderer.renderingLayerMask = (uint)Mathf.Pow(2, _cubeObjectSelectionRenderingLayerMask);                    
                    break;
                */
                case SelectionMode.LOCKED:
                case SelectionMode.PLAYERONTILE:
                    /*
                    if (renderer.transform.CompareTag("Floor"))
                    {
                        renderer.renderingLayerMask = (uint)Mathf.Pow(2, _axisLockRenderingLayerMask);
                    }
                    else
                        renderer.renderingLayerMask = (uint)Mathf.Pow(2, _objectLockRenderingLayerMask);
                    */
                    break;    
                case SelectionMode.ENABLE:
                    renderer.enabled = true;
                    break;
                case SelectionMode.DISABLE:
                    renderer.enabled = false;
                    break;
            }
        }
        CurrentSelectionMode = mode;
    }

    public void Unselect()
    {
        if(CurrentSelectionMode == SelectionMode.NOT_SELECTED)
            return;
        foreach (var renderer in _renderers)
        {
            if((CurrentSelectionMode == SelectionMode.AXIS || CurrentSelectionMode == SelectionMode.CUBE)
                && (renderer.transform.CompareTag("Floor")))
            {
                _ToggleSelectionShader(false, renderer, GameManager.Instance.Settings.AxisSelectionFadeOutDuration);
            }
            else
            {
                renderer.renderingLayerMask = (uint)Mathf.Pow(2, _defaultRenderingLayerMask);
            }
        }
        CurrentSelectionMode = SelectionMode.NOT_SELECTED;
    }

    public void StartShineAnim()
    {
        //if (CurrentSelectionMode == SelectionMode.AXIS || CurrentSelectionMode == SelectionMode.CUBE)
        StartCoroutine(ShineAnim());
    }
    private IEnumerator ShineAnim()
    {
        foreach (var renderer in _renderers)
        {
            if (renderer.transform.CompareTag("SelectionShine"))
            {
                _ToggleSelectionShader(true, renderer, GameManager.Instance.Settings.AxisSelectionFadeInDuration);
            }
            else if (renderer.transform.CompareTag("Floor"))
            {
                _ToggleSelectionShader(false, renderer, GameManager.Instance.Settings.AxisSelectionFadeOutDuration);
            }
        }        
        yield return new WaitForSeconds(GameManager.Instance.Settings.RubikscCubeAxisRotationDuration);
        foreach (var renderer in _renderers)
        {
            if (renderer.transform.CompareTag("SelectionShine"))
            {
                _ToggleSelectionShader(false, renderer, GameManager.Instance.Settings.AxisSelectionFadeOutDuration);
            }
        }
    }

    private void _ToggleSelectionShader(bool _selected, Renderer renderer, float duration)
    {
        if (_selected)
        {
            if (_selectionCurrentValues.ContainsKey(renderer))
            {
                if (_selectionCurrentValues[renderer].EnableSelectionTween != null && _selectionCurrentValues[renderer].EnableSelectionTween.active)
                    return;

                if (_selectionCurrentValues[renderer].DisableSelectionTween != null && _selectionCurrentValues[renderer].DisableSelectionTween.active)
                    _selectionCurrentValues[renderer].DisableSelectionTween.Kill();

                _selectionCurrentValues[renderer].EnableSelectionTween = DOTween.To(() => renderer.material.GetFloat("_EffectAlpha"), x => renderer.material.SetFloat("_EffectAlpha", x), 1.0f, duration).SetEase(Ease.InOutSine);
            }
        }
        else{
            if (_selectionCurrentValues.ContainsKey(renderer))
            {
                if (_selectionCurrentValues[renderer].DisableSelectionTween != null && _selectionCurrentValues[renderer].DisableSelectionTween.active)
                    return;
                if (_selectionCurrentValues[renderer].EnableSelectionTween != null && _selectionCurrentValues[renderer].EnableSelectionTween.active)
                    _selectionCurrentValues[renderer].EnableSelectionTween.Kill();

                _selectionCurrentValues[renderer].DisableSelectionTween = DOTween.To(() => renderer.material.GetFloat("_EffectAlpha"), x => renderer.material.SetFloat("_EffectAlpha", x), 0.0f, duration).SetEase(Ease.InOutSine);
            }
        }
    }
}

 