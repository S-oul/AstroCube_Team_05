using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public abstract class UIView : MonoBehaviour
{
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] private float viewFadeDuration = 0.8f;

    [Header("(OPTIONAL)")]
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Selectable firstSelected;

    public Transform CameraPosition => cameraPosition;

    private Coroutine fadeCoroutine;

    private InputAction navigateAction;


    protected virtual void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    protected virtual void OnEnable()
    {
        var uiModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
        navigateAction = uiModule.move; 

        navigateAction.performed += OnNavigatePerformed;
    }

    protected virtual void OnDisable()
    {
        if (navigateAction != null)
            navigateAction.performed -= OnNavigatePerformed;
    }


    private void OnNavigatePerformed(InputAction.CallbackContext ctx)
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (EventSystem.current.currentSelectedGameObject != null)
            return;

        if (firstSelected != null)
            EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
    }


    public virtual void Show()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        gameObject.SetActive(true);

        fadeCoroutine = StartCoroutine(FadeCanvas(0f, 1f, onComplete: () =>
        {
            if (firstSelected != null)
                StartCoroutine(SelectAfterFrame());
        }));
    }

    public virtual void ShowImmediate()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        gameObject.SetActive(true);

        if (firstSelected != null && EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
    }

    public virtual void Hide()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeCanvas(1f, 0f));
    }

    public void HideImmediate()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
    }

    private IEnumerator FadeCanvas(float from, float to, System.Action onComplete = null)
    {
        if (viewFadeDuration <= 0f)
        {
            canvasGroup.alpha = to;
            canvasGroup.interactable = to > 0.99f;
            canvasGroup.blocksRaycasts = to > 0.99f;
            if (to < 0.01f)
                gameObject.SetActive(false);
            onComplete?.Invoke();
            yield break;
        }

        float elapsed = 0f;
        canvasGroup.alpha = from;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (elapsed < viewFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / viewFadeDuration);
            canvasGroup.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        canvasGroup.alpha = to;

        bool visible = to > 0.99f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;

        if (!visible)
            gameObject.SetActive(false);

        onComplete?.Invoke();
    }

    private IEnumerator SelectAfterFrame()
    {
        yield return null;
        if (EventSystem.current != null && firstSelected != null)
            EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
    }
}
