using UnityEditor.Analytics;
using UnityEngine;

public class SmoothDamping : MonoBehaviour
{
    [SerializeField] Transform _playerTransform;
    [SerializeField] float _verticalSmoothAmount;
    [SerializeField] float _intencity;
    [SerializeField] float _maxHight;
    [SerializeField] float _minHight;

    Vector3 _artCubeStartLocalPos;
    float _imaginaryCubePos;
    float _verticalVelocity;

    private void Start()
    {
        _artCubeStartLocalPos = transform.localPosition;
        _imaginaryCubePos = _playerTransform.position.y;
    }

    private void LateUpdate()
    {
        // calculate _verticalVelocity
        float newImaginaryCubePos = Mathf.SmoothDamp(
            _imaginaryCubePos,
            _playerTransform.position.y,
            ref _verticalVelocity,
            _verticalSmoothAmount
        );
        _imaginaryCubePos = newImaginaryCubePos;

        // clamp
        if (_imaginaryCubePos - _playerTransform.position.y > _maxHight) _imaginaryCubePos = _playerTransform.position.y + _maxHight;
        if (_imaginaryCubePos - _playerTransform.position.y < _minHight) _imaginaryCubePos = _playerTransform.position.y + _minHight;

        // create new pos
        transform.localPosition = 
            _artCubeStartLocalPos - Vector3.up *
            _verticalVelocity *
            _intencity;
    }
}
