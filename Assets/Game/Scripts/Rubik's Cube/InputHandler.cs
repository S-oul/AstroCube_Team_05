using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class InputHandler : MonoBehaviour
{
    [SerializeField] PlayerHold _playerHold;
    RubiksCubeController _controller;

    void Awake()
    {
        _controller = GetComponent<RubiksCubeController>();
    }

    public void OnSwitchColumnsLine(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            _controller.ActionSwitchLineCols();
        }
    }
    public void OnClockWise(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            _controller.ActionMakeTurn(false);
        }
    }
    public void OnCounterClockWise(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            _controller.ActionMakeTurn(true);
        }
    }

    public void OnMoveOverlayCube(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            _controller.ActionRotateCube(callbackContext.ReadValue<Vector2>());
        }
    }

    public void OnResetRoom(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            EventManager.Instance.TriggerReset();
        }
    }
    public void OnInteract(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if (_playerHold.IsHolding) _playerHold.TryRelease();
            else _playerHold.TryHold();
        }
    }


    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), "ZQSD : A & E to turn Cube, F to swap Face");
        GUI.Label(new Rect(10, 30, 300, 20), "'Trigger or L2/R2' to turn Cube, 'Bumper otr R1/L1' to swap Face");
        GUI.Label(new Rect(10, 50, 300, 20), "'V' or 'WestButton' to return Room to normal");


    }

}
