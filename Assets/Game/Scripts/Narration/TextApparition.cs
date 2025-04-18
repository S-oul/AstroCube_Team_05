using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextApparition : MonoBehaviour
{
    [SerializeField] float _apparitionDuration;
    [SerializeField] float _stayDuration;
    [SerializeField] float _disparitionDuration;
    [SerializeField] TMP_Text _text;
    Color _invisible = new Color(1,1,1,0);

    void Awake()
    {
        if(_text)
            _text.color = _invisible;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_text)
            return;
        StartCoroutine(TextDisplay());
    }

    IEnumerator TextDisplay()
    {
        yield return _text.DOColor(Color.white, _apparitionDuration).WaitForCompletion();
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(_stayDuration);
        _text.DOColor(_invisible, _disparitionDuration);
    }
}
