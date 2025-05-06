using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class MouseCamControl : MonoBehaviour
{
    [Header("Customised Settings")]
    [SerializeField] CustomisedSettings _customSettings; 
    
    [Header("Camera Movement")]
    [SerializeField] Transform _playerTransform;

    [Header("Raycast")]
    [SerializeField] RubiksCubeController rubiksCubeController;
    [SerializeField] LayerMask _detectableLayer;
    [SerializeField] float _maxDistance;

    [Header("Cameras")]
    [SerializeField] Camera _mainCamera;
    [SerializeField] Camera _kaleidoCam;

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

    float _cameraSensibilityMouse;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _settings = GameManager.Instance.Settings;
        UpdateCameraFOV(_customSettings.customFov);
        _inputHandler = InputHandler.Instance;
        _cameraSensibilityMouse = _customSettings.customMouse;
    }
    public void OnCamera(InputAction.CallbackContext callbackContext) //also used for NoClip
    {
        moveX = callbackContext.ReadValue<Vector2>().x* _cameraSensibilityMouse * Time.deltaTime; 
        moveY = callbackContext.ReadValue<Vector2>().y* _cameraSensibilityMouse * Time.deltaTime; 
    }
    void Update()
    {        
        if (_inputHandler == null || !_inputHandler.CanMove)
            return;

        _yRotation -= moveY;
        _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_yRotation, 0f, 0f);
        _playerTransform.Rotate(Vector3.up * moveX);

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

    private void OnEnable()
    {
        EventManager.OnFOVChange += UpdateCameraFOV;
        EventManager.OnMouseChange += UpdateCameraMouseSensitivity;
    }
    private void OnDisable()
    {
        EventManager.OnFOVChange -= UpdateCameraFOV;
        EventManager.OnMouseChange -= UpdateCameraMouseSensitivity;
    }

    void UpdateCameraFOV(float newFOV)
    {
        _mainCamera.fieldOfView = newFOV;
        _kaleidoCam.fieldOfView = newFOV * (4f/7f);
    }    
    void UpdateCameraMouseSensitivity(float newCamMouseSen)
    {
        _cameraSensibilityMouse = newCamMouseSen;
    }
}
