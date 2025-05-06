using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableRubiksCube : MonoBehaviour, IHoldable
{
    [SerializeField] private GameObject _exitDoor;
    [SerializeField] private Light _light;
    [SerializeField] private InputDisplay inputDisplay;
    [SerializeField] private GameActionsSequencer _sequencer;
    private Transform _originalParent;
    private Transform _originalTransform;
    private Rigidbody _rb;

    public Action PickUpDelegate { get; set; }

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
        inputDisplay.OnResolve?.Invoke();
        PickUpDelegate?.Invoke();
        if(_sequencer != null) _sequencer.Play();
        StartCoroutine(HoldRubiksCube(newParent));
    }

    IEnumerator HoldRubiksCube(Transform newParent)
    {
        InputHandler.Instance.CanMove = false;
        DOTween.To(() => _light.intensity, x => _light.intensity = x, 0f, 1).SetEase(Ease.OutCirc);

        transform.DOMove(newParent.position + new Vector3(-1.6f,0,0),4.0f);
        transform.DOScale(newParent.localScale*2, 1.0f);
        transform.DORotateQuaternion(new Quaternion(.45f,24,4,1), 7.0f);

        yield return new WaitForSeconds(1.0f);
        transform.parent = newParent;
        Quaternion angle = Quaternion.LookRotation(Camera.main.transform.position - _exitDoor.transform.position); // Look to exit door
        angle.y = Mathf.Round(angle.y / 90) * 90;
        GameManager.Instance.StartSequence(angle);
    }

    public void OnRelease() { return; }
}