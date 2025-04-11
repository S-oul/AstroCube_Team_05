using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableRubiksCube : MonoBehaviour, IHoldable
{
    [SerializeField] private Transform _exitDoor;
    private Transform _originalParent;
    private Transform _originalTransform;
    private Rigidbody _rb;

    void Start()
    {
        _originalParent = transform.parent;
        LayerMask maskCube = LayerMask.GetMask("Holdable");
        _rb = GetComponent<Rigidbody>();
        _exitDoor.tag = "Untagged";
    }

    public Transform GetOriginalParent()
    {
        return _originalParent;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void OnHold(Transform newParent)
    {
        _exitDoor.tag = "VictoryZone";
        Destroy(transform.GetComponent<BoxCollider>());
        StartCoroutine(HoldRubiksCube(newParent));
    }

    IEnumerator HoldRubiksCube(Transform newParent)
    {
        InputHandler.Instance.CanMove = false;
        transform.DOMove(newParent.position, 1.0f);
        transform.DOScale(newParent.localScale, 1.0f);
        yield return new WaitForSeconds(1.0f);
        transform.parent = newParent;

        GameManager.Instance.StartSequence();
    }

    public void OnRelease() { return; }
}
