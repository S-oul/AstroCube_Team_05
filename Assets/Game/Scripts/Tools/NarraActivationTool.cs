using MoreMountains.FeedbacksForThirdParty;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class NarraActivationTool : MonoBehaviour
{
    [SerializeField] bool _isNarraActiveTool = true;
    [SerializeField] GameObject _screenMessage;

    public bool IsNarraActiveTool
    {
        get => _isNarraActiveTool;
    }

    public static event Action NarraIsDisabledEvent;
    bool _narraIsDisabledEventCalled = false;

    private void Start()
    {
        if (IsNarraActiveTool == false)
        {
            Debug.Log("WARNING ! Narrative scene triggers are currently DEACTIVATED in this scene.");

            NarraIsDisabledEvent?.Invoke();
            _narraIsDisabledEventCalled = true;
        }

        if (_screenMessage != null)
        {
            _screenMessage.SetActive(!IsNarraActiveTool);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            _isNarraActiveTool = !_isNarraActiveTool;
            Debug.Log("Narra activation set to : " + _isNarraActiveTool);

            if (_narraIsDisabledEventCalled == false && _isNarraActiveTool == false)
            {
                NarraIsDisabledEvent?.Invoke();
                _narraIsDisabledEventCalled = true;
            }

            if (_screenMessage != null)
            {
                _screenMessage.SetActive(!_isNarraActiveTool);
            }
        }
    }
}
