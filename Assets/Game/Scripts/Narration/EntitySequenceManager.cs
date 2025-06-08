using UnityEngine;

public class EntitySequenceManager : MonoBehaviour
{
    [SerializeField] private TextAnimation _textAnimation;

    public void DisplayText()
    {
        _textAnimation.DisplayText();
    }

    public void DistortScreen(float duration)
    {
        PostProcessManager.Instance.SetScreenDistortion(1.0f, duration);
    }

    public void SetDistortion(float amount)
    {
        PostProcessManager.Instance.SetScreenDistortion(amount);
    }

    private void OnDisable()
    {
        PostProcessManager.Instance.SetScreenDistortion(0.0f);
    }
}
