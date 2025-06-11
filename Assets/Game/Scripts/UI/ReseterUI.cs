using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReseterUI : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private string _sliderProperty = "Slider";
    private GameSettings _gameSettings;


    private Coroutine _sliderCoroutine;


    private void Awake()
    {
        _gameSettings = GameManager.Instance.Settings;
    }

    public void OnReset()
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
    }


}
