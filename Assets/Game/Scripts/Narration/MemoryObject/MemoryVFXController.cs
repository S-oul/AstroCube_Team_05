using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.VFX;
using FMODUnity;

public class MemoryVFXController : MonoBehaviour
{
    [SerializeField] private VisualEffect _vfx;
    [SerializeField] private float _animationDuration;
    [SerializeField] private float _stayDuration;
    [SerializeField] private bool _spawnsLDElement;

    [Header("FMOD")]
    [SerializeField] private EventReference _memoryVFXEvent;
    
    private Transform _origin;
    private GameObject _LDElement;
    private MemoryObject _memoryObject;
    private Material _material;

    void Start()
    {
        LinkOriginToVFX();
        _memoryObject = GetComponentInParent<MemoryObject>();
    }

    public void StartVFX(GameObject objectToActivate)
    {
        _spawnsLDElement = objectToActivate;
        
        _LDElement = objectToActivate;
        _LDElement?.SetActive(false);
        
        if (_vfx)
        {
            if (!_memoryVFXEvent.IsNull) RuntimeManager.PlayOneShot(_memoryVFXEvent, transform.position);
            LinkOriginToVFX();
            _vfx.SetFloat("Lerp_Delta", 0.0f);
            StartCoroutine(PlayAnimation());
            _vfx.Play();
        }
    }

    [Button("Link Origin To VFX")]
    public void LinkOriginToVFX()
    {
        _origin = transform.Find("Origin - VFX");
        _vfx.SetVector3("Origin", _origin.transform.localPosition);
        if (_LDElement)
        {
            Vector3 localPosition = transform.InverseTransformPoint(_LDElement.transform.position);
            Quaternion localRotation = Quaternion.Inverse(transform.rotation) * _LDElement.transform.rotation;
            Vector3 localScale = new Vector3(
                _LDElement.transform.lossyScale.x / transform.lossyScale.x,
                _LDElement.transform.lossyScale.y / transform.lossyScale.y,
                _LDElement.transform.lossyScale.z / transform.lossyScale.z
            );

            _vfx.SetVector3("Origin_LD_Element_Position", localPosition);
            _vfx.SetVector3("Origin_LD_Element_Rotation", localRotation.eulerAngles);
            _vfx.SetVector3("Origin_LD_Element_Scale", localScale);
        }
    }

    private void LateUpdate()
    {
        if (_LDElement)
        {
            _material = _LDElement.GetComponent<MeshRenderer>().material;
            
            Vector3 localPosition = transform.InverseTransformPoint(_LDElement.transform.position);
            Quaternion localRotation = Quaternion.Inverse(transform.rotation) * _LDElement.transform.rotation;
            Vector3 localScale = new Vector3(
                _LDElement.transform.lossyScale.x / transform.lossyScale.x,
                _LDElement.transform.lossyScale.y / transform.lossyScale.y,
                _LDElement.transform.lossyScale.z / transform.lossyScale.z
            );

            _vfx.SetVector3("Origin_LD_Element_Position", localPosition);
            _vfx.SetVector3("Origin_LD_Element_Rotation", localRotation.eulerAngles);
            _vfx.SetVector3("Origin_LD_Element_Scale", localScale);
        }
    }

    private IEnumerator PlayAnimation()
    {
        yield return DOTween
            .To(() => _vfx.GetFloat("Lerp_Delta"), (t) => _vfx.SetFloat("Lerp_Delta", t), 0.5f, _animationDuration)
            .SetEase(Ease.InOutCubic).WaitForCompletion();

        _memoryObject.OnCharacterAnimationFinished?.Invoke();
        
        yield return new WaitUntil(() => _memoryObject.IsSkipped, new TimeSpan(0, 0, 0, (int) _stayDuration, (int)(_stayDuration % 1.0f)), () => {}, WaitTimeoutMode.InGameTime);
        
        if (_LDElement)
        {
            DOTween
                .To(() => _vfx.GetFloat("ParticleSizeMultiplier"), (t) => _vfx.SetFloat("ParticleSizeMultiplier", t), 0.3f, _animationDuration)
                .SetEase(Ease.InOutCubic);
            yield return DOTween
                .To(() => _vfx.GetFloat("Lerp_Delta"), (t) => _vfx.SetFloat("Lerp_Delta", t), 1f, _animationDuration)
                .SetEase(Ease.InOutCubic).WaitForCompletion();
            
            DOTween
                .To(() => _material.GetFloat(Shader.PropertyToID("_Alpha")), (t) => _material.SetFloat(Shader.PropertyToID("_Alpha"), t), 1f, 0.5f)
                .SetEase(Ease.InOutCubic);
            
            _memoryObject.OnAnimationFinished?.Invoke();
            _LDElement.SetActive(true);
        }
        else
        {
            DOTween
                .To(() => _vfx.GetFloat("ParticleSizeMultiplier"), (t) => _vfx.SetFloat("ParticleSizeMultiplier", t), 0, _animationDuration)
                .SetEase(Ease.InOutCubic).WaitForCompletion();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(_origin)
            Gizmos.DrawWireSphere(_origin.position, 0.1f);
    }
}
