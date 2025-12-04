using MoreMountains.Feedbacks;
using UnityEngine;

public class PositionRelativeToFOV : MonoBehaviour
{
    [SerializeField] Camera _camera;
    Vector3 _defaultPosition;
    float _defaultFOV = 60;

    private void Start()
    {
        _defaultPosition = transform.localPosition;
    }

    void Update()
    {
        //Vector3 newPos = _defaultPosition;
        //float FOVdifference = (_camera.fieldOfView - _defaultFOV) / 30;

        //newPos.x += 3.5f * FOVdifference;
        //newPos.y += -1 * FOVdifference;
        //newPos.z += -9 * FOVdifference;

        //transform.localPosition = newPos;

        Vector3 newPos = _defaultPosition;
        float FOVdifference = (_camera.fieldOfView - _defaultFOV) / 25;

        newPos.x += -0.1f * FOVdifference;
        newPos.y += -0.5f * FOVdifference;
        newPos.z += -6 * FOVdifference;

        transform.localPosition = newPos;
    }
}
