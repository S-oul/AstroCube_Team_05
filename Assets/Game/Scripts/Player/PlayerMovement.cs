using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Scene Requirments")]
    [SerializeField] CharacterController _controller;
    [SerializeField] Transform _camera;
    [SerializeField] Transform _floorCheck;
    [SerializeField] LayerMask _floorLayer;

    [Header("Jump")]
    [SerializeField] bool _canJump = true;

    [Header("Crouch")]
    [SerializeField] bool _canCrouch = true;

    Vector3 _gravityDirection;

    float _floorDistance = 0.1f;

    float _currentMoveSpeed;
    Vector3 _verticalVelocity;
    Vector3 _horizontalVelocity; 
    bool _isGrounded;

    float _defaultCameraHeight;
    float _defaultControllerHeight;
    Vector3 _defaultControllerCenter;

    float xInput = 0;
    float zInput = 0;
    bool jumpInput = false;
    bool crouchInput = false;

    bool _isSlipping = false;
    Vector3 _pastHorizontalVelocity;
    GameSettings _gameSettings;

    // HeadBobbing
    float _walkingDuration;
    float _startWalkingDuration;
    float _stopWalkingDuration;
    bool _isWalking;

    public float defaultSpeed { get; private set; }


    void Start()
    {
        _gameSettings = GameManager.Instance.Settings;
        GetComponent<DetectNewParent>().enabled = _gameSettings.EnableGravityRotation;
        
        _defaultCameraHeight = _camera.transform.localPosition.y;
        _defaultControllerHeight = _controller.height;
        _defaultControllerCenter = _controller.center;

        defaultSpeed = _gameSettings.PlayerMoveSpeed;
        _currentMoveSpeed = defaultSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // collect player inputs
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        if (_canJump) jumpInput = Input.GetButtonDown("Jump");
        if (_canCrouch) crouchInput = Input.GetKey(KeyCode.LeftShift);

        // check player state
        _isGrounded = Physics.CheckSphere(_floorCheck.position, _floorDistance, _floorLayer);

        // apply gravity 
        _gravityDirection = transform.up;
        _verticalVelocity += _gravityDirection * _gameSettings.Gravity * Time.deltaTime;
        if (_isGrounded)
        {
            _verticalVelocity = Vector3.zero;
        }

        // movePlayer (walking around)
        if (_isSlipping ) _pastHorizontalVelocity = _horizontalVelocity;
        _horizontalVelocity = transform.right * xInput + transform.forward * zInput;

        if (_isSlipping)
        {
            _horizontalVelocity = _horizontalVelocity * _gameSettings.SlippingMovementControl + _pastHorizontalVelocity;

            //clamp
            _horizontalVelocity.x = _horizontalVelocity.x > 1 ? 1 : _horizontalVelocity.x;
            _horizontalVelocity.x = _horizontalVelocity.x < -1 ? -1 : _horizontalVelocity.x;
            _horizontalVelocity.z = _horizontalVelocity.z > 1 ? 1 : _horizontalVelocity.z;
            _horizontalVelocity.z = _horizontalVelocity.z < -1 ? -1 : _horizontalVelocity.z;
        }

        // jump
        if (jumpInput && _isGrounded)
        {
            _verticalVelocity = transform.up * Mathf.Sqrt(_gameSettings.JumpHeight * -2f * _gameSettings.Gravity);
        }

        // crouch
        Vector3 newCamPos;
        if (crouchInput)
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

        // apply calculated
        _controller.Move(_horizontalVelocity * 
                         (crouchInput ? _currentMoveSpeed : _currentMoveSpeed / _gameSettings.CrouchSpeed) * 
                         Time.deltaTime);
        _controller.Move(_verticalVelocity *Time.deltaTime);

        // gravity rotation
        if (_gameSettings.EnableGravityRotation == false && transform.parent != null)
        {
            GetComponent<DetectNewParent>().enabled = false;
            transform.SetParent(null);
        }        
        
        if (_gameSettings.EnableGravityRotation == true && transform.parent == null)
        {
            Transform parentChangerChild = transform.GetChild(3);
            GetComponent<DetectNewParent>().enabled = true;
        }

        _ApplyCameraHeight(newCamPos.y);
    }

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
                newCameraHeight = Vector3.up * (currentDefaultHeight + _gameSettings.HeadBobbingCurve.Evaluate((_walkingDuration * _gameSettings.HeadBobbingSpeed) % 1) * _gameSettings.HeadBobbingAmount);
            }
        }
        else
        {
            _walkingDuration = 0.0f;
            if(_stopWalkingDuration <= _gameSettings.StopWalkingTransitionDuration)
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
}
