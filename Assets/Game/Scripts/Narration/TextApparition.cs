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
    [field: SerializeField] public float StayDuration { get; private set; }

    [SerializeField] private UnityEvent OnEndTextApparition;

    [field: Header("Disparition")]
    [field: SerializeField] public float CharFadeOutDelay { get; private set; }
    [field: SerializeField] public float CharFadeOutDuration { get; private set; }

    [field: Header("Effects")]
    [BoxGroup("Shaking"), SerializeField] private bool ShakeText;
    [field: BoxGroup("Shaking"), ShowIf("ShakeText"), SerializeField] public float CharShakeStrength { get; private set; }
    [field: BoxGroup("Shaking"), ShowIf("ShakeText"), SerializeField] public int CharShakeFrequency { get; private set; }
    [field: BoxGroup("Shaking"), ShowIf("ShakeText"), SerializeField] public float CharShakeDuration { get; private set; }
    [field: BoxGroup("Shaking"), ShowIf("ShakeText"), SerializeField] public float CharShakeRandomness { get; private set; }

    private DOTweenTMPAnimator animator;
    private Coroutine displayCo;

    private void Awake()
    {
        if (TryGetComponent<TMP_Text>(out _textMesh)) _textMesh.color = new Color(_textMesh.color.r, _textMesh.color.g, _textMesh.color.b, 0);
        else Debug.LogWarning("TextApparition sans texte associé sur : " + name);
        animator = new DOTweenTMPAnimator(_textMesh);
    }

    
    public void Display()
    {
        displayCo = StartCoroutine(TextDisplay());
    }

    IEnumerator TextDisplay()
    {
        yield return new WaitForSeconds(ApparitionDelay);

        for (int i = 0; i < animator.textInfo.characterCount; i++)
        {
            animator.DOFadeChar(i, 1, CharFadeInDuration);
            if(ShakeText) animator.DOShakeCharOffset(i, CharShakeDuration, CharShakeStrength, CharShakeFrequency, CharShakeRandomness, true);
            yield return new WaitForSeconds(CharFadeInDelay);
        }

        yield return new WaitForSeconds(StayDuration);
        displayCo = null;
        Hide();
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
