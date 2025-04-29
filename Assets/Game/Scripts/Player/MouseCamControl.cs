using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class MouseCamControl : MonoBehaviour
{
    [Header("Camera Movement")]
    [SerializeField] Transform _playerTransform;

    [Header("Raycast")]
    [SerializeField] RubiksCubeController rubiksCubeController;
    [SerializeField] LayerMask _detectableLayer;
    [SerializeField] float _maxDistance;

    //To Fix
    //[SerializeField] bool _MoveOverlayCubeWithCamRota = true;

    [SerializeField] bool _doReversedCam = true;

    Transform _oldTile;

    float _yRotation;
    GameSettings _settings;
    InputHandler _inputHandler;


    // Camera movement
    float moveX = 0;
    float moveY = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _settings = GameManager.Instance.Settings;
        Camera.main.fieldOfView = _settings.FOV;
        _inputHandler = InputHandler.Instance;
    }
    public void OnCamera(InputAction.CallbackContext callbackContext) //also used for NoClip
    {
        moveX = callbackContext.ReadValue<Vector2>().x* _settings.CameraSensibilityMouse * Time.deltaTime; 
        moveY = callbackContext.ReadValue<Vector2>().y* _settings.CameraSensibilityMouse * Time.deltaTime; 
    }
    void Update()
    {
        if (_inputHandler == null || !_inputHandler.CanMove)
            return;

        _yRotation -= moveY;
        _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_yRotation, 0f, 0f);
        _playerTransform.Rotate(Vector3.up * moveX);

        /*if (_doReversedCam)
        {

            Vector3 forward = _playerTransform.forward;
            forward.y = 0; // Ignore vertical tilt if needed
            float angle = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
            float normalizedAngle = (angle < 0) ? angle + 360 : angle; // Normalize to 0-360

            if (normalizedAngle >= 315 || normalizedAngle < 135)
            {
                rubiksCubeController.CameraPlayerReversed = false;
            }
            else
            {
                rubiksCubeController.CameraPlayerReversed = true;
            }
        }*/

        /*if (_MoveOverlayCubeWithCamRota)
        {
            _cameraOverlay.Rotate(Vector3.up * moveX);
            _cameraOverlay.Rotate(_cameraOverlay.forward * moveY);
        }*/

        //Raycast

        if (!GameManager.Instance.IsRubiksCubeEnabled)
            return;

        RaycastHit _raycastInfo;

        if (Physics.Raycast(transform.position, transform.forward, out _raycastInfo, _maxDistance, _detectableLayer))
        {
            GameObject collider = _raycastInfo.collider.gameObject;
            _oldTile = collider.transform;
            if (rubiksCubeController != null && _oldTile.parent != null) rubiksCubeController.SetActualCube(_oldTile.parent);
        }
    }
}
