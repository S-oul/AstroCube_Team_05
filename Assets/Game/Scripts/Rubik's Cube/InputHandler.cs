using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystemManager;

public class InputHandler : MonoBehaviour
{
    [SerializeField] PlayerHold _playerHold;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] MouseCamControl _mouseCam;
    [SerializeField] private CameraFocusAttractor _cameraFocusAttractor;

    RubiksCubeController _controller;
    PlayerInput _playerInput;

    public static InputHandler Instance => instance;
    private static InputHandler instance;

    public Vector2 CameraMovement => _cameraMovement;
    private Vector2 _cameraMovement;

    private bool _oneHandActAsNormal = true;
    
    public bool CanMove
    {
        get => _canMove;
        set
        {
            _canMove = value;
            ToggleCanMove(value);
        }
    }
    [SerializeField, ReadOnly] private bool _canMove;

    private static HashSet<EInputType> _disabledInputs = new HashSet<EInputType>();
    private static bool _globalInputEnabled = true;

    public static void DisableAllInputs()
    {
        _globalInputEnabled = false;
    }

    public static void EnableAllInputs()
    {
        _globalInputEnabled = true;
        _disabledInputs.Clear();
    }

    public static void DisableInputs(List<EInputType> types)
    {
        _globalInputEnabled = true;
        foreach (var type in types)
            _disabledInputs.Add(type);
    }

    public static void EnableInputs(List<EInputType> types)
    {
        foreach (var type in types)
            _disabledInputs.Remove(type);
    }

    public static bool IsInputEnabled(EInputType type)
    {
        return _globalInputEnabled && !_disabledInputs.Contains(type);
    }

    void Start()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }
        else instance = this;

        _canMove = true;
        _controller = GetComponent<RubiksCubeController>();
        _playerInput = InputSystemManager.Instance.PlayerInputs;

        InputActionMap _actionMap = _playerInput.actions.FindActionMap("PlayerMovement");
        if (_actionMap != null)
        {
            if (_playerMovement != null)
            {
                _actionMap.Enable();
            }
            else Debug.LogWarning("playerMovement script is missing from InputHandler Inspector");
        }
        else Debug.LogWarning("PlayerMovement InputMap not found.");

        _playerInput.actions.FindActionMap("OtherActions").Enable();

        if (!GameManager.Instance.IsRubiksCubeEnabled)
        {
            _playerInput.actions.FindActionMap("RubiksCube").Disable();
        }
    }

    private void ToggleCanMove(bool canMove)
    {
        if (_playerInput == null) return;

        switch (canMove)
        {
            case true:
                _playerInput.actions.FindActionMap("PlayerMovement")?.Enable();
                break;
            case false:
                _playerInput.actions.FindActionMap("PlayerMovement")?.Disable();
                break;
        }
    }

    #region Rubiks Cube Inputs
    public void OnSwitchColumnsLineLeft(InputAction.CallbackContext ctx)
    {
        if (!IsInputEnabled(EInputType.SWITCH_COLUMNS_LINE_LEFT)) return;
        if (ctx.performed)
            _controller.ActionSwitchLineCols(true);
    }

    public void OnSwitchColumnsLineRight(InputAction.CallbackContext ctx)
    {
        if (!IsInputEnabled(EInputType.SWITCH_COLUMNS_LINE_RIGHT)) return;
        if (ctx.performed)
            _controller.ActionSwitchLineCols(false);
    }

    public void OnClockWise(InputAction.CallbackContext ctx)
    {
        if (!IsInputEnabled(EInputType.CLOCKWISE)) return;
        if (ctx.performed)
            _controller.ActionMakeTurn(false);
    }

    public void OnCounterClockWise(InputAction.CallbackContext ctx)
    {
        if (!IsInputEnabled(EInputType.COUNTER_CLOCKWISE)) return;
        if (ctx.performed)
            _controller.ActionMakeTurn(true);
    }    

    public void OnMoveOverlayCube(InputAction.CallbackContext ctx)
    {
        return; // don't do it anymore
        if (!IsInputEnabled(EInputType.MOVE_OVERLAY_CUBE)) return;
        if (ctx.performed)
            _controller.ActionRotateCubeUI(ctx.ReadValue<Vector2>());
    }

    public void OnResetRoom(InputAction.CallbackContext ctx)
    {
        if (!IsInputEnabled(EInputType.RESET_ROOM)) return;

        if (ctx.performed)
        {
            if (!_controller.ControlledScript.IsReversing)
                EventManager.Instance.TriggerReset();
        }
        else if (ctx.canceled && ctx.time - ctx.startTime < 0.5f)
        {
            if (!_controller.ControlledScript.IsReversing && _controller.ControlledScript.Moves.Count > 0)
                EventManager.Instance.TriggerResetOnce();
        }
    }
    public void OnPreviewCancel(InputAction.CallbackContext ctx)
    {
        if (!IsInputEnabled(EInputType.PREVIEW_CANCEL)) return;
        if (ctx.performed)
            EventManager.TriggerPreviewCancel();
    }

    public void OnSwitchLookMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        _oneHandActAsNormal = !_oneHandActAsNormal;
    }
    #endregion

    #region Other Actions
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!IsInputEnabled(EInputType.INTERACT)) return;
        if (ctx.performed)
            _playerHold.TryHold();
    }

    public void OnGamePause(InputAction.CallbackContext ctx)
    {
        if (!IsInputEnabled(EInputType.GAME_PAUSE)) return;
        if (ctx.performed)
        {
            if (!EventManager.gamePaused) EventManager.TriggerGamePause();
            else EventManager.TriggerGameUnpause();
        }
    }

    public void OnSeeExit(InputAction.CallbackContext ctx)
    {
        if (!IsInputEnabled(EInputType.SEE_EXIT)) return;
        if (ctx.performed)
            EventManager.TriggerSeeExit();
    }
    #endregion

    #region Player Movement & NoClip Movement
    public void OnMovement(InputAction.CallbackContext ctx)
    {
        if (!_oneHandActAsNormal)
        {
            OnFakeCamera(ctx);
            return;
        }
        if (!IsInputEnabled(EInputType.MOVEMENT)) return;
        if (!_controller.ControlledScript.IsReversing)
            _playerMovement.ActionMovement(ctx.ReadValue<Vector2>());
    }

    void onFakeMovement(InputAction.CallbackContext ctx)
    {
        if (!IsInputEnabled(EInputType.MOVEMENT)) return;
        if (!_controller.ControlledScript.IsReversing)
            _playerMovement.ActionMovement(ctx.ReadValue<Vector2>());
    }
    public void OnCamera(InputAction.CallbackContext ctx)
    {
        if (!_oneHandActAsNormal)
        {
            onFakeMovement(ctx);
            return;
        }

        if (!IsInputEnabled(EInputType.CAMERA)) return;

        if (_controller.ControlledScript == null || !_controller.ControlledScript.IsReversing)
            _mouseCam.OnCamera(ctx);
    }


    void OnFakeCamera(InputAction.CallbackContext ctx)
    {
        if (!IsInputEnabled(EInputType.CAMERA)) return;
        if (!_controller.ControlledScript.IsReversing)
            _mouseCam.OnCamera(ctx);
    }



    //Unused
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!IsInputEnabled(EInputType.MOVEMENT)) return;
        if (!ctx.performed && !_controller.ControlledScript.IsReversing)
            _playerMovement.ActionJump();
    }

    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (!IsInputEnabled(EInputType.MOVEMENT)) return;
        if (!ctx.performed && !_controller.ControlledScript.IsReversing)
            _playerMovement.ActionCrouch();
    }

    #endregion

    #region Noclip
    public void OnVerticalMovement(InputAction.CallbackContext ctx)
    {
        if (!IsInputEnabled(EInputType.MOVEMENT)) return;
        if (!ctx.performed)
            _playerMovement.ActionVerticalMovement(ctx.ReadValue<float>());
    }
    #endregion

    #region UI
    public void OnShowStripLayer(InputAction.CallbackContext ctx)
    {
        if (!IsInputEnabled(EInputType.SHOW_STRIPS)) return;
        if (ctx.performed)
            _controller.ShowStripLayerToPlayer = !_controller.ShowStripLayerToPlayer;
    }
    #endregion

    public void BlockMovement()
    {
        DisableInputs(new List<EInputType>
        {
            EInputType.MOVEMENT,
        });
    }

    public void DeBlockMovement()
    {
        EnableInputs(new List<EInputType>
        {
            EInputType.MOVEMENT,
        });
    }
}
