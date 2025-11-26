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

    private void Start()
    {
        _artCubeStartLocalPos = transform.localPosition;
        _imaginaryCubePosY = _playerTransform.position.y;
        _imaginaryCubePosX = _playerTransform.position.x;
    }

    private void LateUpdate()
    {
        // vertical movement
        
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


        // horizontal movement

        // X

        // calculate _horizontalVelocity
        newImaginaryCubePos = Mathf.SmoothDamp(
            _imaginaryCubePosX,
            _playerTransform.localPosition.x,
            ref _horizontalVelocityX,
            _horizontalSmoothAmount
        );
        _imaginaryCubePosX = newImaginaryCubePos;

        // clamp
        if (_imaginaryCubePosX - _playerTransform.localPosition.x > _maxHorizontalRange) _imaginaryCubePosX = _playerTransform.localPosition.x + _maxHorizontalRange;
        if (_imaginaryCubePosX - _playerTransform.localPosition.x < _minHorizontalRange) _imaginaryCubePosX = _playerTransform.localPosition.x + _minHorizontalRange;


        // Z
        
        newImaginaryCubePos = Mathf.SmoothDamp(
            _imaginaryCubePosZ,
            _playerTransform.localPosition.z,
            ref _horizontalVelocityZ,
            _horizontalSmoothAmount
        );
        _imaginaryCubePosZ = newImaginaryCubePos;

        // clamp
        if (_imaginaryCubePosZ - _playerTransform.localPosition.z > _maxHorizontalRange) _imaginaryCubePosZ = _playerTransform.localPosition.z + _maxHorizontalRange;
        if (_imaginaryCubePosZ - _playerTransform.localPosition.z < _minHorizontalRange) _imaginaryCubePosZ = _playerTransform.localPosition.z + _minHorizontalRange;


        // create new pos
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
            _horizontalIntencity;
    }
}
