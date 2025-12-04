using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LetterController : MonoBehaviour
{
    [SerializeField] private float _fxSlider = 0.0f;
    [SerializeField] private Image _letter;
    [SerializeField] private StudioEventEmitter _emitter;
    private Material _mat;

    void Start()
    {
        _emitter.enabled = true;
        _mat = _letter.material;
    }

    void Update()
    {
        _mat.SetFloat("_DebugSlider", _fxSlider);
    }
}
