using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChangeUiController : MonoBehaviour
{

    [SerializeField] Sprite _Xbox;
    [SerializeField] Sprite _PS;
    [SerializeField] Sprite _Keyboard;


    Image _sprite;
    private void Awake()
    {
        InputSystem.onActionChange += InputSystem_onDeviceChange;
        _sprite = GetComponent<Image>();


    }

    void OnDisable()
    {
        InputSystem.onActionChange -= InputSystem_onDeviceChange;
    }

    private void InputSystem_onDeviceChange(object obj, InputActionChange change)
    {
        InputAction receivedInputAction;
        InputDevice lastDevice;
        if (change == InputActionChange.ActionPerformed)
        {
            receivedInputAction = (InputAction)obj;
            lastDevice = receivedInputAction.activeControl.device;

            string name = lastDevice.name;
            print(name);


            if (name.Contains("XInputControllerWindows")) _sprite.sprite = _Xbox;
            else if (name.Contains("Playstation")) _sprite.sprite = _PS;
            else _sprite.sprite = _Keyboard; ;
        }
    }

}
