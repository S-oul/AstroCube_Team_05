using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCamControl : MonoBehaviour
{
    [Header("Camera Movement")]
    [SerializeField] Transform _playerTransform;

    [Header("Raycast")]
    [SerializeField] RubiksCubeController rubiksCubeController;
    [SerializeField] LayerMask _detectableLayer;
    [SerializeField] float _maxDistance;

    Transform _oldTile;
    float _xRotation;
    GameSettings _settings;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _settings = GameManager.Instance.Settings;
    }

    void Update()
    {
        // Camera movement
        float moveX = Input.GetAxis("Mouse X") * _settings.CameraSensibilityMouse * Time.deltaTime;
        float moveY = Input.GetAxis("Mouse Y") * _settings.CameraSensibilityMouse * Time.deltaTime * -1;
        moveX += Input.GetAxis("Joystick X") * _settings.CameraSensibilityJoystick * Time.deltaTime;
        moveY += Input.GetAxis("Joystick Y") * _settings.CameraSensibilityJoystick * Time.deltaTime;

        _xRotation -= moveY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        _playerTransform.Rotate(Vector3.up * moveX);

        //Raycast
        RaycastHit _raycastInfo;

        if (Physics.Raycast(transform.position, transform.forward, out _raycastInfo, _maxDistance, _detectableLayer))
        {
            GameObject collider = _raycastInfo.collider.gameObject;
            if (_oldTile != collider.transform)
            {
                _oldTile = collider.transform;
                if (rubiksCubeController != null && _oldTile.parent != null) rubiksCubeController.SetActualCube(_oldTile.parent);
            }
        }
    }
}
