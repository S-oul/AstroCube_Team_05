using UnityEngine;
using UnityEngine.InputSystem.XR;

public class InputHandler : MonoBehaviour
{
    RubiksCubeController _controller;
    void Awake()
    {
        _controller = GetComponent<RubiksCubeController>();
    }

    public void OnSwitchColumnsLine()
    {
        _controller.SwitchLineCols();
    }
    public void OnClockWise()
    {
        _controller.ActionMakeTurn(false);
    }
    public void OnCounterClockWise()
    {
        _controller.ActionMakeTurn(true);

    }
}
