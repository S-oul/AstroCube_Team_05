using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashOverlay : MonoBehaviour
{
    public static FlashOverlay Instance { get; private set; }

    [SerializeField] private Image _image;

    public bool IsPlaying { get; private set; } = false;

    private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    private void Awake()
    {
        Instance = this;
    }

    public void Play(float duration) { Play(duration, 1f); }
    public void Play(float duration, float targetAlpha)
    {
        StartCoroutine(_CoroutinePlay(duration, targetAlpha));
    }

    private IEnumerator _CoroutinePlay(float duration, float targetAlpha)
    {
        IsPlaying = true;

        float halfDuration = duration / 2f;

        Color baseColor = _image.color;
        Color targetColor = baseColor;
        targetColor.a = targetAlpha;

        _image.DOColor(targetColor, halfDuration);
        yield return new WaitForSeconds(halfDuration);

        _image.DOColor(baseColor, halfDuration);
        yield return new WaitForSeconds(halfDuration);

        IsPlaying = false;
    }
}