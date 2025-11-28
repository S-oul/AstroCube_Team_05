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

    /*
     Lerp_Delta:
    0 = At Origin
    0.5 = At character   STAY DURING _stayDuration
    1 : At LD Elements     STAY INDEFINITELY
    */

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
            _vfx.SetVector3("Origin_LD_Element_Position", _LDElement.transform.localPosition);
            _vfx.SetVector3("Origin_LD_Element_Rotation", _LDElement.transform.rotation.eulerAngles);
            _vfx.SetVector3("Origin_LD_Element_Scale", _LDElement.transform.localScale);
        }
        
        //mettre l'objet en enfant pour avoir la bonne position (local ?)
        
        
        // desactiver le mesh renderer et activer le collider apr�s toute l'animation
        
        
        //Il faut que les models 3D soient READABLE
    }

    private IEnumerator PlayAnimation()
    {
        yield return DOTween
            .To(() => _vfx.GetFloat("Lerp_Delta"), (t) => _vfx.SetFloat("Lerp_Delta", t), 0.5f, _animationDuration)
            .SetEase(Ease.InOutCubic).WaitForCompletion();

        _memoryObject.OnCharacterAnimationFinished?.Invoke();
        
        yield return new WaitForSeconds(_stayDuration);
        
        yield return DOTween
            .To(() => _vfx.GetFloat("Lerp_Delta"), (t) => _vfx.SetFloat("Lerp_Delta", t), 1f, _animationDuration)
            .SetEase(Ease.InOutCubic).WaitForCompletion();

        if (_LDElement)
        {
            _memoryObject.OnAnimationFinished?.Invoke();
            _LDElement.GetComponent<MeshRenderer>().enabled = false;
            _LDElement.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(_origin)
            Gizmos.DrawWireSphere(_origin.position, 0.1f);
    }
}
