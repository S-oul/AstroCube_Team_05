using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIToggleButton : MonoBehaviour
{
    [SerializeField] private TMP_Text stateText;
    [SerializeField] private bool isEnable = true;
    [SerializeField] private string saveKey;

    public UnityEvent<bool> onToggleChanged;

    public bool IsEnable => isEnable;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        
        if(stateText == null)
            stateText = GetComponentInChildren<TMP_Text>();

        if (!string.IsNullOrEmpty(saveKey) && PlayerPrefs.HasKey(saveKey))
        {
            isEnable = PlayerPrefs.GetInt(saveKey) == 1;
        }

        _button.onClick.AddListener(Toggle);
        RefreshUI();
    }

    private void Toggle()
    {
        isEnable = !isEnable;
        RefreshUI();


        PlayerPrefs.SetInt(saveKey, isEnable ? 1 : 0);

        onToggleChanged?.Invoke(isEnable);
    }

    private void RefreshUI()
    {
        if (isEnable)
        {
            stateText.text = "Enable";
        }
        else
        {
            stateText.text = "Disable";
        }
    }


    public void ApplySavedState()
    {
        RefreshUI();
        onToggleChanged?.Invoke(isEnable);
    }
}
