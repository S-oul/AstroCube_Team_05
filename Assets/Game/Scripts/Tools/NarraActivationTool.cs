using MoreMountains.FeedbacksForThirdParty;
using UnityEngine;

public class NarraActivationTool : MonoBehaviour
{
    [SerializeField] bool _isNarraActiveTool = true;


    public bool IsNarraActiveTool
    {
        get => _isNarraActiveTool;
    }
}
