using System;
using System.Collections;
using System.Linq;
using RubiksStatic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InteractLine : MonoBehaviour, IHoldable
{
    [SerializeField] private EntitySequenceManager _entityOverlay;
    [SerializeField] RubiksMovement _UICube;

    public UnityEvent onPlayerActivate;

    public bool IsLevel8;
    
    private void Start()
    {
        var xxx = _UICube.GetCubesFromFace(_UICube.AllBlocks[IsLevel8? 0 :23], SliceAxis.X);
        print(xxx.Count);
        foreach (var block in xxx)
        {
            var ani = block.GetComponentInChildren<Animator>();
                ani.gameObject.transform.localScale = Vector3.zero;
                ani.SetBool("ImFar",true);
        }
    }

    public void OnHold(Transform newParent)
    {
        onPlayerActivate?.Invoke();
        _entityOverlay.gameObject.SetActive(true);
    }


    public void CallCoroutine()
    {
        StartCoroutine(WaitTimeForSequence());
    }
    IEnumerator WaitTimeForSequence()
    {
        foreach (Transform block in transform)
        {
            block.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(2f);
        var xxx = _UICube.GetCubesFromFace(_UICube.AllBlocks[23], SliceAxis.X);
        if(!IsLevel8)
        {
            foreach (var T in _UICube.GetCubesFromFace(_UICube.AllBlocks[24], SliceAxis.X))
            {
                var ani = T.GetComponentInChildren<Animator>();
                ani?.SetBool("ImFar",false);
                ani?.SetTrigger("DoAttach");
            }
        }
        else
        {
            xxx = _UICube.AllBlocks;
        }
        
        foreach (var block in xxx)
        {
            var ani = block.GetComponentInChildren<Animator>();
             ani?.SetBool("ImFar",false);
             ani?.SetTrigger("DoAttach");
        }
    }
    
    public void OnRelease()
    {
        return;
    }

    public Transform GetTransform()
    {
        return null;
    }

    public Transform GetOriginalParent()
    {
        return null;
    }
}
