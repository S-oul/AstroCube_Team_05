using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] LayerMask _detectableTileLayer;
    [SerializeField] LayerMask _detectableObjectLayer;
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
    public void OnCamera(Vector2 vector2) //also used for NoClip
    {
        mousePos = vector2 * _cameraSensibilityMouse * Time.deltaTime;
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

        _yRotation -= mousePos.y;
        _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);

        if (_doReversedCam)
        {
            Vector3 forward = _playerTransform.forward;
            forward.y = 0; // Ignore vertical tilt if needed
            
            float angle = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
            float normalizeAngle = (angle < 0) ? angle + 360 : angle; // Normalize to 0-360
            bool isReversed = normalizeAngle >= 315 || normalizeAngle < 135;
            
            rubiksCubeController.CameraPlayerReversed = isReversed;
        }

        transform.localRotation = Quaternion.Euler(_yRotation, 0f, 0f);
        _playerTransform.Rotate(Vector3.up * mousePos.x);

        if (!GameManager.Instance.IsRubiksCubeEnabled)
            return;

        RaycastHit _raycastInfo;

        if (GameManager.Instance.Settings.AimAtObject)
        {
            if (Physics.Raycast(transform.position, transform.forward, out _raycastInfo, _maxDistance, _detectableObjectLayer))
            {
                GameObject o = _raycastInfo.collider.gameObject;

                var cube = o.GetComponentInParent<SelectionCube>();
                if (cube)
                {
                    if(cube.name == "MiddleZone")
                    {
                        List<Transform> cubes = rubiksCubeController.ControlledScript.GetCubesFromFace(cube.transform, rubiksCubeController.SelectedSlice);


                        Transform middleCube = cubes.First(x => x.name.Contains("Middle"));
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
                GameObject o = _raycastInfo.collider.gameObject;
                _oldTile = o.transform;
            }
        }

        if (_oldTile == null || rubiksCubeController == null || _oldTile.parent == null)
            return;

        if(forceNewSelection)
            rubiksCubeController.SetActualCube(_oldTile.parent);
        else
        {
            if (rubiksCubeController.ActualFace == null || rubiksCubeController.ActualFace.transform != _oldTile.parent)
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
