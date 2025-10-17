using MoreMountains.FeedbacksForThirdParty;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class NarraActivationTool : MonoBehaviour
{
    [SerializeField] bool _isNarraActiveTool = true;

    public bool IsNarraActiveTool
    {
#if UNITY_EDITOR
        get => _isNarraActiveTool;
#else
        get => true;
#endif
    }

    public static event Action NarraIsDisabledEvent;

    private void Start()
    {
        if (IsNarraActiveTool == false)
        {
            Debug.Log("WARNING ! Narrative scene triggers are currently DEACTIVATED in this scene.");

            NarraIsDisabledEvent?.Invoke();
        }
    }
}
