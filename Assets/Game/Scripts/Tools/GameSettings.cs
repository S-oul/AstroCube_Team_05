using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    // Player Properties
    public float CameraSensibilityJoystick => cameraSensibilityJoystick;
    public float CameraSensibilityMouse => cameraSensibilityMouse;
    public float PlayerMoveSpeed => _playerMoveSpeed;
    public float Gravity => _gravity;
    public float JumpHeight => _jumpHeight;
    public float CrouchSpeed => _crouchSpeed;
    public float CrouchHeight => _crouchHeight;
    public float SlippingMovementControl => _slippingMovementControl;
    public bool EnableGravityRotation => _enableGravityRotation;


    // Rubik's Cube Properties
    public float RubikscCubeAxisRotationDuration => _rubikscCubeAxisRotationDuration;
    public float UiRubikscCubeRotationDuration => _uiRubikscCubeRotationDuration;


    // Global Properties
    public float ResetDuration => _resetDuration;


    [Header("-- PLAYER --")]
    [SerializeField] private float cameraSensibilityJoystick = 1000f;
    [SerializeField] private float cameraSensibilityMouse = 100f;

    [Header("- Movement")]
    [SerializeField] float _playerMoveSpeed = 12f;
    [SerializeField] float _gravity = -20.0f;

    [Header("- Jump")]
    [SerializeField] float _jumpHeight = 1.0f;

    [Header("- Crouch")]
    [SerializeField, Range(0.0f, 1.0f)] float _crouchSpeed = 0.5f;
    [SerializeField, Range(0.0f, 1.0f)] float _crouchHeight = 0.5f;

    [Header("- Slipping")]
    [SerializeField][Range(0.0f, 0.1f)] float _slippingMovementControl = 0.01f;

    [Header("- GravityRotation")]
    [SerializeField] bool _enableGravityRotation = false;


    [Header("-- RUBIK'S CUBE --")]
    [SerializeField] private float _rubikscCubeAxisRotationDuration = 0.2f;
    [SerializeField] private float _uiRubikscCubeRotationDuration = 0.2f;

    [Header("-- GLOBAL --")]
    [SerializeField] private float _resetDuration = 2.0f;

}
