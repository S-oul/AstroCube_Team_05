using UnityEngine;

public class MysticEntityOverlay : MonoBehaviour
{
    [Header("Wiggle Effect")]
    [SerializeField] private Transform _wiggleEffectRoot;
    [SerializeField] private float _wiggleEffectScaleFactorMin = 1.1f;
    [SerializeField] private float _wiggleEffectScaleFactorMax = 0.9f;
    [SerializeField] private float _wiggleEffectPeriod = 1.0f;
    private float _wiggleEffectTimer = 0f;

    private void Update()
    {
        _UpdateWiggleEffect();
    }

    private void _UpdateWiggleEffect()
    {
        _wiggleEffectTimer += Time.deltaTime;
        float percent = Mathf.PingPong(_wiggleEffectTimer, _wiggleEffectPeriod) / _wiggleEffectPeriod;
        Vector3 scale = _wiggleEffectRoot.localScale;
        scale.x = Mathf.Lerp(_wiggleEffectScaleFactorMin, _wiggleEffectScaleFactorMax, percent);
        _wiggleEffectRoot.localScale = scale;
    }
}