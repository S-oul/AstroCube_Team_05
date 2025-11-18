using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.VFX;

public class MemoryVFXController : MonoBehaviour
{
    [SerializeField] private VisualEffect _vfx;
    [SerializeField] private float _animationDuration;
    [SerializeField] private float _stayDuration;
    [SerializeField] private bool _spawnsLDElement;
    
    private Transform _origin;
    private GameObject _LDElement;

    /*
     Lerp_Delta:
    0 = At Origin
    0.5 = At character   STAY DURING _stayDuration
    1 : At LD Elements     STAY INDEFINITELY
    */

    void Start()
    {
        LinkOriginToVFX();
    }

    public void StartVFX(GameObject objectToActivate)
    {
        _spawnsLDElement = objectToActivate;

        if (objectToActivate)
        {
            _LDElement = objectToActivate;
            _LDElement.SetActive(false);
        }
        
        if (_vfx)
        {
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
    }

    private IEnumerator PlayAnimation()
    {
        yield return DOTween
            .To(() => _vfx.GetFloat("Lerp_Delta"), (t) => _vfx.SetFloat("Lerp_Delta", t), 0.5f, _animationDuration)
            .SetEase(Ease.InOutCubic).WaitForCompletion();
        
        yield return new WaitForSeconds(_stayDuration);

        if (_LDElement)
        {
            yield return DOTween
                .To(() => _vfx.GetFloat("Lerp_Delta"), (t) => _vfx.SetFloat("Lerp_Delta", t), 1f, _animationDuration)
                .SetEase(Ease.InOutCubic).WaitForCompletion();
            
            _LDElement.GetComponent<MeshRenderer>().enabled = false;
            _LDElement.SetActive(true);
        }
        else
        {
            yield return DOTween
                .To(() => _vfx.GetFloat("Lerp_Delta"), (t) => _vfx.SetFloat("Lerp_Delta", t), 0f, _animationDuration)
                .SetEase(Ease.InOutCubic).WaitForCompletion();
            
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(_origin)
            Gizmos.DrawWireSphere(_origin.position, 0.1f);
    }
}
