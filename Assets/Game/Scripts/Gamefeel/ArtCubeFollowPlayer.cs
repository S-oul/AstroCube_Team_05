using UnityEngine;

public class ArtCubeFollowPlayer : MonoBehaviour
{
    [SerializeField] Transform _targetTransform;
    [SerializeField] float _verticalSmoothAmount;
    [SerializeField] float _horisontalSmoothAmount;

    Vector3 verticalVelocity;
    Vector3 horisontalVelocity;

    private void LateUpdate()
    {
        Vector3 newVericalPos = Vector3.SmoothDamp(
            transform.position,
            _targetTransform.position,
            ref verticalVelocity,
            _verticalSmoothAmount
        );

        Vector3 newHorisontalPos = Vector3.SmoothDamp(
            transform.position,
            _targetTransform.position,
            ref horisontalVelocity,
            _horisontalSmoothAmount
        );

        transform.position = new Vector3(
            newHorisontalPos.x,
            //_targetTransform.position.x,
            newVericalPos.y,
            newHorisontalPos.z
        //_targetTransform.position.z
        );
    }
}