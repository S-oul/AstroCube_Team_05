using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RubiksStatic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class CameraAnimator : MonoBehaviour
{
    [SerializeField] Animator _mainCameraAnimator;
    SelectionCube _parentCubePart;
    float _lastYRotation;
    float _currentYRotation;

    bool _isRotating;
    int _rotationDir;

    int _oldRotationDir = 0;

    void OnEnable()
    {   
        EventManager.OnStartCubeRotation += DriftAnimatorCheck;

        _parentCubePart = GetComponent<DetectNewParent>().CurrentParent;
        if (_parentCubePart != null) _lastYRotation = _parentCubePart.transform.eulerAngles.y;
    }

    private void OnDisable()
    {
        EventManager.OnStartCubeRotation -= DriftAnimatorCheck;
    }

    void DriftAnimatorCheck()
    {
        if(GameManager.Instance.ActualSliceAxis == SliceAxis.Y)
        StartCoroutine(DriftCoroutine());
    }
    IEnumerator DriftCoroutine()
    {
        _parentCubePart = GetComponent<DetectNewParent>().CurrentParent;
        _lastYRotation = _parentCubePart.transform.eulerAngles.y;
        if (!_parentCubePart) {print("hey"); yield return null; }

        yield return new WaitForEndOfFrame();
        
        _currentYRotation = _parentCubePart.transform.eulerAngles.y;
        // determine if the parent is rotating and in what direction
        float delta = Mathf.DeltaAngle(_lastYRotation, _currentYRotation);
        _lastYRotation = _currentYRotation;

        print(delta);
        _isRotating = Mathf.Abs(delta) > 0;
        _rotationDir = delta > 0 ? 1 : -1;

        // decide wether to apply visual feedback. 

        if (_rotationDir > 0)
        {
            _mainCameraAnimator.SetTrigger("DriftLeft");
        }
        else if (_rotationDir < 0)
        {
            _mainCameraAnimator.SetTrigger("DriftRight");
        } 
    }
    public IEnumerator TurnAround()
    {
        if(_mainCameraAnimator)
            _mainCameraAnimator.SetTrigger("TurnAround");
        AnimationClip anim = _mainCameraAnimator.runtimeAnimatorController.animationClips.First(x => x.name == "TurnAround");
        yield return new WaitForSeconds(anim.length);
    }
}
