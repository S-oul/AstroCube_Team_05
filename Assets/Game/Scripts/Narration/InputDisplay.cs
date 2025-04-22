using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputDisplay : MonoBehaviour
{
    [SerializeField] float _apparitionDuration;
    [SerializeField] float _disparitionDuration;
    [SerializeField] TMP_Text _text;
    Color _invisible = new Color(1,1,1,0);
    public Action OnDoInput;

    void Awake()
    {
        if(_text)
            _text.color = _invisible;
        OnDoInput += End;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!_text) return;
        _text.DOColor(Color.white, _apparitionDuration);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!_text) return;
        _text.DOColor(_invisible, _disparitionDuration);
    }

    private void End()
    {
        StartCoroutine(StartEnd());
    }

    private IEnumerator StartEnd()
    {
        yield return _text.DOColor(_invisible, _disparitionDuration).WaitForCompletion();
        Destroy(gameObject);  
    }

}
