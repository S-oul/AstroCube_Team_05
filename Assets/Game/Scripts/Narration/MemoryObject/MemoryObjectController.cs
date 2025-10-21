using NaughtyAttributes;
using UnityEngine;
using UnityEngine.VFX;

public class MemoryObjectController : MonoBehaviour
{
    [SerializeField] private VisualEffect _vfx;
    [SerializeField] private GameObject _origin;

    void Start()
    {
        LinkOriginToVFX();
    }

    [Button("Link Origin To VFX")]
    public void LinkOriginToVFX()
    {
        _vfx.SetVector3("Origin", _origin.transform.localPosition);
        Debug.Log(_origin.transform.localPosition);
    }
}
