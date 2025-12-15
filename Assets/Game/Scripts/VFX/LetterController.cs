using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LetterController : MonoBehaviour
{
    [SerializeField] private float _lineWriteState = 0.0f;
    [SerializeField] private float _lineDistortState = 0.0f;
    [SerializeField] private Image _letter;
    private Material _mat;

    void Start()
    {
        _mat = _letter.material;
    }

    void Update()
    {
        _mat.SetFloat("_LineWrite", _lineWriteState);
        _mat.SetFloat("_LineDistortion", _lineDistortState);
    }
}
