using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.VFX;

public class MemoryVFXController : MonoBehaviour
{
    [SerializeField] private VisualEffect _vfx;
    [SerializeField] private float _animationDuration;
    [SerializeField] private float _stayDuration;
    
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
        if(_LDElement)
            _LDElement.SetActive(false);
    }

    public void StartVFX(GameObject objectToActivate)
    {
        if (_vfx)
        {
            StartCoroutine(PlayAnimation());
            _vfx.Play();
        }
    }

    [Button("Link Origin To VFX")]
    public void LinkOriginToVFX()
    {
        _LDElement = GetComponentInParent<MemoryObject>().GameObjectToActivate;
        _origin = transform.Find("Origin - VFX");
        _vfx.SetVector3("Origin", _origin.transform.localPosition);
        if (_LDElement)
        {
            _vfx.SetMesh("LD_Element", _LDElement.GetComponent<MeshFilter>().sharedMesh);
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
        
        yield return new WaitForSeconds(_stayDuration);

        if (_LDElement)
        {
            yield return DOTween
            .To(() => _vfx.GetFloat("Lerp_Delta"), (t) => _vfx.SetFloat("Lerp_Delta", t), 1f, _animationDuration)
            .SetEase(Ease.InOutCubic).WaitForCompletion();
            _LDElement.GetComponent<MeshRenderer>().enabled = false;
            _LDElement.SetActive(true);
        }
        _vfx.Stop();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(_origin)
            Gizmos.DrawWireSphere(_origin.position, 0.1f);
    }
}
