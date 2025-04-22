using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextApparition : MonoBehaviour
{
    [field: Header("Components")]
    [field: SerializeField] public TMP_Text TextMesh { get; private set; }

    [field: Header("Apparition")]
    [field: SerializeField] public float ApparitionDelay { get; private set; }
    [field: SerializeField] public float CharFadeInDelay { get; private set; }
    [field: SerializeField] public float CharFadeInDuration { get; private set; }
    [field: SerializeField] public float StayDuration { get; private set; }

    [field: Header("Disparition")]
    [field: SerializeField] public float CharFadeOutDelay { get; private set; }
    [field: SerializeField] public float CharFadeOutDuration { get; private set; }

    private void Awake()
    {
        if (TextMesh != null)
        {
            TextMesh.color = new Color(TextMesh.color.r, TextMesh.color.g, TextMesh.color.b, 0);
        }
    }

    
    public void Display()
    {
        StartCoroutine(TextDisplay());
    }

    IEnumerator TextDisplay()
    {
        yield return new WaitForSeconds(ApparitionDelay);

        DOTweenTMPAnimator animator = new DOTweenTMPAnimator(TextMesh);
        for (int i = 0; i < animator.textInfo.characterCount; i++)
        {
            animator.DOFadeChar(i, 1, CharFadeInDuration);
            yield return new WaitForSeconds(CharFadeInDelay);
        }

        yield return new WaitForSeconds(StayDuration);

        for (int i = 0; i < animator.textInfo.characterCount; i++)
        {
            animator.DOFadeChar(i, 0, CharFadeOutDuration);
            yield return new WaitForSeconds(CharFadeOutDelay);
        }
    }
}
