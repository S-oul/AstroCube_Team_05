using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystemManager;

public class InputSystemManager : MonoBehaviour
{
    public static InputSystemManager Instance => instance;
    private static InputSystemManager instance;

    public EInputMode CurrentInputMode => _currentInputMode;
    private EInputMode _currentInputMode;

    public PlayerInput PlayerInputs => _playerInputs;
    private PlayerInput _playerInputs;

    public enum EInputMode
    {
        CONTROLLER,
        KEYBOARD
    }

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;

        _playerInputs = GetComponent<PlayerInput>();
        InputSystem.onActionChange += InputActionChangeCallback;
    }

    private void InputActionChangeCallback(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed)
        {
            InputAction receivedInputAction = (InputAction)obj;
            InputDevice lastDevice = receivedInputAction.activeControl.device;

            _currentInputMode = lastDevice.name.Equals("Keyboard") || lastDevice.name.Equals("Mouse") ? EInputMode.KEYBOARD : EInputMode.CONTROLLER;
        }
    }
}
