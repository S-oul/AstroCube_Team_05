using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    // Player Properties
    public float PlayerMoveSpeed => _playerMoveSpeed;
    public float Gravity => _gravity;
    public float JumpHeight => _jumpHeight;
    public float CrouchSpeed => _crouchSpeed;
    public float CrouchHeight => _crouchHeight;
    public float SlippingMovementControl => _slippingMovementControl;
    public float HeadBobbingAmount => _headBobbingAmount;
    public float HeadBobbingSpeed => _headBobbingSpeed;
    public float StartWalkingTransitionDuration => _startWalkingTransitionDuration;
    public float StopWalkingTransitionDuration => _stopWalkingTransitionDuration;
    public AnimationCurve HeadBobbingCurve => _headBobbingCurve;
    public bool EnableGravityRotation => _enableGravityRotation;


    // Rubik's Cube Properties
    public float RubikscCubeAxisRotationDuration => _rubikscCubeAxisRotationDuration;
    public float PreviewRubikscCubeAxisRotationDuration => _previewRubikscCubeAxisRotationDuration;
    public float UiRubikscCubeRotationDuration => _uiRubikscCubeRotationDuration;
    public AnimationCurve AnimationSpeedCurve => _AnimationSpeedCurve;
    public Vector4 RubiksCubeRotationScreenshakeSettings => _rubiksCubeRotationscreenshakeSettings;


    // Global Properties
    public float ResetDuration => _resetDuration;
    public float StencilFadeInDuration => _stencilFadeInDuration;
    public float StencilFadeOutDuration => _stencilFadeOutDuration;
    public float StencilStayDuration => _stencilStayDuration;


    [Header("-- PLAYER --")]

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

    [Header("- HeadBobbing Walking")]
    [SerializeField] float _headBobbingAmount = 2.0f;
    [SerializeField] float _headBobbingSpeed = 3.0f;
    [SerializeField] float _startWalkingTransitionDuration = 0.5f;
    [SerializeField] float _stopWalkingTransitionDuration = 0.5f;
    [SerializeField] AnimationCurve _headBobbingCurve;

    [Header("- HeadBobbing Stairs")] // Not implemented yet
    [SerializeField] float _headBobbingStairsAmount = 2.0f;
    [SerializeField] float _headBobbingStairsSpeed = 3.0f;
    [SerializeField] float _startStairsTransitionDuration = 0.5f;
    [SerializeField] float _stopStairsTransitionDuration = 0.5f;
    [SerializeField] AnimationCurve _headBobbingStairsCurve;


    [Header("- GravityRotation")]
    [SerializeField] bool _enableGravityRotation = false;

    [Header("-- RUBIK'S CUBE --")]
    [SerializeField] private float _rubikscCubeAxisRotationDuration = 0.2f;
    [SerializeField] private float _previewRubikscCubeAxisRotationDuration = 0.2f;
    [SerializeField] private float _uiRubikscCubeRotationDuration = 0.2f;

    [SerializeField] AnimationCurve _AnimationSpeedCurve = new AnimationCurve();

    [SerializeField][InfoBox("Duration, Strength, Vibrato, Randomness", EInfoBoxType.Normal)] private Vector4 _rubiksCubeRotationscreenshakeSettings = new(2.0f, 0.4f, 10.0f, 90.0f);

    [Header("-- GLOBAL --")]
    [SerializeField] private float _resetDuration = 2.0f;


    [Header("- Show Exit")]
    [SerializeField] float _stencilFadeInDuration = 0.5f;
    [SerializeField] float _stencilFadeOutDuration = 0.5f;
    [SerializeField] float _stencilStayDuration = 2.0f;

}
