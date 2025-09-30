using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
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
    [SerializeField] LayerMask _detectableTileLayer;
    [SerializeField] LayerMask _detectableObjectLayer;
    [SerializeField] float _maxDistance;

    //[Header("Cameras")]
    //[SerializeField] 
    Camera _mainCamera;

    [Header("Options")]
    [SerializeField] bool _doReversedCam = true;

    [Header("Sensitivity")]
    [SerializeField] private float yawSensitivity = 1.0f;
    [SerializeField] private float pitchSensitivity = 1.0f;

    Transform _oldTile;

    float _yRotation;
    GameSettings _settings;
    InputHandler _inputHandler;

    Vector2 _mousePos;
    private Quaternion _externalRotationInfluence = Quaternion.identity;
    private float _rotationInfluenceAmount = 0f;

    private float _externalYawInfluence = 0f;
    private float _yawInfluenceAmount = 0f;

    private bool _isExternalPitchForced = false;

    public Transform PlayerTransform => _playerTransform;

    CinemachineVirtualCamera _cinemashineCam;
    LayerMask _detectableLayer;

    void Start()
    {
        _cinemashineCam = GetComponent<CinemachineVirtualCamera>();
        _mainCamera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        _settings = GameManager.Instance.Settings;
        UpdateCameraFOV(_customSettings.customFov);
        _inputHandler = InputHandler.Instance;
        ForceResetSelection();
        _detectableLayer = GameManager.Instance.Settings.AimAtObject? _detectableObjectLayer : _detectableTileLayer;
    }

    public void OnCamera(InputAction.CallbackContext callbackContext)
    {
        Vector2 rawInput = callbackContext.ReadValue<Vector2>();
        _mousePos = new Vector2(rawInput.x * yawSensitivity * Time.deltaTime,
                               rawInput.y * pitchSensitivity * Time.deltaTime);
    }

    void Update()
    {
        UpdateSelection(false);
    }

    private void ForceResetSelection()
    {
        UpdateSelection(true);
    }

    private void UpdateSelection(bool forceNewSelection = false)
    {
        if (_inputHandler == null || !_inputHandler.CanMove)
            return;

        if (!_isExternalPitchForced)
        {
            _yRotation -= _mousePos.y;
            _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);
        }

        Quaternion baseRotation = Quaternion.Euler(_yRotation, 0f, 0f);
        transform.localRotation = Quaternion.Slerp(baseRotation, _externalRotationInfluence, _rotationInfluenceAmount);

        float yawInput = _mousePos.x;

        float targetYaw = _playerTransform.eulerAngles.y + yawInput;
        float newYaw = Mathf.LerpAngle(targetYaw, _externalYawInfluence, _yawInfluenceAmount);

        _playerTransform.rotation = Quaternion.Euler(0f, newYaw, 0f);

        if (!GameManager.Instance.IsUIRubiksCubeEnabled)
            return;

        RaycastHit _raycastInfo;

        if (GameManager.Instance.Settings.AimAtObject)
        {
            if (Physics.Raycast(transform.position, transform.forward, out _raycastInfo, _maxDistance, _detectableLayer))
            {
                GameObject o = _raycastInfo.collider.gameObject;

                var cube = o.GetComponentInParent<SelectionCube>();
                if (cube)
                {
                    if (cube.name == "MiddleZone")
                    {
                        List<Transform> cubes = rubiksCubeController.ControlledScript.GetCubesFromFace(cube.transform, rubiksCubeController.SelectedSlice);

                        Transform middleCube = cubes.FirstOrDefault(x => x.name.Contains("Middle"));
                        if (!middleCube)
                            return;
                        Tile tile = middleCube.GetComponentInChildren<Tile>();

                        if (tile)
                        {
                            _oldTile = tile.transform;
                        }
                    }
                    else
                    {
                        var tile = cube.GetComponentInChildren<Tile>();

                        if (tile)
                        {
                            _oldTile = tile.transform;
                        }
                    }
                }
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, transform.forward, out _raycastInfo, _maxDistance, _detectableTileLayer))
            {
                GameObject collider = _raycastInfo.collider.gameObject;
                _oldTile = collider.transform;
            }
        }

        if (_oldTile == null || rubiksCubeController == null || _oldTile.parent == null)
            return;

        if (forceNewSelection)
            rubiksCubeController.SetActualCube(_oldTile.parent);
        else
        {
            if (rubiksCubeController.ActualFace == null || rubiksCubeController.ActualFace.transform != _oldTile.parent)
            {
                EventManager.TriggerCubeSwitchFace();
                rubiksCubeController.SetActualCube(_oldTile.parent);
            }
        }

    }

    public float GetVerticalAngle()
    {
        return _yRotation;
    }

    public float PlayerTransformEulerY()
    {
        return _playerTransform.eulerAngles.y;
    }

    public void SetExternalPitch(float pitch, float influence)
    {
        _externalRotationInfluence = Quaternion.Euler(pitch, 0f, 0f);
        _rotationInfluenceAmount = influence;
        _yRotation = pitch;
        _isExternalPitchForced = true;
    }

    public void SetExternalYaw(float yaw, float influence)
    {
        _externalYawInfluence = yaw;
        _yawInfluenceAmount = influence;
    }

    public void ClearExternalInfluence()
    {
        _rotationInfluenceAmount = 0f;
        _yawInfluenceAmount = 0f;
        _isExternalPitchForced = false;
        _mousePos = Vector2.zero;

        _yRotation = NormalizePitchAngle(transform.localEulerAngles.x);
    }

    private float NormalizePitchAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;

        return Mathf.Clamp(angle, -90f, 90f);
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
        _cinemashineCam.m_Lens.FieldOfView = newFOV;
    }

    void UpdateCameraMouseSensitivity(float newCamMouseSen)
    {
        yawSensitivity = newCamMouseSen;
        pitchSensitivity = newCamMouseSen;
    }

    void ResetMousePosition()
    {
        _mousePos = Vector2.zero;
        _yRotation = 0.0f;
    }
}
