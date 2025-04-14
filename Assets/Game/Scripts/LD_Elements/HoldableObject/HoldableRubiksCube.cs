using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableRubiksCube : MonoBehaviour, IHoldable
{
    [SerializeField] private GameObject _exitDoor;
    [SerializeField] private Light _light;
    private Transform _originalParent;
    private Transform _originalTransform;
    private Rigidbody _rb;

    void Start()
    {
        _originalParent = transform.parent;
        LayerMask maskCube = LayerMask.GetMask("Holdable");
        _rb = GetComponent<Rigidbody>();
        _exitDoor.SetActive(false);
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
        _exitDoor.SetActive(true);
        Destroy(transform.GetComponent<BoxCollider>());
        StartCoroutine(HoldRubiksCube(newParent));
    }

    IEnumerator HoldRubiksCube(Transform newParent)
    {
        InputHandler.Instance.CanMove = false;
        DOTween.To(() => _light.intensity, x => _light.intensity = x, 0f, 1).SetEase(Ease.OutCirc);
        transform.DOMove(newParent.position, 1.0f);
        transform.DOScale(newParent.localScale, 1.0f);
        yield return new WaitForSeconds(1.0f);
        transform.parent = newParent;
        GameManager.Instance.StartSequence();
    }

    public void OnRelease() { return; }
}
