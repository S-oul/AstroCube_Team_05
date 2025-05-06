using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class CameraAnimator : MonoBehaviour
{
    [SerializeField] Animator _mainCameraAnimator;
    GameObject _parentCubePart;
    float _lastYRotation;
    float _currentYRotation;

    bool _isRotating;
    int _rotationDir;

    int _oldRotationDir = 0;

    void Start()
    {   
        _parentCubePart = GetComponent<DetectNewParent>().currentParent;
        if (_parentCubePart != null) _lastYRotation = _parentCubePart.transform.eulerAngles.y;
    }

    void Update()
    {
        _parentCubePart = GetComponent<DetectNewParent>().currentParent;
        if (_parentCubePart == null) { return; }

        // determine if the parent is rotating and in what direction
        _currentYRotation = _parentCubePart.transform.eulerAngles.y;
        float delta = Mathf.DeltaAngle(_lastYRotation, _currentYRotation);

        _lastYRotation = _currentYRotation;

        _isRotating = Mathf.Abs(delta) > 0;
        if (Mathf.Abs(delta) > 10) return;
        _rotationDir = _isRotating ? (delta > 0 ? 1 : -1) : 0;

        // decide wether to apply visual feedback. 

        if (_rotationDir > 0 && _oldRotationDir <= 0)
        {
            _mainCameraAnimator.SetTrigger("DriftLeft");
        }
        else if (_rotationDir < 0 && _oldRotationDir >= 0)
        {
            _mainCameraAnimator.SetTrigger("DriftRight");
        } 
        else if (_rotationDir == 0 && _oldRotationDir > 0)
        {
            _mainCameraAnimator.SetTrigger("DriftRight");
        } 
        else if (_rotationDir == 0 && _oldRotationDir < 0)
        {
            _mainCameraAnimator.SetTrigger("DriftLeft");
        }

        _oldRotationDir = _rotationDir;
    }

    public IEnumerator TurnAround()
    {
        if(_mainCameraAnimator)
            _mainCameraAnimator.SetTrigger("TurnAround");
        AnimationClip anim = _mainCameraAnimator.runtimeAnimatorController.animationClips.First(x => x.name == "TurnAround");
        yield return new WaitForSeconds(anim.length);
    }
}
