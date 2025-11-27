using System;
using Unity.Mathematics;
using UnityEditor.Analytics;
using UnityEngine;

public class SmoothDamping : MonoBehaviour
{
    [Header("Y-Axis")]

    // vertical movement
    [SerializeField] Transform _playerTransform;
    [SerializeField] float _verticalSmoothAmount;
    [SerializeField] float _verticalIntencity;
    [SerializeField] float _maxHight;
    [SerializeField] float _minHight;

    Vector3 _artCubeStartLocalPos;
    float _imaginaryCubePosY;
    float _verticalVelocity;

    [Header("X and Z Axis")]

    // horisontal movement
    [SerializeField] float _horizontalSmoothAmount;
    [SerializeField] float _horizontalIntencity;
    [SerializeField] float _maxHorizontalRange;
    [SerializeField] float _minHorizontalRange;

    float _imaginaryCubePosX;
    float _horizontalVelocityX;

    float _imaginaryCubePosZ;
    float _horizontalVelocityZ;

    [Header("Camera Rotation")]
    [SerializeField] Transform _cameraTransfrom;
    [SerializeField] float _rotationSmoothAmount;
    [SerializeField] float _rotationIntencity;
    [SerializeField] Vector2 _maxRange;
    [SerializeField] Vector2 _minRange;

    Quaternion _oldCamRotation;
    Vector3 _rotationDirection;
    float _rotationSpeed;

    Vector2 _imaginaryCubePosVec;
    Vector2 _rotationVelocity;

    Vector2 targetPos = Vector2.zero;

    private void Start()
    {
        _artCubeStartLocalPos = transform.localPosition;
        _imaginaryCubePosY = _playerTransform.position.y;
        _imaginaryCubePosX = _playerTransform.position.x;

        _oldCamRotation = _cameraTransfrom.rotation;
    }

    private void LateUpdate()
    {
        // vertical movement ----------------

        // calculate _verticalVelocity
        float newImaginaryCubePos = Mathf.SmoothDamp(
            _imaginaryCubePosY,
            _playerTransform.position.y,
            ref _verticalVelocity,
            _verticalSmoothAmount
        );
        _imaginaryCubePosY = newImaginaryCubePos;

        // clamp
        if (_imaginaryCubePosY - _playerTransform.position.y > _maxHight) _imaginaryCubePosY = _playerTransform.position.y + _maxHight;
        if (_imaginaryCubePosY - _playerTransform.position.y < _minHight) _imaginaryCubePosY = _playerTransform.position.y + _minHight;


        // horizontal movement ----------------

        // X

        // calculate _horizontalVelocity
        newImaginaryCubePos = Mathf.SmoothDamp(
            _imaginaryCubePosX,
            _playerTransform.position.x,
            ref _horizontalVelocityX,
            _horizontalSmoothAmount
        );
        _imaginaryCubePosX = newImaginaryCubePos;

        // clamp
        if (_imaginaryCubePosX - _playerTransform.position.x > _maxHorizontalRange) _imaginaryCubePosX = _playerTransform.position.x + _maxHorizontalRange;
        if (_imaginaryCubePosX - _playerTransform.position.x < _minHorizontalRange) _imaginaryCubePosX = _playerTransform.position.x + _minHorizontalRange;


        // Z
        
        newImaginaryCubePos = Mathf.SmoothDamp(
            _imaginaryCubePosZ,
            _playerTransform.position.z,
            ref _horizontalVelocityZ,
            _horizontalSmoothAmount
        );
        _imaginaryCubePosZ = newImaginaryCubePos;

        // clamp
        if (_imaginaryCubePosZ - _playerTransform.position.z > _maxHorizontalRange) _imaginaryCubePosZ = _playerTransform.position.z + _maxHorizontalRange;
        if (_imaginaryCubePosZ - _playerTransform.position.z < _minHorizontalRange) _imaginaryCubePosZ = _playerTransform.position.z + _minHorizontalRange;


        // camera rotation movement ----------------

        //GetCameraRotation();

        //targetPos += new Vector2(_rotationDirection.x, _rotationDirection.y + _rotationDirection.z) * _rotationSpeed;

        targetPos += _cameraTransfrom.gameObject.GetComponent<MouseCamControl>().GetMousePos;

        Vector2 newImaginaryCubePosVec = Vector2.SmoothDamp(
            _imaginaryCubePosVec,
            targetPos,
            ref _rotationVelocity,
            _rotationSmoothAmount
        );
        _imaginaryCubePosVec = newImaginaryCubePosVec;

        // clamp
        if (_imaginaryCubePosVec.x - targetPos.x > _maxRange.x) _imaginaryCubePosVec.x = targetPos.x + _maxRange.x;
        if (_imaginaryCubePosVec.x - targetPos.x < _minRange.x) _imaginaryCubePosVec.x = targetPos.x + _minRange.x;

        if (_imaginaryCubePosVec.y - targetPos.y > _maxRange.y) _imaginaryCubePosVec.y = targetPos.y + _maxRange.y;
        if (_imaginaryCubePosVec.y - targetPos.y < _minRange.y) _imaginaryCubePosVec.y = targetPos.y + _minRange.y;

        Vector3 rotationVelocityV3 = new Vector3(_rotationVelocity.x, _rotationVelocity.y, 0);

        Debug.Log(targetPos);

        // create new pos ----------------
        transform.localPosition =
            _artCubeStartLocalPos

            + transform.InverseTransformDirection(Vector3.down) *
            _verticalVelocity *
            _verticalIntencity

            + transform.InverseTransformDirection(Vector3.left) *
            _horizontalVelocityX *
            _horizontalIntencity

            + transform.InverseTransformDirection(Vector3.back) *
            _horizontalVelocityZ *
            _horizontalIntencity

            //- transform.InverseTransformDirection(rotationVelocityV3) * _rotationIntencity;
            - rotationVelocityV3 * _rotationIntencity;
    }

    //void GetCameraRotation()
    //{
    //    Quaternion currentRot = _cameraTransfrom.rotation;
    //    Quaternion delta = currentRot * Quaternion.Inverse(_oldCamRotation);

    //    delta.ToAngleAxis(out float angle, out Vector3 axis); // get angle
    //    if (angle > 180f) angle -= 360f; // normalize

    //    if (Mathf.Abs(angle) > 0.0001f)
    //    {
    //        _rotationDirection = axis;
    //        _rotationSpeed = angle;
    //    } 
    //    else
    //    {
    //        _rotationDirection = Vector3.zero;
    //        _rotationSpeed = 0;
    //    }

    //    Debug.Log("Rotating around " + axis + " with angle " + angle);

    //    _oldCamRotation = currentRot;
    //}
}
