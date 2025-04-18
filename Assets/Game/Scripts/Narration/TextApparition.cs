using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextApparition : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private List<TMP_Text> _text;

    [Header("Apparition")]
    [SerializeField] private float _apparitionDelay;
    [SerializeField] private float _charFadeInDelay;
    [SerializeField] private float _charFadeInDuration;
    [SerializeField] private float _stayDuration;

    [Header("Disparition")]
    [SerializeField] float _charFadeOutDelay;
    [SerializeField] float _charFadeOutDuration;

    void Awake()
    {
        if (_text != null)
        {
            foreach (TMP_Text text in _text)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_text == null) return;
        StartCoroutine(TextDisplay());
    }

    IEnumerator TextDisplay()
    {
        GetComponent<Collider>().enabled = false;
        foreach (TMP_Text textMesh in _text)
        {
            yield return new WaitForSeconds(_apparitionDelay);

            DOTweenTMPAnimator animator = new DOTweenTMPAnimator(textMesh);
            for (int i = 0; i < animator.textInfo.characterCount; i++)
            {
                animator.DOFadeChar(i, 1, _charFadeInDuration);
                yield return new WaitForSeconds(_charFadeInDelay);
            }

            yield return new WaitForSeconds(_stayDuration);

            for (int i = 0; i < animator.textInfo.characterCount; i++)
            {
                animator.DOFadeChar(i, 0, _charFadeOutDuration);
                yield return new WaitForSeconds(_charFadeOutDelay);
            }
        }
    }
}
