using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

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
    //[SerializeField] Camera _kaleidoCam;

    //To Fix
    //[SerializeField] bool _MoveOverlayCubeWithCamRota = true;

    [SerializeField] bool _doReversedCam = true;

    Transform _oldTile;

    float _yRotation;
    GameSettings _settings;
    InputHandler _inputHandler;


    // Camera movement
    Vector2 mousePos = new();

    float _cameraSensibilityMouse;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _settings = GameManager.Instance.Settings;
        UpdateCameraFOV(_customSettings.customFov);
        _inputHandler = InputHandler.Instance;
        _cameraSensibilityMouse = _customSettings.customMouse;
        ForceResetSelection();
    }
    public void OnCamera(InputAction.CallbackContext callbackContext) //also used for NoClip
    {
        mousePos = callbackContext.ReadValue<Vector2>() * _cameraSensibilityMouse * Time.deltaTime;
    }

    void Update()
    {
        if (_inputHandler == null || !_inputHandler.CanMove)
            return;

        _yRotation -= mousePos.y;
        _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_yRotation, 0f, 0f);
        _playerTransform.Rotate(Vector3.up * mousePos.x);

        if (!GameManager.Instance.IsRubiksCubeEnabled)
            return;

        RaycastHit _raycastInfo;

        if (Physics.Raycast(transform.position, transform.forward, out _raycastInfo, _maxDistance, _detectableLayer))
        {
            GameObject collider = _raycastInfo.collider.gameObject;
            _oldTile = collider.transform;
            if (rubiksCubeController != null && _oldTile.parent != null)
            {
                if(rubiksCubeController.ActualFace == null || rubiksCubeController.ActualFace.transform != _oldTile.parent)
                    rubiksCubeController.SetActualCube(_oldTile.parent);
            }
        }
    }

    private void ForceResetSelection()
    {
        if (_inputHandler == null || !_inputHandler.CanMove)
            return;

        _yRotation -= mousePos.y;
        _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_yRotation, 0f, 0f);
        _playerTransform.Rotate(Vector3.up * mousePos.x);

        if (!GameManager.Instance.IsRubiksCubeEnabled)
            return;

        RaycastHit _raycastInfo;

        if (Physics.Raycast(transform.position, transform.forward, out _raycastInfo, _maxDistance, _detectableLayer))
        {
            GameObject collider = _raycastInfo.collider.gameObject;
            _oldTile = collider.transform;

            rubiksCubeController.SetActualCube(_oldTile.parent);            
        }
    }

    private void OnEnable()
    {
        EventManager.OnFOVChange += UpdateCameraFOV;
        EventManager.OnMouseChange += UpdateCameraMouseSensitivity;
        EventManager.OnEndNarrativeSequence += ResetMousePosition;
        EventManager.OnPlayerChangeParent += ForceResetSelection;
    }
    private void OnDisable()
    {
        EventManager.OnFOVChange -= UpdateCameraFOV;
        EventManager.OnMouseChange -= UpdateCameraMouseSensitivity;
        EventManager.OnEndNarrativeSequence -= ResetMousePosition;
        EventManager.OnPlayerChangeParent -= ForceResetSelection;
    }

    void UpdateCameraFOV(float newFOV)
    {
        _mainCamera.fieldOfView = newFOV;
        //_kaleidoCam.fieldOfView = newFOV * (4f/7f);
    }    
    void UpdateCameraMouseSensitivity(float newCamMouseSen)
    {
        _cameraSensibilityMouse = newCamMouseSen;
    }

    void ResetMousePosition()
    {
        mousePos = Vector2.zero;
        _yRotation = 0.0f;
    }
}
