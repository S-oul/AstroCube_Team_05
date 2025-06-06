using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextApparition : MonoBehaviour
{
    // À mettre sur le même objet que le script
    private TMP_Text _textMesh;

    [field: Header("Apparition")]
    [field: SerializeField] public float ApparitionDelay { get; private set; }
    [field: SerializeField] public float CharFadeInDelay { get; private set; }
    [field: SerializeField] public float CharFadeInDuration { get; private set; }
    [BoxGroup("Stay"), SerializeField] private bool Permanent;
    [field: BoxGroup("Stay"), HideIf("Permanent"), SerializeField] public float StayDuration { get; private set; }

    [field: Header("Disparition")]
    [field: SerializeField] public float CharFadeOutDelay { get; private set; }
    [field: SerializeField] public float CharFadeOutDuration { get; private set; }

    [field: Header("Effects")]
    [field: SerializeField] public bool AutoTrigger { get; private set; }
    [BoxGroup("Shaking"), SerializeField] private bool ShakeText;
    [field: BoxGroup("Shaking"), ShowIf("ShakeText"), SerializeField] public float CharShakeStrength { get; private set; }
    [field: BoxGroup("Shaking"), ShowIf("ShakeText"), SerializeField] public int CharShakeFrequency { get; private set; }
    [field: BoxGroup("Shaking"), ShowIf("ShakeText"), SerializeField] public float CharShakeDuration { get; private set; }
    [field: BoxGroup("Shaking"), ShowIf("ShakeText"), SerializeField] public float CharShakeRandomness { get; private set; }
    [field: BoxGroup("Shaking"), ShowIf("ShakeText"), SerializeField] public bool CharShakeFadeOut { get; private set; }

    [Header("Events")]
    [SerializeField] private UnityEvent OnStartTextApparition;
    [SerializeField] private UnityEvent OnStayTextApparition;
    [SerializeField] private UnityEvent OnEndTextApparition;

    private DOTweenTMPAnimator animator;
    private Coroutine displayCo;

    private void Awake()
    {
        if (TryGetComponent<TMP_Text>(out _textMesh)) _textMesh.color = new Color(_textMesh.color.r, _textMesh.color.g, _textMesh.color.b, 0);
        else Debug.LogWarning("TextApparition sans texte associé sur : " + name);
        animator = new DOTweenTMPAnimator(_textMesh);

        if (AutoTrigger) Display();
    }

    
    public void Display()
    {
        displayCo = StartCoroutine(TextDisplay());
    }

    IEnumerator TextDisplay()
    {
        yield return new WaitForSeconds(ApparitionDelay);

        OnStartTextApparition?.Invoke();
        for (int i = 0; i < animator.textInfo.characterCount; i++)
        {
            animator.DOFadeChar(i, 1, CharFadeInDuration);
            if(ShakeText) animator.DOShakeCharOffset(i, CharShakeDuration, CharShakeStrength, CharShakeFrequency, CharShakeRandomness, CharShakeFadeOut);
            yield return new WaitForSeconds(CharFadeInDelay);
        }

        OnStayTextApparition?.Invoke();
        if (!Permanent)
        {
            yield return new WaitForSeconds(StayDuration);
            displayCo = null;
            Hide();
        }
        else displayCo = null;
    }

    public void Hide()
    {
        if(displayCo != null)
        {
            StopCoroutine(displayCo);
            displayCo = null;
        }
        StartCoroutine(TextHide());
    }

    IEnumerator TextHide()
    {
        for (int i = 0; i < animator.textInfo.characterCount; i++)
        {
            animator.DOFadeChar(i, 0, CharFadeOutDuration);
            yield return new WaitForSeconds(CharFadeOutDelay);
        }

        OnEndTextApparition?.Invoke();
    }
}
