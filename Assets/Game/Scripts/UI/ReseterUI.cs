using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReseterUI : MonoBehaviour
{
    public static ReseterUI Instance { get; private set; }

    [Header("UI Materials")]
    [SerializeField] private List<Material> _materials = new();  
    [SerializeField] private string _sliderProperty = "_Slider1";
    [SerializeField] private GameObject _sliderObject;

    private GameSettings _gameSettings;

    private Coroutine _sliderCoroutine;
    private float _targetDuration = 1f;
    private bool _isHolding = false;

    private void Awake()
    {
        Instance = this;
        _gameSettings = GameManager.Instance.Settings;
    }

    private void OnDisable()
    {
        if (_sliderCoroutine != null)
            StopCoroutine(_sliderCoroutine);
    }



    public void StartReset()
    {
        if (!HasPropertyOnAllMaterials())
        {
            Debug.LogError($"At least one material does not contain the property '{_sliderProperty}'");
            return;
        }

        float resetTime = _gameSettings.ResetCurve.Evaluate(GameManager.Instance.RubiksCube.Moves.Count);
        _targetDuration = resetTime;

        _isHolding = true;

        if (_sliderCoroutine != null)
            StopCoroutine(_sliderCoroutine);

        _sliderCoroutine = StartCoroutine(AnimateSliderForward(resetTime));
    }

    public void CancelReset()
    {
        _isHolding = false;

        if (_sliderCoroutine != null)
            StopCoroutine(_sliderCoroutine);

        _sliderCoroutine = StartCoroutine(AnimateSliderBackward(0.25f));
    }

    public void ForceConfirmReset()
    {
        ConfirmReset();
    }



    private void ConfirmReset()
    {
        Debug.Log("Reset confirmé !");
        EventManager.Instance.TriggerReset();
    }



    private IEnumerator AnimateSliderForward(float duration)
    {
        _sliderObject.SetActive(true);

        float elapsed = 0f;

        while (elapsed < duration && _isHolding)
        {
            float t = elapsed / duration;
            SetFloatOnAllMaterials(t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (!_isHolding)
            yield break;

        SetFloatOnAllMaterials(1f);

        ConfirmReset();

        _sliderObject.SetActive(false);
        _sliderCoroutine = null;
    }

    private IEnumerator AnimateSliderBackward(float duration)
    {
        float startValue = GetFirstMaterialValue();
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float v = Mathf.Lerp(startValue, 0f, t);
            SetFloatOnAllMaterials(v);

            elapsed += Time.deltaTime;
            yield return null;
        }

        SetFloatOnAllMaterials(0f);
        _sliderObject.SetActive(false);

        _sliderCoroutine = null;
    }


    private void SetFloatOnAllMaterials(float value)
    {
        for (int i = 0; i < _materials.Count; i++)
        {
            if (_materials[i] != null)
                _materials[i].SetFloat(_sliderProperty, value);
        }
    }

    private float GetFirstMaterialValue()
    {
        if (_materials.Count == 0 || _materials[0] == null)
            return 0;

        return _materials[0].GetFloat(_sliderProperty);
    }

    private bool HasPropertyOnAllMaterials()
    {
        foreach (var mat in _materials)
        {
            if (mat == null) continue;
            if (!mat.HasProperty(_sliderProperty))
                return false;
        }
        return true;
    }
}
