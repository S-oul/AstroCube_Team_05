using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFadeOut : MonoBehaviour
{
    [SerializeField] private Image imageToFade;
    [SerializeField] private float fadeDuration = 1.5f;


    private void Awake()
    {
        imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, 1f);
    }


    private void Start()
    {
        FadeIn(fadeDuration);
    }

    public void FadeIn(float duration)
    {
        StartCoroutine(Fade(1f, 0f, duration));
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, alpha);
            yield return null;
        }
        imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, endAlpha);
    }
}
