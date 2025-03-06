using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCamControl : MonoBehaviour
{
    [Header("Camera Movement")]
    [SerializeField] Transform _playerTransform;
    [SerializeField] Transform _cameraOverlay;
    [SerializeField] float _joyStCamControlSpeed = 1000f;
    [SerializeField] float _mouseCamControlSpeed = 100f;

    [Header("Raycast")]
    [SerializeField] RubiksCubeController rubiksCubeController;
    [SerializeField] LayerMask _detectableLayer;
    [SerializeField] float _maxDistance;

    Transform _oldTile;

    float _xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //rubiksCubeController = FindObjectOfType<RubiksCubeController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Camera movement
        float moveX = Input.GetAxis("Mouse X") * _mouseCamControlSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Mouse Y") * _mouseCamControlSpeed * Time.deltaTime * -1;
        moveX += Input.GetAxis("Joystick X") * _joyStCamControlSpeed * Time.deltaTime;
        moveY += Input.GetAxis("Joystick Y") * _joyStCamControlSpeed * Time.deltaTime;

        _xRotation -= moveY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        _playerTransform.Rotate(Vector3.up * moveX);
        //_cameraOverlay.Rotate(Vector3.up * moveX);
        //_cameraOverlay.Rotate(_cameraOverlay.forward * moveY);

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
