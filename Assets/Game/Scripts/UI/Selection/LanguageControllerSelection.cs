using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LanguageControllerSelection : MonoBehaviour
{
    [SerializeField] private Button _languageButton;
    [SerializeField] private InputAction _selectAction;
    
    private void OnEnable()
    {
        _selectAction.Enable();
        _selectAction.performed += OnSelectPerformed;
    }
    
    private void OnDisable()
    {
        _selectAction.Disable();
        _selectAction.performed -= OnSelectPerformed;
    }

    private void OnSelectPerformed(InputAction.CallbackContext obj)
    {
        if (EventSystem.current.currentSelectedGameObject != _languageButton.gameObject)
            return;
        
        if (obj.ReadValue<float>() <= -0.5f)
        {
            LocalizationManager.Instance.SwitchLanguage(-1);
        } else if (obj.ReadValue<float>() >= 0.5f)
        {
            LocalizationManager.Instance.SwitchLanguage(1);
        }
        
    }
}
