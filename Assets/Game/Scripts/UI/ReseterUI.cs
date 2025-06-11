using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReseterUI : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private string _sliderProperty = "_Slider1";
    [SerializeField] private GameObject _sliderObject;
    private GameSettings _gameSettings;


    private Coroutine _sliderCoroutine;


    private void OnEnable()
    {
        EventManager.OnPlayerReset += OnReset;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerReset -= OnReset;
    }


    private void Awake()
    {
        _gameSettings = GameManager.Instance.Settings;
    }

    public void OnReset(float caca)
    {
        

        if (!_material.HasProperty(_sliderProperty))
        {
            Debug.LogError($"Material doesnt contain the property : '{_sliderProperty}'. Check the shader.");
            return;
        }
        float resetTime = _gameSettings.ResetCurve.Evaluate(GameManager.Instance.RubiksCube.Moves.Count);

        if (_sliderCoroutine != null)
            StopCoroutine(_sliderCoroutine);

        _sliderCoroutine = StartCoroutine(AnimateSlider(resetTime));
    }

    private IEnumerator AnimateSlider(float duration)
    {
        _sliderObject.SetActive(true);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            _material.SetFloat(_sliderProperty, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _material.SetFloat(_sliderProperty, 1f);
        _sliderCoroutine = null;

        _material.SetFloat(_sliderProperty, 1f);
        _sliderObject.SetActive(false);
    }


}
