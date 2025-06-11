using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntitySequenceManager : MonoBehaviour
{
    [SerializeField] private TextAnimation _textAnimation;
    [SerializeField] List<GameObject> _objectToDisable;

    [SerializeField] InteractLine _interactLine;

    public UnityEvent OnEnd;
    public void StartAnim() => _ToggleObjects(false);
    public void StopAnim()
    {
        OnEnd?.Invoke();
        if(_interactLine) _interactLine.CallCoroutine();
        _ToggleObjects(true);
        gameObject.SetActive(false);
    }

    private void _ToggleObjects(bool isActive)
    {
        foreach (var obj in _objectToDisable)
        {
            if (obj)
                obj.gameObject.SetActive(isActive);
        }

    }
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
