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
    public void OnResetRoom()
    {
        EventManager.TriggerReset();
    }


    private void OnGUI()
    {
        GUI.Label(new Rect(10,10,300,20), "ZQSD : A & E to turn Cube, F to swap Face");
        GUI.Label(new Rect(10, 30, 300, 20), "'Trigger or L2/R2' to turn Cube, 'Bumper otr R1/L1' to swap Face");
        GUI.Label(new Rect(10, 50, 300, 20), "'V' or 'WestButton' to return Room to normal");


    }

}
