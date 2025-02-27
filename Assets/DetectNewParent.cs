using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DetectNewParent : MonoBehaviour
{
    [SerializeField] LayerMask _detectableLayer;

    //private void OnTriggerEnter(Collider other)
    //{
    //    transform.parent.SetParent(other.transform);
    //}

    private void Update()
    {
        RaycastHit _raycastInfo;

        if (Physics.Raycast(transform.position, -transform.up, out _raycastInfo, 100, _detectableLayer))
        {
            transform.parent.SetParent(_raycastInfo.collider.gameObject.transform);
        }
    }
}
