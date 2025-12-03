using System;
using DG.Tweening;
using UnityEngine;

public class DoorClosing : MonoBehaviour
{

    [SerializeField] private Collider _blockCollider;
    [SerializeField] private GameObject _cube;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            _blockCollider.enabled = true;
            _cube.transform.DOLocalMoveY(0f, 6f).SetEase(Ease.InCubic);
        }
    }
}
