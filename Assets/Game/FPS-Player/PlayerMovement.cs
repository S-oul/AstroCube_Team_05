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

    [Header("Jump")]
    [SerializeField] bool _canJump = true;
    [SerializeField] float _jumpHeight = 10;

    [Header("Crouch")]
    [SerializeField] bool _canCrouch = true;
    [SerializeField, Range(0.0f, 1.0f)] float _crouchSpeed;
    [SerializeField, Range(0.0f, 1.0f)] float _crouchHeight;

    Vector3 _gravityDirection;

    float _floorDistance = 0.1f;

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

    public float defaultSpeed { get; private set; }

    void Start()
    {
        _defaultCameraHeight = _camera.transform.localPosition.y;
        _defaultControllerHeight = _controller.height;
        _defaultControllerCenter = _controller.center;

        defaultSpeed = _speed;
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
        _verticalVelocity += _gravityDirection * _gravity * Time.deltaTime;
        if (_isGrounded)
        {
            _verticalVelocity = Vector3.zero;
        }

        // movePlayer (walking around)
        _horizontalVelocity = transform.right * xInput + transform.forward * zInput;

        // jump
        if (jumpInput && _isGrounded)
        {
            _verticalVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        // crouch
        if (crouchInput)
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

        // apply calculated movements
        _controller.Move(_horizontalVelocity * 
                         (crouchInput ? _speed : _speed/_crouchSpeed) * 
                         Time.deltaTime);
        _controller.Move(_verticalVelocity *Time.deltaTime);
    }

    public void setSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    public void setSpeedToDefault()
    {
        _speed = defaultSpeed;
    }

    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log("collided with: " + other.gameObject.name);
        if (other.gameObject.tag != "floor") return;
        transform.SetParent(other.gameObject.transform);
        //Debug.Log("new parent named: " + other.gameObject.name);
    }
}
