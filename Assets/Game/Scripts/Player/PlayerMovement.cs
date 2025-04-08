using AK.Wwise;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Scene Requirments")]
    [SerializeField] CharacterController _controller;
    [SerializeField] Transform _camera;
    [SerializeField] Transform _floorCheck;
    [SerializeField] LayerMask _floorLayer;

    [Header("Movement")]
    [SerializeField] float _speed = 12f;
    [SerializeField] float _gravity = -9.81f;
    bool _hasGravity = true;

    [Header("Jump")]
    [SerializeField] bool _canJump = true;

    [Header("Crouch")]
    [SerializeField] bool _canCrouch = true;

    [Header("Slipping")]
    [SerializeField][Range(0.0f, 0.1f)] float _slippingMovementControl = 0.01f;

    [Header("GravityRotation")]
    [SerializeField] bool _enableGravityRotation = true;

    [Header("NoClip")]
    [SerializeField] bool _resetRotationWhenNoClip = false;

    [Header("WISE")]
    [SerializeField] AK.Wwise.Event AKWiseEvent;

    [Header("Laser Rotation")]
    [SerializeField] private GameObject laserToRotate;
    [SerializeField] private float rotationSpeed = 50f;

    private bool isPlayerLocked = false;
    public bool IsPlayerLocked
    {
        get => isPlayerLocked;
        set => isPlayerLocked = value;
    }

    Vector3 _gravityDirection;
    float _floorDistance = 0.1f;
    float _currentMoveSpeed;
    Vector3 _verticalVelocity;
    Vector3 _horizontalVelocity;
    bool _isGrounded;

    float _defaultCameraHeight;
    float _defaultControllerHeight;
    Vector3 _defaultControllerCenter;

    float _xInput = 0;
    float _zInput = 0;
    float _yInput = 0;
    bool _jumpInput = false;
    bool _crouchInput = false;

    bool _isSlipping = false;
    Vector3 _pastHorizontalVelocity;
    GameSettings _gameSettings;

    float _walkingDuration;
    float _startWalkingDuration;
    float _stopWalkingDuration;
    bool _isWalking;

    Vector3 newCamPos;
    public float defaultSpeed { get; private set; }

    private float _timerBeforeNextStep = 0;
    public float _timerTNextStep = 1;

    void Start()
    {
        _gameSettings = GameManager.Instance.Settings;
        GetComponent<DetectNewParent>().DoGravityRotation = _gameSettings.EnableGravityRotation;

        _defaultCameraHeight = _camera.transform.localPosition.y;
        _defaultControllerHeight = _controller.height;
        _defaultControllerCenter = _controller.center;

        defaultSpeed = _gameSettings.PlayerMoveSpeed;
        _currentMoveSpeed = defaultSpeed;
    }

    void Update()
    {
        // Bouton B => raycast
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            if (!IsPlayerLocked)
            {
                ShootRaycastForLaserUnlock();
            }
            else
            {
                isPlayerLocked = false;
            }
        }

        if (isPlayerLocked)
        {
            RotateLaserWithJoystick();
            _horizontalVelocity = Vector3.zero;
            _verticalVelocity = Vector3.zero;
            _controller.Move(Vector3.zero);
            return;
        }

        _isGrounded = Physics.CheckSphere(_floorCheck.position, _floorDistance, _floorLayer);

        if (_hasGravity)
        {
            _gravityDirection = transform.up;
            _verticalVelocity += _gravityDirection * _gravity * Time.deltaTime;
            if (_isGrounded)
            {
                _verticalVelocity = Vector3.zero;
            }
        }

        if (_isSlipping) _pastHorizontalVelocity = _horizontalVelocity;
        _horizontalVelocity = transform.right * _xInput + transform.forward * _zInput;

        if (_isSlipping)
        {
            _horizontalVelocity = _horizontalVelocity * _gameSettings.SlippingMovementControl + _pastHorizontalVelocity;
            _horizontalVelocity.x = Mathf.Clamp(_horizontalVelocity.x, -1f, 1f);
            _horizontalVelocity.z = Mathf.Clamp(_horizontalVelocity.z, -1f, 1f);
        }

        if (_jumpInput && _isGrounded)
        {
            _verticalVelocity = transform.up * Mathf.Sqrt(_gameSettings.JumpHeight * -2f * _gameSettings.Gravity);
        }
        _jumpInput = false;

        if (_crouchInput)
        {
            _controller.height *= _gameSettings.CrouchHeight;
            _controller.center = Vector3.up * _gameSettings.CrouchHeight * -1;
            newCamPos = _camera.transform.localPosition;
            newCamPos.y = _defaultCameraHeight * _gameSettings.CrouchHeight;
        }
        else
        {
            _controller.height = _defaultControllerHeight;
            _controller.center = _defaultControllerCenter;
            newCamPos = _camera.transform.localPosition;
            newCamPos.y = _defaultCameraHeight;
        }
        _crouchInput = false;

        _horizontalVelocity += transform.up * _yInput;

        _controller.Move(_horizontalVelocity *
                         (_crouchInput ? _currentMoveSpeed : _currentMoveSpeed / _gameSettings.CrouchSpeed) * Time.deltaTime);
        _controller.Move(_verticalVelocity * Time.deltaTime);

        _ApplyCameraHeight(newCamPos.y);
        ExecuteFootStep();
    }

    private void RotateLaserWithJoystick()
    {
        if (laserToRotate == null) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Rotation type tourelle : X haut/bas, Y gauche/droite
        Vector3 rotation = new Vector3(-vertical, horizontal, 0);
        laserToRotate.transform.Rotate(rotation * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void ShootRaycastForLaserUnlock()
    {
        if (_camera == null) return;

        Ray ray = new Ray(_camera.position, _camera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);

            if (hit.collider.CompareTag("Lazer"))
            {
                IsPlayerLocked = true;
                Debug.Log("Laser target hit — Player unlocked!");
            }
        }

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 2f);
    }

    void ExecuteFootStep()
    {
        if (_isWalking)
        {
            _timerBeforeNextStep += Time.deltaTime;
        }
        else
        {
            _timerBeforeNextStep = 0;
        }

        if (_timerBeforeNextStep >= _timerTNextStep)
        {
            _timerBeforeNextStep = 0;
            AKWiseEvent.Post(gameObject);
        }
    }

    #region Inputs
    public void ActionMovement(Vector2 direction)
    {
        if (isPlayerLocked) return;

        _xInput = direction.x;
        _zInput = direction.y;
    }

    public void ActionJump()
    {
        if (isPlayerLocked) return;

        _jumpInput = _canJump;
    }

    public void ActionCrouch()
    {
        if (isPlayerLocked) return;

        _crouchInput = _canCrouch;
    }

    public void ActionVerticalMovement(float direction)
    {
        if (isPlayerLocked) return;

        _yInput = direction;
    }
    #endregion

    public void SetSpeed(float newSpeed)
    {
        _currentMoveSpeed = newSpeed;
    }

    public void SetSpeedToDefault()
    {
        _currentMoveSpeed = defaultSpeed;
    }

    public void SetSlippingState(bool isSlipping)
    {
        _isSlipping = isSlipping;
    }

    private void _ApplyCameraHeight(float currentDefaultHeight)
    {
        if (isPlayerLocked)
            return;

        Vector3 newCameraHeight;
        _isWalking = _horizontalVelocity != Vector3.zero;
        if (_isWalking && !_isSlipping)
        {
            if (_startWalkingDuration <= _gameSettings.StartWalkingTransitionDuration)
            {
                _stopWalkingDuration = 0.0f;
                _startWalkingDuration += Time.deltaTime;
                newCameraHeight = Vector3.up * Mathf.Lerp(_camera.transform.localPosition.y,
                        currentDefaultHeight + _gameSettings.HeadBobbingCurve.Evaluate(0.0f) * _gameSettings.HeadBobbingAmount,
                        _startWalkingDuration / _gameSettings.StartWalkingTransitionDuration);
            }
            else
            {
                _walkingDuration += Time.deltaTime;
                newCameraHeight = Vector3.up * (currentDefaultHeight +
                    _gameSettings.HeadBobbingCurve.Evaluate((_walkingDuration * _gameSettings.HeadBobbingSpeed) % 1) * _gameSettings.HeadBobbingAmount);
            }
        }
        else
        {
            _walkingDuration = 0.0f;
            if (_stopWalkingDuration <= _gameSettings.StopWalkingTransitionDuration)
            {
                _startWalkingDuration = 0.0f;
                _stopWalkingDuration += Time.deltaTime;
                newCameraHeight = Vector3.up * Mathf.Lerp(_camera.transform.localPosition.y,
                        currentDefaultHeight,
                        _stopWalkingDuration / _gameSettings.StopWalkingTransitionDuration);
            }
            else
            {
                newCameraHeight = Vector3.up * currentDefaultHeight;
            }
        }
        _camera.transform.localPosition = newCameraHeight;
    }

    public void ActivateNoClip()
    {
        GetComponent<CharacterController>().excludeLayers = Physics.AllLayers;
        _hasGravity = false;
        _verticalVelocity = Vector3.zero;
        _controller.Move(Vector3.zero);
        transform.SetParent(null);
        if (_resetRotationWhenNoClip)
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
        }
    }

    public void DeactivateNoClip()
    {
        GetComponent<CharacterController>().excludeLayers = 0;
        _hasGravity = true;
    }
}
