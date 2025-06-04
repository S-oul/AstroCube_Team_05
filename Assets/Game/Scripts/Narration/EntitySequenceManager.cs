using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static UnityEngine.Timeline.TimelineAsset;

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
