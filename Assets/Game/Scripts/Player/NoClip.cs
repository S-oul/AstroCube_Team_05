using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NoClip : MonoBehaviour
{
    PlayerInput _playerInput;

    PlayerMovement _playerMovement;
    bool _noClipEnabled = false;

    //InputActionMap _playerMovementMap;
    InputActionMap _noClipMap;

    List<string> _enabledActionMaps = new List<string>();

    private void Start()
    {
        _playerInput = InputSystemManager.Instance.PlayerInputs;
        if (_playerInput == null)
        {
            Debug.LogError("PlayerInput missing from NoClip inspector.");
        }
        _playerMovement = GetComponent<PlayerMovement>();

        //_playerMovementMap = _playerInput.actions.FindActionMap("PlayerMovement");
        //if (_playerMovementMap == null) Debug.LogError("Could not find InputActionMap 'PlayerMovement'.");
        _noClipMap = _playerInput.actions.FindActionMap("NoClip");
        if (_noClipMap == null) Debug.LogError("Could not find InputActionMap 'NoClip'.");

        foreach (InputActionMap map in _playerInput.actions.actionMaps)
        {
            if (map.enabled)
            {
                _enabledActionMaps.Add(map.name);
            }
        }
    }

    void Update()
    {
        if (_playerInput == null)
        {
            Debug.LogError("PlayerInput missing from NoClip inspector");
            return;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            _noClipEnabled = !_noClipEnabled;

            if (_noClipEnabled)
            {
                _playerMovement.ActivateNoClip();
                foreach (InputActionMap map in _playerInput.actions.actionMaps)
                {
                    map.Disable();
                }
                _noClipMap.Enable();
            }
            else
            {
                _playerMovement.DeactivateNoClip();
                foreach (InputActionMap map in _playerInput.actions.actionMaps)
                {
                    if (_enabledActionMaps.Contains(map.name))
                    {
                        map.Enable();
                    }
                }
                _noClipMap.Disable();
            }
        }
    }
}
