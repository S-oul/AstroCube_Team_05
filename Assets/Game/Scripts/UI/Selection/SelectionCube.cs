//
//  Outline.cs
//  QuickOutline
//
//  Created by Chris Nolet on 3/30/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]

public class SelectionCube : MonoBehaviour 
{
    [SerializeField]
    int _defaultRenderingLayerMask, _cubeSelectionRenderingLayerMask, _axisSelectionRenderingLayerMask;

    private Renderer[] _renderers;

    public enum SelectionMode
    {
        AXIS,
        CUBE
    }

    void Awake() 
    {
        // Cache renderers
        _renderers = GetComponentsInChildren<Renderer>();
    }

    public void Select(SelectionMode mode) 
    {
        foreach (var renderer in _renderers) 
        {
            switch (mode)
            {
                case SelectionMode.AXIS:
                    renderer.renderingLayerMask = (uint)Mathf.Pow(2, _axisSelectionRenderingLayerMask);
                    break;
                case SelectionMode.CUBE:
                    renderer.renderingLayerMask = (uint)Mathf.Pow(2, _cubeSelectionRenderingLayerMask);
                    break;
            }
        }
    }

    public void Unselect()
    {
        foreach (var renderer in _renderers)
        {
            renderer.renderingLayerMask = (uint)Mathf.Pow(2, _defaultRenderingLayerMask);
        }
    }
}
