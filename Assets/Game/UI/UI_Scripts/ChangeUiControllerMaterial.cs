using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;

public class ChangeUiControllerMaterial : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material _xboxMaterial;
    [SerializeField] private Material _playstationMaterial;
    [SerializeField] private Material _keyboardMaterial;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();

        InputSystem.onEvent += OnInputEvent;   // <--- LA BONNE MÉTHODE
    }

    private void OnDisable()
    {
        InputSystem.onEvent -= OnInputEvent;
    }

    private void OnInputEvent(UnityEngine.InputSystem.LowLevel.InputEventPtr eventPtr, InputDevice device)
    {
        if (device == null)
            return;

        string name = device.name;

        // --- Xbox ---
        if (device is XInputController || name.Contains("Xbox"))
        {
            _image.material = _xboxMaterial;
            return;
        }

        // --- PlayStation ---
        if (device is DualShockGamepad || device is DualSenseGamepadHID || name.Contains("PS") || name.Contains("DualSense"))
        {
            _image.material = _playstationMaterial;
            return;
        }

        // --- Keyboard / Mouse ---
        if (device is Keyboard || device is Mouse)
        {
            _image.material = _keyboardMaterial;
            return;
        }
    }
}
