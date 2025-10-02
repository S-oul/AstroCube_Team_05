using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Scene Requirements")]
    [SerializeField] Rigidbody _rb;
    [SerializeField] Transform _camera;
    [SerializeField] Transform _floorCheck;
    [SerializeField] LayerMask _floorLayer;

    bool _hasGravity = true;

    [Header("Jump")]
    [SerializeField] bool _canJump = true;

    [Header("Crouch")]
    [SerializeField] bool _canCrouch = true;

    [Header("Slipping")]
    [SerializeField] [Range(0.0f, 0.1f)] float _slippingMovementControl = 0.01f;

    [Header("GravityRotation")]
    [SerializeField] bool _enableGravityRotation = true;

    [Header("NoClip")]
    [SerializeField] bool _resetRotationWhenNoClip = false;

    bool _canMove = true;
    Vector3 _gravityDirection;

    private GroundTypePlayerIsWalkingOn _currentGroundType = GroundTypePlayerIsWalkingOn.Default;


    float _floorDistance = 0.1f;

    float _currentMoveSpeed;
    float _currentMoveSpeedFactor = 1f;
    Vector3 _horizontalVelocity;
    bool _isGrounded;

    float _defaultCameraHeight;

    float _xInput = 0;
    float _zInput = 0;
    float _yInput = 0; //noclip
    bool _jumpInput = false;
    bool _crouchInput = false;

    bool _isSlipping = false;
    Vector3 _pastHorizontalVelocity;
    GameSettings _gameSettings;

    // HeadBobbing
    float _walkingDuration;
    float _startWalkingDuration;
    float _stopWalkingDuration;
    bool _isWalking;

    Vector3 newCamPos;

    Vector3 _externallyAppliedMovement = Vector3.zero;

    public bool isOnDefaultGround;

    public float defaultSpeed { get; private set; }
    public bool HasGravity { get => _hasGravity; set => _hasGravity = value; }

    private float _timerBeforeNextStep = 0;
    public float _timerTNextStep = 1;
    
    //slope handler
    private RaycastHit _currentSlope;

    private void OnEnable()
    {
        EventManager.OnStartCubeRotation += DisableMovement;
        EventManager.OnEndCubeRotation += EnableMovement;
        EventManager.OnEndCubeRotation += UnParentPlayer;

    }

    private void OnDisable()
    {
        EventManager.OnStartCubeRotation -= DisableMovement;
        EventManager.OnEndCubeRotation -= EnableMovement;
        EventManager.OnEndCubeRotation -= UnParentPlayer;

    }

    public void EnableMovement() => _canMove = true;
    public void DisableMovement() => _canMove = false;

    public void UnParentPlayer() => transform.SetParent(null);
    void Start()
    {
        _gameSettings = GameManager.Instance.Settings;
        GetComponent<DetectNewParent>().DoGravityRotation = _gameSettings.EnableGravityRotation;

        _defaultCameraHeight = _camera.transform.localPosition.y;

        defaultSpeed = _gameSettings.PlayerMoveSpeed;
        _currentMoveSpeed = defaultSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_currentSlope.normal);
        
        if (!_canMove) return;
        /*
        // collect player inputs
        _xInput = Input.GetAxis("Horizontal");
        _zInput = Input.GetAxis("Vertical");
        if (_canJump) _jumpInput = Input.GetButtonDown("Jump");
        if (_canCrouch) _crouchInput = Input.GetKey(KeyCode.LeftShift);
        */

        //check player state
        _isGrounded = Physics.CheckSphere(_floorCheck.position, _floorDistance, _floorLayer);

        //apply gravity
        if (_hasGravity) {
            _gravityDirection = transform.up;
        }

        //_gravityDirection = transform.up;
        //_verticalVelocity = _gravityDirection * _gameSettings.Gravity * Time.deltaTime;

        // movePlayer (walking around)
        if (_isSlipping) _pastHorizontalVelocity = _horizontalVelocity;
        _horizontalVelocity = transform.right * _xInput + transform.forward * _zInput;

        if (_isSlipping) {
            _horizontalVelocity = _horizontalVelocity * _gameSettings.SlippingMovementControl + _pastHorizontalVelocity;

            //clamp
            _horizontalVelocity.x = _horizontalVelocity.x > 1 ? 1 : _horizontalVelocity.x;
            _horizontalVelocity.x = _horizontalVelocity.x < -1 ? -1 : _horizontalVelocity.x;
            _horizontalVelocity.z = _horizontalVelocity.z > 1 ? 1 : _horizontalVelocity.z;
            _horizontalVelocity.z = _horizontalVelocity.z < -1 ? -1 : _horizontalVelocity.z;
        }
        
        // jump
        if (_jumpInput && _isGrounded)
        {
            Debug.Log("jump");
            _rb.AddForce(transform.up * (_gameSettings.JumpHeight * 0.75f), ForceMode.Impulse);
        }

        _jumpInput = false;
        _crouchInput = false;

        // no clip
        _horizontalVelocity += transform.up * _yInput;
        if (_OnSlope())
        {
            _horizontalVelocity = Vector3.ProjectOnPlane(_horizontalVelocity, _currentSlope.normal) * Mathf.Sqrt(2.0f);
        }
        if (_IsInFrontOfStep(out float newYLevel))
        {
            transform.position += new Vector3(0, newYLevel, 0) + _horizontalVelocity * 0.05f;
        }

        // apply calculated Movement
        _rb.velocity += _horizontalVelocity * (_gameSettings.PlayerMoveSpeed * Time.deltaTime) + _externallyAppliedMovement;
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _gameSettings.PlayerMoveSpeed);
        if (!_isGrounded && !_OnSlope())
        {
            _rb.velocity += Vector3.down * _gameSettings.Gravity;
        }

        _ApplyCameraHeight(newCamPos.y);
        ExecuteFootStep();
    }

    void ExecuteFootStep()
    {
        if (_isWalking) {
            _timerBeforeNextStep += Time.deltaTime;
            EventManager.TriggerPlayerFootSteps(_currentGroundType);

        }
        else {
            _timerBeforeNextStep = 0;
        }

        float stepDuration = _timerTNextStep / _currentMoveSpeedFactor;
        if (_timerBeforeNextStep >= stepDuration) {
            _timerBeforeNextStep = 0;
            UpdateGroundType();
            EventManager.TriggerPlayerFootSteps(_currentGroundType);

        }
    }

    #region Inputs

    public void ActionMovement(Vector2 direction)
    {
        //Debug.Log("actionMovement direction is " + direction);
        _xInput = direction.x;
        _zInput = direction.y;
    }

    public void ActionJump()
    {
        _jumpInput = _canJump;
    }

    public void ActionCrouch()
    {
        _crouchInput = _canCrouch;
    }

    #endregion

    public void SetSpeed(float newSpeed)
    {
        _currentMoveSpeed = newSpeed * Time.deltaTime;
    }

    public void SetSpeedFactor(float speedFactor)
    {
        _currentMoveSpeedFactor = speedFactor;
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
        Vector3 newCameraHeight;
        _isWalking = _horizontalVelocity != Vector3.zero;
        if (_isWalking && !_isSlipping) {
            if (_startWalkingDuration <= _gameSettings.StartWalkingTransitionDuration) {
                _stopWalkingDuration = 0.0f;
                _startWalkingDuration += Time.deltaTime;
                newCameraHeight = Vector3.up * Mathf.Lerp(_camera.transform.localPosition.y,
                    currentDefaultHeight + _gameSettings.HeadBobbingCurve.Evaluate(0.0f) * _gameSettings.HeadBobbingAmount,
                    _startWalkingDuration / _gameSettings.StartWalkingTransitionDuration);
            } else {
                _walkingDuration += Time.deltaTime;
                newCameraHeight = Vector3.up * (currentDefaultHeight + _gameSettings.HeadBobbingCurve.Evaluate((_walkingDuration * _gameSettings.HeadBobbingSpeed) % 1) * _gameSettings.HeadBobbingAmount);
            }
        } else {
            _walkingDuration = 0.0f;
            if (_stopWalkingDuration <= _gameSettings.StopWalkingTransitionDuration) {
                _startWalkingDuration = 0.0f;
                _stopWalkingDuration += Time.deltaTime;
                newCameraHeight = Vector3.up * Mathf.Lerp(_camera.transform.localPosition.y,
                    currentDefaultHeight,
                    _stopWalkingDuration / _gameSettings.StopWalkingTransitionDuration);
            } else {
                newCameraHeight = Vector3.up * currentDefaultHeight;
            }
        }

        _camera.transform.localPosition = newCameraHeight;
    }

    //NoClip
    public void ActivateNoClip()
    {
        GetComponent<CharacterController>().excludeLayers = Physics.AllLayers;
        _hasGravity = false;
        _rb.velocity = Vector3.zero;
        transform.SetParent(null);
        if (_resetRotationWhenNoClip) {
            transform.rotation = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
        }
    }

    public void DeactivateNoClip()
    {
        GetComponent<CharacterController>().excludeLayers = 0;
        _hasGravity = true;
    }

    public void ActionVerticalMovement(float direction)
    {
        _yInput = direction;
    }

    public void SetExternallyAppliedMovement(Vector3 directon, float speed = 1)
    {
        _externallyAppliedMovement = directon * speed;
    }

    private void UpdateGroundType()
    {
        Ray ray = new Ray(_floorCheck.position, -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, _floorDistance + 0.2f, _floorLayer))
        {
            string groundTag = hit.collider.tag;
            //Debug.Log("Ground tag detected: " + groundTag);
            switch (groundTag)
            {
                case "Floor_Default":
                default:
                    _currentGroundType = GroundTypePlayerIsWalkingOn.Default;
                    break;
                case "Floor_Grass":
                    _currentGroundType = GroundTypePlayerIsWalkingOn.Grass;
                    break;
            }
            //Debug.Log("Ground type detected: " + groundTag);
        }
        else
        {
            //Debug.Log("No ground or tag detected , setting to default.");
            _currentGroundType = GroundTypePlayerIsWalkingOn.Default;
        }
    }

    private bool _OnSlope()
    {
        float threshold = _currentSlope.normal != Vector3.up ? 0.0f : 0.2f;
        
        if (Physics.Raycast(transform.position + transform.forward * threshold, -transform.up, out _currentSlope, 100, LayerMask.GetMask("Floor")))
        {
            return _currentSlope.normal != Vector3.up;
        }
        return false;
    }
    
    private bool _IsInFrontOfStep(out float stepHeight)
    {
        stepHeight = 0.0f;
        if (Physics.Raycast(transform.position + _horizontalVelocity * 0.6f, -transform.up, out RaycastHit hit, 100, LayerMask.GetMask("Floor")))
        {
            stepHeight = hit.point.y - _floorCheck.position.y + 0.03f;
            return stepHeight > 0.05f && stepHeight < _gameSettings.StepHeightMax;
        }
        return false;
    }

}