using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NoClip : MonoBehaviour
{
    [SerializeField] PlayerInput _playerInput;

    PlayerMovement _playerMovement;
    bool _noClipEnabled = false;

    private void Start()
    {
        if (_playerInput == null)
        {
            Debug.LogError("PlayerInput missing from NoClip inspector");
        }
        _playerMovement = GetComponent<PlayerMovement>();
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
                Debug.Log("Activating NoClip");
                _playerMovement.ActivateNoClip();
                //switch input system to NoClip input system
            }
            else
            {
                _playerMovement.DeactivateNoClip();
                //switch back to default input system
            }
        }
    }
}
