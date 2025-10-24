using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private List<Image> imagesToAppearInOrder = new List<Image>();

    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float holdDuration = 1.5f;

    [SerializeField] private string nextSceneName = "GameEntry";

    private void Start()
    {
        foreach (var img in imagesToAppearInOrder)
        {
            if (img != null)
            {
                var c = img.color;
                c.a = 0f;
                img.color = c;
            }
        }

        StartCoroutine(PlayIntroSequence());
    }

    private IEnumerator PlayIntroSequence()
    {
        foreach (var img in imagesToAppearInOrder)
        {
            if (img == null) continue;

            yield return StartCoroutine(Fade(img, 0f, 1f, fadeDuration));

            yield return new WaitForSeconds(holdDuration);

            yield return StartCoroutine(Fade(img, 1f, 0f, fadeDuration));
        }

        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator Fade(Image img, float from, float to, float duration)
    {
        float elapsed = 0f;
        Color c = img.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            c.a = Mathf.Lerp(from, to, t);
            img.color = c;
            yield return null;
        }

        c.a = to;
        img.color = c;
    }
}
