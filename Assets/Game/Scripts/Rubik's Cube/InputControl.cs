using UnityEngine;
using UnityEngine.InputSystem;

public class InputControl : MonoBehaviour
{
    RubiksCubeController RbkcController;
    public PlayerAction Inputs;
    private void Awake()
    {
        RbkcController = GetComponent<RubiksCubeController>();
        if( RbkcController == null ) { Debug.LogError("No Rubiks Controller"); }
        Inputs = new PlayerAction();
        Inputs.NicoScheme.Enable();

        Inputs.NicoScheme.Select.performed += OnSelect;
        Inputs.NicoScheme.DeSelect.performed += OnDeSelect;
        Inputs.NicoScheme.ShowCube.performed += OnShowCube;

        Inputs.NicoScheme.Dpad.performed += OnDpad;

        Inputs.NicoScheme.ClockWise.performed += OnClockWise;
        Inputs.NicoScheme.CounterClockWise.performed += OnCounterClockWise;



    }
    private void OnShowCube(InputAction.CallbackContext context)
    {
        RbkcController.ActionShowUpcube();
    }
    private void OnSelect(InputAction.CallbackContext context)
    {
        RbkcController.ActionValidate();
    }
    private void OnDeSelect(InputAction.CallbackContext context)
    {
        RbkcController.ActionDeValidate();
    }
    private void OnDpad(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        print(dir);
        switch (dir.x, dir.y)
        {
            case (0, -1):
                RbkcController.ActionUp();
                return;
            case (1, 0):
                RbkcController.ActionRight();
                return;
            case (0, 1):
                RbkcController.ActionDown();
                return;
            case (-1, 0):
                RbkcController.ActionLeft();
                return;
        }
    }

    private void OnClockWise(InputAction.CallbackContext context)
    {
        RbkcController.ActionMakeTurn(true);
    }
    private void OnCounterClockWise(InputAction.CallbackContext context)
    {
        RbkcController.ActionMakeTurn(false);
    }


}
