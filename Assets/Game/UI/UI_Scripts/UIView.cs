using System.Collections;
using UnityEngine;


public abstract class UIView : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float viewFadeDuration = 0.8f;

    [Header("(OPTIONAL)")]
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Transform cameraLookAt;



    public Transform CameraPosition => cameraPosition;
    public Transform CameraLookAt => cameraLookAt;

    private Coroutine fadeCoroutine;


    protected virtual void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }


    public virtual void Show()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        gameObject.SetActive(true);

        fadeCoroutine = StartCoroutine(FadeCanvas(0f, 1f));
    }

    public virtual void Hide()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeCanvas(1f, 0f, () => gameObject.SetActive(false)));
    }

    public void HideImmediate()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
    }

    private IEnumerator FadeCanvas(float from, float to, System.Action onComplete = null)
    {
        float elapsed = 0f;
        canvasGroup.alpha = from;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (elapsed < viewFadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / viewFadeDuration);
            canvasGroup.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        canvasGroup.alpha = to;
        if (to > 0.99f)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        onComplete?.Invoke();
    }
}

