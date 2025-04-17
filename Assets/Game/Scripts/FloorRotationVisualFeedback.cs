using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class FloorRotationVisualFeedback : MonoBehaviour
{
    [SerializeField] Animator _mainCameraAnimator;
    GameObject _parentCubePart;
    float _lastYRotation;
    float _currentYRotation;

    bool _isRotating;
    int _rotationDir;

    int _oldRotationDir = 0;

    // Start is called before the first frame update
    void Start()
    {   
        _parentCubePart = GetComponent<DetectNewParent>().currentParent;
        if (_parentCubePart != null) _lastYRotation = _parentCubePart.transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        _parentCubePart = GetComponent<DetectNewParent>().currentParent;
        if (_parentCubePart == null) { return; }

        // determin if the parint is rotating and in what direction
        _currentYRotation = _parentCubePart.transform.eulerAngles.y;
        float delta = Mathf.DeltaAngle(_lastYRotation, _currentYRotation);

        _lastYRotation = _currentYRotation;

        _isRotating = Mathf.Abs(delta) > 0;
        if (Mathf.Abs(delta) > 10) return;
        _rotationDir = _isRotating ? (delta > 0 ? 1 : -1) : 0;

        // decide wether to apply visual feedback. 

        //.Log("rotDir is: " +  _rotationDir + "\noldRotDir is: " + _oldRotationDir + "\n");

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
}
