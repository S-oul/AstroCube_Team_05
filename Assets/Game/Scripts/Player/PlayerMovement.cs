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

    [Header("Movement")]
    [SerializeField] float _speed = 12f;
    [SerializeField] float _gravity = -9.81f;
    bool _hasGravity = true;

    [Header("Jump")]
    [SerializeField] bool _canJump = true;
    [SerializeField] float _jumpHeight = 10;

    [Header("Crouch")]
    [SerializeField] bool _canCrouch = true;
    [SerializeField, Range(0.0f, 1.0f)] float _crouchSpeed;
    [SerializeField, Range(0.0f, 1.0f)] float _crouchHeight;

    [Header("Slipping")]
    [SerializeField][Range(0.0f, 0.1f)] float _slippingMovementControl = 0.01f;

    [Header("GravityRotation")]
    [SerializeField] bool _enableGravityRotation = true;

    [Header("NoClip")]
    [SerializeField] bool _resetRotationWhenNoClip = false;

    Vector3 _gravityDirection;

    float _floorDistance = 0.1f;

    Vector3 _verticalVelocity;
    Vector3 _horizontalVelocity; 
    bool _isGrounded;

    float _defaultCameraHeight;
    float _defaultControllerHeight;
    Vector3 _defaultControllerCenter;

    float _xInput = 0;
    float _zInput = 0;
    float _yInput = 0;//noclip
    bool _jumpInput = false;
    bool _crouchInput = false;


    bool _isSlipping = false;
    Vector3 _pastHorizontalVelocity;
    public float defaultSpeed { get; private set; }

    void Start()
    {
        GetComponent<DetectNewParent>().enabled = _enableGravityRotation;
        
        _defaultCameraHeight = _camera.transform.localPosition.y;
        _defaultControllerHeight = _controller.height;
        _defaultControllerCenter = _controller.center;

        defaultSpeed = _speed;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // collect player inputs
        _xInput = Input.GetAxis("Horizontal");
        _zInput = Input.GetAxis("Vertical");
        if (_canJump) _jumpInput = Input.GetButtonDown("Jump");
        if (_canCrouch) _crouchInput = Input.GetKey(KeyCode.LeftShift);
        */

        // check player state
        _isGrounded = Physics.CheckSphere(_floorCheck.position, _floorDistance, _floorLayer);

        // apply gravity 
        if (_hasGravity)
        {
            _gravityDirection = transform.up;
            _verticalVelocity += _gravityDirection * _gravity * Time.deltaTime;
            if (_isGrounded)
            {
                _verticalVelocity = Vector3.zero;
            }
        }

        // movePlayer (walking around)
        if (_isSlipping ) _pastHorizontalVelocity = _horizontalVelocity;
        _horizontalVelocity = transform.right * _xInput + transform.forward * _zInput;

        if (_isSlipping)
        {
            _horizontalVelocity = _horizontalVelocity * _slippingMovementControl + _pastHorizontalVelocity;

            //clamp
            _horizontalVelocity.x = _horizontalVelocity.x > 1 ? 1 : _horizontalVelocity.x;
            _horizontalVelocity.x = _horizontalVelocity.x < -1 ? -1 : _horizontalVelocity.x;
            _horizontalVelocity.z = _horizontalVelocity.z > 1 ? 1 : _horizontalVelocity.z;
            _horizontalVelocity.z = _horizontalVelocity.z < -1 ? -1 : _horizontalVelocity.z;
        }

        // jump
        if (_jumpInput && _isGrounded)
        {
            _verticalVelocity = transform.up * Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        } _jumpInput = false;

        // crouch
        if (_crouchInput)
        {
            _controller.height *= _crouchHeight;
            _controller.center = Vector3.up * _crouchHeight * -1;

            Vector3 newCamPos = _camera.transform.localPosition;
            newCamPos.y = _defaultCameraHeight * _crouchHeight;
            _camera.transform.localPosition = newCamPos;
        }
        else
        {
            _controller.height = _defaultControllerHeight;
            _controller.center = _defaultControllerCenter;
            Vector3 newCamPos = _camera.transform.localPosition;
            newCamPos.y = _defaultCameraHeight;
            _camera.transform.localPosition = newCamPos;
        }
        _crouchInput = false;

        // no clip
        _horizontalVelocity += transform.up * _yInput;

        // apply calculated
        _controller.Move(_horizontalVelocity * 
                         (_crouchInput ? _speed : _speed/_crouchSpeed) * 
                         Time.deltaTime);
        _controller.Move(_verticalVelocity * Time.deltaTime);

        // gravity rotation
        if (_enableGravityRotation == false && transform.parent != null)
        {
            GetComponent<DetectNewParent>().enabled = false;
            transform.SetParent(null);
        }        
        
        if (_enableGravityRotation == true && transform.parent == null)
        {
            Transform parentChangerChild = transform.GetChild(3);
            GetComponent<DetectNewParent>().enabled = true;
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
        _speed = newSpeed;
    }

    public void SetSpeedToDefault()
    {
        _speed = defaultSpeed;
    }

    public void SetSlippingState(bool isSlipping)
    {
        _isSlipping = isSlipping;
    }

    //NoClip
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
    public void ActionVerticalMovement(float direction)
    {
        _yInput = direction;
    }
}
