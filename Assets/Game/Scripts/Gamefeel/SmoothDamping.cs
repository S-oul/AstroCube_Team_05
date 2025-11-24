using UnityEditor.Analytics;
using UnityEngine;

public class SmoothDamping : MonoBehaviour
{
    [SerializeField] Transform _playerTransform;
    [SerializeField] float _verticalSmoothAmount;
    [SerializeField] float _intencity;

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
        float newImaginaryCubePos = Mathf.SmoothDamp(
            _imaginaryCubePos,
            _playerTransform.position.y,
            ref _verticalVelocity,
            _verticalSmoothAmount
        );
        _imaginaryCubePos = newImaginaryCubePos;

        transform.localPosition = _artCubeStartLocalPos - Vector3.up * _verticalVelocity * _intencity;
    }
}
