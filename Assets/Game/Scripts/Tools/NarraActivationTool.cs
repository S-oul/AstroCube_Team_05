using MoreMountains.FeedbacksForThirdParty;
using Unity.VisualScripting;
using UnityEngine;

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

    private void Start()
    {
        if (_isNarraActiveTool == false)
        {
            Debug.Log("WARNING ! Narrative scene triggers are currently DEACTIVATED in this scene.");
        }
    }
}
