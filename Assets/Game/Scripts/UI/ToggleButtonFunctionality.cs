using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ToggleButtonFunctionality : MonoBehaviour
{
    bool _buttonState = true;
    [SerializeField] TMP_Text _stateText;

    string _activeStateText = "<u><b>ON</b></u> / off";     // underline on
    string _nonActiveStateText = "on / <u><b>OFF</b></u>";  // underline off

    // Start is called before the first frame update
    private void OnEnable()
    {
        SetButtonState(_buttonState);
    }

    public void SetButtonState(bool state)
    {
        //if (_buttonState == state) return;

        _buttonState = state;
        _stateText.SetText(state ? _activeStateText : _nonActiveStateText);
    }

    void ToggleButtonState()
    {
        SetButtonState(!_buttonState);
    }

    public void OnButtonClick()
    {
        ToggleButtonState();
    }
}
