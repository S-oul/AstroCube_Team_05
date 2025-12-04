using System;
using DG.Tweening;
using UnityEngine;

public class DoorClosing : MonoBehaviour
{
    [SerializeField] private Collider _triggerCollider;
    [SerializeField] private Collider _blockCollider;
    [SerializeField] private GameObject _cube;
    
    [SerializeField] private FMODUnity.EventReference _doorStartCloseSound;
    [SerializeField] private FMODUnity.EventReference _doorEndCloseSound;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            Destroy(_triggerCollider);
            _blockCollider.enabled = true;
            
            if (!_doorStartCloseSound.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(_doorStartCloseSound, transform.position);
            }

            _cube.transform.DOLocalMoveY(0f, 6f).SetEase(Ease.InCubic).OnComplete(() => 
            {
                if (!_doorEndCloseSound.IsNull)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(_doorEndCloseSound, transform.position);
                }
            });
        }
    }
}
