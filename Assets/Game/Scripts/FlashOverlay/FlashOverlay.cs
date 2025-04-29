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

    public void Play(float duration)
    {
        StartCoroutine(_CoroutinePlay(duration));
    }

    private IEnumerator _CoroutinePlay(float duration)
    {
        IsPlaying = true;

        float timer = 0f;

        Color imageColor = _image.color;

        float halfDuration = duration / 2f;
        while (timer < duration) {
            timer += Time.deltaTime;
            float alpha = Mathf.PingPong(timer, halfDuration) / halfDuration;
            imageColor.a = alpha;
            _image.color = imageColor;
            yield return _waitForEndOfFrame;
        }

        imageColor.a = 0f;
        _image.color = imageColor;

        IsPlaying = false;
    }
}